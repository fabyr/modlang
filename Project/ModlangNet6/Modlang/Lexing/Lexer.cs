using Modlang.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Lexing
{
    public class Lexer : IDisposable
    {
        // TODO: This could be made in a better way
        private static readonly Encoding DefaultFileEncoding = Encoding.UTF8;
        private Encoding _textEncoding;

        private enum LexerState
        {
            NORMAL,
            IN_IDENTIFIER,
            IN_STRING,
            IN_CHAR,
            IN_NUMBER,
            IN_COMMENT,
            IN_COMMENT_MULTILINE,
            IN_OPERATOR
        }

        public enum NewlineMode
        {
            CR,
            LF,
            CRLF
        }

        public NewlineMode LineBreakMode { get; set; } = NewlineMode.CRLF;

        // NOT USED ANYMORE
        //private static readonly LexerState[] ContinousStates = new[] { LexerState.IN_STRING, LexerState.IN_COMMENT, LexerState.IN_COMMENT_MULTILINE };

        private Stream _s;
        private bool _disposeOfStream = false;

        private string _filename; // The Filename appearing when an exception is thrown

        public string FileName { get => _filename; set => _filename = value; }

        public Lexer(Stream s, string origin) : this(s)
        {
            _filename = origin;
        }

        public Lexer(Stream s)
        {
            _s = s;
            _filename = s.ToString();
            _textEncoding = DefaultFileEncoding;
        }

        public Lexer(string code, Encoding enc) : this(new MemoryStream(enc.GetBytes(code)))
        {
            _textEncoding = enc;
            _disposeOfStream = true;
        }

        public Lexer(string file) : this(new FileStream(file, FileMode.Open, FileAccess.Read))
        {
            _disposeOfStream = true;
            _textEncoding = DefaultFileEncoding;
            _filename = file;
        }

        //private long Position { get => _s.Position; }

        public LexerResult Lex()
        {
            LexerResult lr = new LexerResult();
            List<string> visitedList = new List<string>();
            StreamReader sr = new StreamReader(_s, _textEncoding);
            StreamScanner sc = new StreamScanner(sr);
            LexCore(_filename, sc, lr, visitedList);
            return lr;
        }

        // Lexes the contents of the specified file. (The tokens are directly added to an existing result)
        // This gets called whenever the lexer "above in the call stack" encounters a using "<filename>";
        private void LexFile(string file, LexerResult lr, List<string> visitedList)
        {
            if (visitedList.Contains(file))
                throw new Exception($"The file \"{file}\" has already been imported at one point.");
            if(!File.Exists(file))
                throw new Exception($"The file \"{file}\" cannot be found or accessed.");
            visitedList.Add(file);
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using(StreamReader sr = new StreamReader(fs, DefaultFileEncoding))
                {
                    StreamScanner sc = new StreamScanner(sr);
                    LexCore(file, sc, lr, visitedList);
                }
            }
        }

        private void LexCore(string file, StreamScanner sc, LexerResult lr, List<string> visitedList)
        {
            LexerResult res = lr;
            LexerState state = LexerState.NORMAL; // Stores the context "we are in". basically different behaviour flag if we are inside a string for example
            StringBuilder buffer = new StringBuilder();

            bool inEscapeSequence = false; // when in String or Char context, this stores if inside that context we are in an escape-sequence-context
            StringBuilder escapeBuffer = new StringBuilder();

            long startpos = 0;
            bool inUsingMode = false; // this stores if we are in a "using" context, basically the next string the lexer encounters will be interpreted as the file for a using statement.
            
            // A bunch of variables keeping track of where we are (for proper exception messages later on)
            long atLine = 1;
            long atColumn = 1;
            long charPos = 0; // total position inside file

            int dontFetch = 0; // this is needed to reprocess one character, for example if a terminating character to an identifier has been reached, the context is reset and this character should be processed again
            // this is only valid for N loop-iterations, as it is decremented there immediately

            bool disableNextEscape = false; // this is needed for a \\ escape sequence, because the window would register the second backslash as the start of another escape sequence
            char cur = '\0';

            void ProcessUsing(string content)
            {
                state = LexerState.NORMAL;
                inUsingMode = false;
                try
                {
                    LexFile(content, lr, visitedList);
                }
                catch (Exception ex)
                {
                    throw new LexerException(file, atLine, charPos, charPos - startpos, atColumn, ex.Message);
                }
            }

            while(!sc.EndOfStream || dontFetch != 0 || (state != LexerState.NORMAL && state != LexerState.IN_COMMENT))
            {
                if (dontFetch == 0)
                {
                    cur = sc.NextChar();
                    /*if (ReplaceTabsWithSpaces && cur == '\t')
                    {
                        cur = ' ';
                        dontFetch += 4;
                    }*/
                    atColumn++;
                    charPos++;
                }
                else
                    dontFetch--;

                //bool whitespaceWindow = char.IsWhiteSpace(cur);
                bool newlineWindow;
                switch (LineBreakMode) // I made it so the Line-Break mode can be controlled by the user
                {
                    case NewlineMode.CR:
                        newlineWindow = cur == '\r';
                        break;
                    case NewlineMode.LF:
                        newlineWindow = cur == '\n';
                        break;
                    case NewlineMode.CRLF:
                        newlineWindow = sc.MatchWindow("\r\n", -1);
                        break;
                    default:
                        newlineWindow = false;
                        break;
                }

                bool escapeWindow = !disableNextEscape && sc.MatchWindow("\\", -1);
                if(disableNextEscape) disableNextEscape = false; // the disable escape is also only valid for the next loop iteration

                bool stringWindow = sc.MatchWindow("\"", 0);
                bool charWindow = sc.MatchWindow("\'", 0);

                // Comment-exit handling
                if (newlineWindow && state == LexerState.IN_COMMENT)
                    state = LexerState.NORMAL;
                else if (state == LexerState.IN_COMMENT_MULTILINE && sc.MatchWindow("*/", -1))
                    state = LexerState.NORMAL;

                // If in normal mode, here are all the characters which can let the lexter enter a different context
                else if (state == LexerState.NORMAL)
                {
                    // Due to the inability of looking in the front the "/" and "*" characters are initially treated as operators
                    // It will be checked for comments down in the Operator section
                    /*if (sc.MatchWindow("//", -1)) // comment start
                    {
                        state = LexerState.IN_COMMENT;
                    }
                    else if (sc.MatchWindow("/*", -1)) // Multiline comment start
                    {
                        state = LexerState.IN_COMMENT_MULTILINE;
                    }else*/
                    if (Constants.IdentifierChars.Contains(cur)) // Identifier start
                    {
                        state = LexerState.IN_IDENTIFIER;
                        startpos = charPos;
                        buffer.Append(cur); // append the first identifier char
                    }
                    else if(stringWindow) // String Start
                    {
                        state = LexerState.IN_STRING;
                        startpos = charPos;
                        //buffer.Append(cur); // don't append the "
                    }
                    else if (charWindow) // Char start
                    {
                        state = LexerState.IN_CHAR;
                        startpos = charPos;
                        //buffer.Append(cur); // don't append the '
                    }
                    else if(Constants.NumberCharset.Contains(cur)) // Number Start
                    {
                        state = LexerState.IN_NUMBER;
                        startpos = charPos;
                        buffer.Append(cur);
                    } else if(cur == Constants.EOI) // Semicolon
                    {
                        res.Add(new Token(file, atLine, charPos, 1, atColumn, Constants.EOI.ToString(), TokenType.EOI));
                    }
                    else if (cur == Constants.CommaOp)
                    {
                        res.Add(new Token(file, atLine, charPos, 1, atColumn, ",", TokenType.OP_COMMA));
                    }
                    else if(!Constants.LexNonOperatorCharset.Contains(cur)) // Operator
                    {
                        state = LexerState.IN_OPERATOR;
                        startpos = charPos;
                        buffer.Append(cur);

                        
                    }
                    //  (Removed and put down to the actual operator code)
                    /*else if(cur == Constants.AssignmentOperator) // Assignment operator is treated as special
                    {
                        res.Add(new Token(file, atLine, charPos, 1, atColumn, Constants.AssignmentOperator.ToString(), TokenType.OP_ASSIGN));
                    }
                    else if (cur == Constants.PeriodOp) // period is also treated as a special operator
                    {
                        res.Add(new Token(file, atLine, charPos, 1, atColumn, ".", TokenType.OP_PERIOD));
                    }
                    else if (cur == Constants.CommaOp) // same with comma
                    {
                        res.Add(new Token(file, atLine, charPos, 1, atColumn, ",", TokenType.OP_COMMA));
                    }*/

                    // All the different braces
                    else if (cur == '(')
                    {
                        res.Add(new Token(file, atLine, charPos, 1, atColumn, "(", TokenType.LBRACE));
                    }
                    else if (cur == ')')
                    {
                        res.Add(new Token(file, atLine, charPos, 1, atColumn, ")", TokenType.RBRACE));
                    }
                    else if (cur == '[')
                    {
                        res.Add(new Token(file, atLine, charPos, 1, atColumn, "[", TokenType.SQUARE_LBRACE));
                    }
                    else if (cur == ']')
                    {
                        res.Add(new Token(file, atLine, charPos, 1, atColumn, "]", TokenType.SQUARE_RBRACE));
                    }
                    else if (cur == '{')
                    {
                        res.Add(new Token(file, atLine, charPos, 1, atColumn, "{", TokenType.CURLY_LBRACE));
                    }
                    else if (cur == '}')
                    {
                        res.Add(new Token(file, atLine, charPos, 1, atColumn, "}", TokenType.CURLY_RBRACE));
                    }
                }
                // Code for Char and String is mostly the same, so no need to copy it.
                // If in Char context at the end the length is asserted to be 1
                else if (state == LexerState.IN_STRING || state == LexerState.IN_CHAR)
                {
                    if (inEscapeSequence)
                    {
                        // "inEscapeSequence" is only set when we deal with a multi-character sequence, which can only be a unicode-escape sequence
                        if ( (escapeBuffer[0] == 'u' && escapeBuffer.Length > Constants.UnicodeEscapeSquenceLength) ||
                             (escapeBuffer[0] == 'x' && (escapeBuffer.Length > Constants.UnicodeEscapeSquenceMaxLengthVariable || !Constants.HexCharset.Contains(cur))) ||
                             (escapeBuffer[0] == 'U' && escapeBuffer.Length > Constants.UnicodeEscapeSquenceLength32) )
                        {
                            inEscapeSequence = false;
                            char? code = Util.EscapeCode(escapeBuffer.ToString());
                            if (!code.HasValue)
                                throw new LexerException(file, atLine, charPos, charPos - escapeBuffer.Length, atColumn, $"Invalid Unicode-Escape Sequence \"\\{cur}\"");
                            buffer.Append(code.Value);
                            escapeBuffer.Clear();
                            dontFetch++;
                        }
                        else
                            escapeBuffer.Append(cur);
                    }
                    else if (escapeWindow)
                    {
                        if (stringWindow || charWindow) // if we have \' or \"
                        {
                            buffer.Remove(buffer.Length - 1, 1); // we can basically just remove the backslash
                            buffer.Append(cur);
                        }
                        else if (cur == 'u' || cur == 'U' || cur == 'x') // unicode escape sequence
                        {
                            inEscapeSequence = true;
                            buffer.Remove(buffer.Length - 1, 1);
                            escapeBuffer.Append(cur);
                        }
                        else
                        {
                            char? code = Util.EscapeCode(cur.ToString()); // code will be null, if the escape sequence is invalid
                            if (!code.HasValue)
                                throw new LexerException(file, atLine, charPos, 1, atColumn, $"Invalid Escape Sequence \"\\{cur}\"");

                            buffer.Remove(buffer.Length - 1, 1);
                            buffer.Append(code.Value);
                            disableNextEscape = true;
                        }
                    }

                    // Here is now the code for when the string is finished.
                    // Due to the "using"-prossibility more code is needed to check for that
                    else if(stringWindow && state == LexerState.IN_STRING)
                    {
                        string content = buffer.ToString();
                        if(inUsingMode)
                        {
                            ProcessUsing(content);
                        } else
                        {
                            state = LexerState.NORMAL;
                            res.Add(new Token(file, atLine, startpos, charPos - startpos, atColumn, content, TokenType.LITERAL_STRING));
                        }
                        buffer.Clear();
                    }
                    else if (charWindow && state == LexerState.IN_CHAR)
                    {
                        string content = buffer.ToString();
                        if (content.Length != 1)
                            throw new LexerException(file, atLine, charPos, charPos - startpos, atColumn, "Character literal must be of length 1");
                        state = LexerState.NORMAL;
                        res.Add(new Token(file, atLine, startpos, charPos - startpos, atColumn, content, TokenType.LITERAL_CHAR));
                        buffer.Clear();
                    }
                    else // if none of the above apply, we just append the current character to the string
                        buffer.Append(cur);
                }

                // The Identifier code is pretty similar to the string stuff, just that the delimiting characters are not " or '
                // rather they are any characters which are not in a predefined charset (Constants.IdentifierCenterChars)
                else if (state == LexerState.IN_IDENTIFIER)
                {
                    if (Constants.IdentifierCenterChars.Contains(cur))
                        buffer.Append(cur);
                    else
                    {
                        state = LexerState.NORMAL;
                        string content = buffer.ToString();
                        if(inUsingMode) // using base64; = using "base64.mdl";
                        {
                            ProcessUsing(content + ".mdl");
                        } else
                        if(content == "using") // if it is that Keyword, set the flag (which treats the next string as the file path)
                        {
                            inUsingMode = true;
                        } else
                        {
                            TokenType? tt = Util.GetKeyword(content);

                            // if we can't get any keyword (tt is null), then we default to regular IDENTIFIER
                            res.Add(new Token(file, atLine, startpos, charPos - startpos, atColumn, content, tt ?? TokenType.IDENTIFIER));
                        }
                        buffer.Clear();

                        dontFetch++; // this has to be incremented, so that the character which stopped the identifier-context gets processed in the next loop iteration
                    }
                } else if(state == LexerState.IN_NUMBER) // also the same as Identifier, just with different characters and delimiting characters
                {
                    if (Constants.NumberCenterCharset.Contains(cur))
                    {
                        buffer.Append(cur);
                    } else
                    {
                        state = LexerState.NORMAL;
                        res.Add(new Token(file, atLine, startpos, charPos - startpos, atColumn, buffer.ToString(), TokenType.LITERAL_NUMBER));
                        buffer.Clear();

                        dontFetch++;
                    }

                } else if(state == LexerState.IN_OPERATOR) // In my language, basically every character which is not in a "forbidden" charset, is treated as an operator (Constants.LexNonOperatorCharset and Constants.NonOperatorCharset)
                {
                    if(!Constants.LexNonOperatorCharset.Contains(cur))
                    {
                        buffer.Append(cur);
                    } else
                    {
                        state = LexerState.NORMAL;
                        string content = buffer.ToString();
                        if (content == "//")
                            state = LexerState.IN_COMMENT;
                        else if (content == "/*")
                            state = LexerState.IN_COMMENT_MULTILINE;
                        else if(content == Constants.AssignmentOperatorStr)
                            res.Add(new Token(file, atLine, charPos, 1, atColumn, Constants.AssignmentOperator.ToString(), TokenType.OP_ASSIGN));
                        else if(content == Constants.ColonOperatorStr)
                            res.Add(new Token(file, atLine, charPos, 1, atColumn, Constants.ColonOperator.ToString(), TokenType.OP_COLON));
                        else if (content == Constants.PeriodOpStr)
                            res.Add(new Token(file, atLine, charPos, 1, atColumn, Constants.PeriodOp.ToString(), TokenType.OP_PERIOD));
                        else if (content == Constants.CommaOpStr)
                            res.Add(new Token(file, atLine, charPos, 1, atColumn, Constants.CommaOp.ToString(), TokenType.OP_COMMA));
                        else
                            res.Add(new Token(file, atLine, startpos, charPos - startpos, atColumn, content, TokenType.OPERATOR));
                        buffer.Clear();

                        dontFetch++;
                    }
                }

                if (newlineWindow) // if we have encountered a newline, reset the column counter and increment the line-number
                {
                    atLine++;
                    atColumn = 0;
                }
            }
        }

        public void Dispose()
        {
            if (_disposeOfStream)
                _s.Dispose();
        }
    }
}
