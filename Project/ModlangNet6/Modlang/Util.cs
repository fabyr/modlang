using Modlang.Abstract;
using Modlang.Abstract.Literals;
using Modlang.Abstract.Types;
using Modlang.Lexing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Modlang
{
    public static class Util
    {
        // Checks whether an expression is describing a identifier which is either a pure identifier or composed of period-connected identifiers
        // if allowArray is true then empty square braces are allowed to follow, which would describe an array type
        public static bool IsIdentifyingExpression(Expression exp, bool allowArray = false)
        {
            return exp is IdentifierExpression || 
                (exp is BinaryOperatorExpression boe && 
                boe.Operator == "." && IsIdentifyingExpression(boe.Left) && IsIdentifyingExpression(boe.Right))
                || (allowArray && (exp is BraceExpression bexp && bexp.Kind == BraceType.SQUARE && bexp.Arguments?.Length == 0));
        }

        // parses a binary number string into the actual object.
        // (without the leading 0b)
        private static object ParseBinNumber(string number)
        {
            ulong result = 0ul;

            // based on the length a different datatype is selected (the same as in C#)
            

            // actually transforming it into a number bit for bit
            for (int i = 0; i < number.Length; i++)
                result |= (ulong)(number[i] == '0' ? 0 : 1) << (number.Length - i - 1);

            if (number.Length <= 31)
            {
                return (int)result;
            }
            else if (number.Length == 32)
            {
                return (uint)result;
            }
            else if (number.Length <= 63)
            {
                return (long)result;
            }
            else
            {
                return (ulong)result;
            }
            //return result;
        }

        // Huge method to make the corresponding Expression-Type out of a Literal-Token
        public static Expression BuildLiteral(Token t)
        {
            switch (t.Kind)
            {
                case TokenType.KEYWORD_TRUE:
                    return new BoolLiteralExpression(true);
                case TokenType.KEYWORD_FALSE:
                    return new BoolLiteralExpression(false);
                case TokenType.LITERAL_STRING:
                    return new StringLiteralExpression(t.Content); // Pretty easy for strings
                case TokenType.LITERAL_NUMBER: // Absolute hell, because depending on the size and the the pre and suffixes a different numeric type has to be selected
                    // Aaaand i'm to lazy to actually comment all the stuff, should be pretty easy to understand on a higher level tho
                    {
                        string cont = t.Content;
                        bool hex = cont.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase);
                        if (!hex && (cont.Contains(".") || cont.EndsWith("f", StringComparison.InvariantCultureIgnoreCase)
                            || cont.EndsWith("d", StringComparison.InvariantCultureIgnoreCase)
                            || cont.IndexOf("e", StringComparison.InvariantCultureIgnoreCase) >= 0)) // floating point
                        {
                            if(cont.EndsWith("f", StringComparison.InvariantCultureIgnoreCase))
                            {
                                cont = cont.Substring(0, cont.Length - 1);
                                return new FloatLiteralExpression(float.Parse(cont, CultureInfo.InvariantCulture));
                            } else if(cont.EndsWith("d", StringComparison.InvariantCultureIgnoreCase))
                                cont = cont.Substring(0, cont.Length - 1);

                            return new DoubleLiteralExpression(double.Parse(cont, CultureInfo.InvariantCulture));
                        }

                        bool forceUL = cont.EndsWith("ul", StringComparison.InvariantCultureIgnoreCase);
                        bool forceL = !forceUL && cont.EndsWith("l", StringComparison.InvariantCultureIgnoreCase);
                        bool forceU = cont.EndsWith("u", StringComparison.InvariantCultureIgnoreCase);

                        if (forceUL)
                            cont = cont.Substring(0, cont.Length - 2);
                        if (forceL || forceU)
                            cont = cont.Substring(0, cont.Length - 1);

                        if (hex)
                        {
                            string numPart = cont.Substring(2);

                            if(forceUL)
                                return new ULongIntegerLiteralExpression(ulong.Parse(numPart, System.Globalization.NumberStyles.HexNumber));
                            if(forceL)
                                return new LongIntegerLiteralExpression(long.Parse(numPart, System.Globalization.NumberStyles.HexNumber));
                            if(forceU)
                                return new UIntegerLiteralExpression(uint.Parse(numPart, System.Globalization.NumberStyles.HexNumber));

                            if (numPart.Length <= 7)
                                return new IntegerLiteralExpression(int.Parse(numPart, System.Globalization.NumberStyles.HexNumber));
                            if (numPart.Length == 8)
                                return new UIntegerLiteralExpression(uint.Parse(numPart, System.Globalization.NumberStyles.HexNumber));
                            if (numPart.Length <= 15)
                                return new LongIntegerLiteralExpression(long.Parse(numPart, System.Globalization.NumberStyles.HexNumber));
                            return new ULongIntegerLiteralExpression(ulong.Parse(numPart, System.Globalization.NumberStyles.HexNumber));
                        }
                        if(cont.StartsWith("0b"))
                        {
                            string numPart = cont.Substring(2);
                            object parsed = ParseBinNumber(numPart);

                            if(forceUL)
                                return new ULongIntegerLiteralExpression((ulong)(int)parsed);
                            if(forceL)
                                return new LongIntegerLiteralExpression((long)(int)parsed);
                            if(forceU)
                                return new UIntegerLiteralExpression((uint)(int)parsed);

                            if (numPart.Length <= 31)
                            {
                                return new IntegerLiteralExpression((int)parsed);
                            }
                            else if (numPart.Length == 32)
                            {
                                return new UIntegerLiteralExpression((uint)parsed);
                            }
                            else if (numPart.Length <= 63)
                            {
                                return new LongIntegerLiteralExpression((long)parsed);
                            }
                            else
                            {
                                return new ULongIntegerLiteralExpression((ulong)parsed);
                            }
                        }
                        if(cont.StartsWith("0") && cont.Length > 1) // Octal
                        {
                            // TODO: COMPLETE THIS FUCKING FEATURE
                            throw new NotImplementedException("The octal feature has not been implemented yet");
                        }

                        if (forceUL)
                            return new ULongIntegerLiteralExpression(ulong.Parse(cont));
                        if (forceL)
                            return new LongIntegerLiteralExpression(long.Parse(cont));
                        if (forceU)
                            return new UIntegerLiteralExpression(uint.Parse(cont));

                        BigInteger bi = BigInteger.Parse(cont);
                        if (bi <= int.MaxValue && bi >= int.MinValue)
                            return new IntegerLiteralExpression((int)bi);

                        if (bi <= uint.MaxValue && bi >= uint.MinValue)
                            return new UIntegerLiteralExpression((uint)bi);

                        if (bi <= long.MaxValue && bi >= long.MinValue)
                            return new LongIntegerLiteralExpression((long)bi);

                        if (bi <= ulong.MaxValue && bi >= ulong.MinValue)
                            return new ULongIntegerLiteralExpression((ulong)bi);

                        throw new FormatException("Number too large");
                    }
                case TokenType.LITERAL_CHAR:
                    return new CharLiteralExpression(t.Content[0]); // also pretty easy
                default:
                    throw new FormatException("Invalid Literal"); // Someone has passed a wrong token to this method >:C
            }
        }

        public static bool IsLiteral(TokenType t)
        {
            switch (t)
            {
                case TokenType.LITERAL_STRING:
                case TokenType.LITERAL_NUMBER:
                case TokenType.LITERAL_CHAR:
                case TokenType.KEYWORD_TRUE:
                case TokenType.KEYWORD_FALSE:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsBrace(TokenType t)
        {
            switch (t)
            {
                case TokenType.LBRACE:
                case TokenType.RBRACE:
                case TokenType.CURLY_LBRACE:
                case TokenType.CURLY_RBRACE:
                case TokenType.SQUARE_LBRACE:
                case TokenType.SQUARE_RBRACE:
                    return true;
                default:
                    return false;
            }
        }

        // This was used at some point but i think not anymore because my brain has come up with better logic in the Parser
        public static bool IsOperatorOrEnding(TokenType t, bool ignoreComma)
        {
            return IsOperator(t, ignoreComma) || IsEnding(t);
        }

        public static bool IsEnding(TokenType t)
        {
            return t == TokenType.EOI || t == TokenType.UNDEFINED;
        }

        public static AccessModifier? GetModifierKeyword(TokenType t)
        {
            switch (t)
            {
                case TokenType.KEYWORD_PUBLIC:
                    return AccessModifier.PUBLIC;
                case TokenType.KEYWORD_PRIVATE:
                    return AccessModifier.PRIVATE;
                case TokenType.KEYWORD_PROTECTED:
                    return AccessModifier.PROTECTED;
                default:
                    return null;
            }
        }

        public static bool IsClosingBrace(TokenType t)
        {
            switch (t)
            {
                case TokenType.RBRACE:
                case TokenType.CURLY_RBRACE:
                case TokenType.SQUARE_RBRACE:
                    return true;
                default:
                    return false;
            }
        }

        // Checks if a brace-type matches the token-type
        // For example if the brace type is SQUARE only the SQUARE_LBRACE AND SQUARE_RBRACE will return true
        public static bool CheckBrace(TokenType t, BraceType bt)
        {
            switch (t)
            {
                case TokenType.LBRACE:
                case TokenType.RBRACE:
                    return bt == BraceType.ROUND;
                case TokenType.CURLY_LBRACE:
                case TokenType.CURLY_RBRACE:
                    return bt == BraceType.CURLY;
                case TokenType.SQUARE_LBRACE:
                case TokenType.SQUARE_RBRACE:
                    return bt == BraceType.SQUARE;
                default:
                    return false;
            }
        }

        public static bool IsOpeningBrace(TokenType t)
        {
            switch (t)
            {
                case TokenType.LBRACE:
                case TokenType.CURLY_LBRACE:
                case TokenType.SQUARE_LBRACE:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsOperator(TokenType tt, bool ignoreComma = false)
        {
            switch (tt)
            {
                case TokenType.OP_COMMA:
                    return !ignoreComma;
                case TokenType.KEYWORD_NEW:
                case TokenType.OPERATOR:
                case TokenType.OP_ASSIGN:
                case TokenType.OP_PERIOD:
                case TokenType.OP_COLON:
                    return true;
                default:
                    return false;
            }
        }

        // Checks if a Token matches the supplied precedence, always false if it is not an operator to begin with
        public static bool CheckTokenPrecedence(Token t, bool isUnary, bool isPrefix, int precedence, bool ignoreComma = false)
        {
            return IsOperator(t.Kind, ignoreComma)
                && GetPrecedence(t.Content, isUnary, isPrefix) == precedence;
        }

        // More details in the Parser in the "ComputeUnary" method
        public static bool ShouldBeUnaryOperator(string op, out bool? isPrefix)
        {
            switch (op)
            {
                case "new":
                case "-":
                case "+":
                case "~":
                case "!":
                    isPrefix = true; // This seems to break some stuff idfc. as long as it works in the end xd
                    //isPrefix = null;
                    return true;
                case "--":
                case "++":
                    isPrefix = null;
                    return true;
                default:
                    isPrefix = null;
                    return false;
            }
        }

        public static bool CannotBeUnaryOperator(string op)
        {
            return op.Contains("=");
        }

        public static bool IsAssignmentOperator(string op)
        {
            return op.EndsWith("=") // any assignment operator has the lowest precedence
                    && !op.Equals("==") && !op.Equals("!=") // but the equality operators shouldn't be treated as assignment operator
                    && !op.Equals("<=") && !op.Equals(">=");  // same with comparison
        }

        // Returns the precedence for any given operator
        // Attention: Precedence changes for some operators if it is unary or not (and also if the unary one is prefix or postfix)
        // This list is mostly oriented around C#'s precedence list
        // (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/#operator-precedence)
        public static int GetPrecedence(string op, bool isUnary, bool isPrefix)
        {
            /*bool? pref;
            isUnary = ShouldBeUnaryOperator(op, out pref);
            if (pref.HasValue)
                isPrefix = pref.Value;*/

            if (isUnary)
            {
                switch (op)
                {
                    case "--":
                    case "++":
                        return isPrefix ? 2 : 1;

                    case "!":
                        return 2;

                    case "new":
                        return 1; // NOT ANYMORE:// NOTE: Changed to two so it works correctly in combination with the period operator

                    default:
                        return 2;
                }
            } else
            {
                if (IsAssignmentOperator(op))  // same with comparison
                    return Constants.MaxPrecedence;

                switch(op)
                {
                    case ".":
                        return 1;

                    case "%":
                    case "/":
                    case "*":
                        return 5;

                    case "+":
                    case "-":
                        return 6;

                    case ">>":
                    case "<<":
                        return 7;

                    case "<":
                    case ">":
                    case ">=":
                    case "<=":
                        return 8;

                    case "==":
                    case "!=":
                        return 9;

                    case "&":
                        return 10;
                    case "^":
                        return 11;
                    case "|":
                        return 12;

                    case "&&":
                        return 13;
                    case "||":
                        return 14;

                    default:
                        return Constants.MaxPrecedence - 1; // any operator which is not "pre-defined" gets the lowest precedence, but still above the assignment operators
                }
            }
        }

        // Transforms an escape code (without the leading backslash) into the actual character
        public static char? EscapeCode(string code)
        {
            if(code.Length == 1)
            {
                switch (code[0])
                {
                    case '"':
                        return '"';
                    case '\\':
                        return '\\';
                    case '\'':
                        return '\'';
                    case '0':
                        return (char)0x00;
                    case 'a':
                        return (char)0x07;
                    case 'b':
                        return (char)0x08;
                    case 'f':
                        return (char)0x0c;
                    case 'n':
                        return (char)0x0a;
                    case 'r':
                        return (char)0x0d;
                    case 't':
                        return (char)0x09;
                    case 'v':
                        return (char)0x0b;
                    default:
                        return null;
                }
            } else
            {
                string core = code.Substring(1);
                int number;
                if (int.TryParse(core, NumberStyles.HexNumber, null, out number))
                {
                    switch(code[0])
                    {
                        case 'u':
                        case 'x':
                            if (core.Length > 4)
                                return null;
                            return (char)number;
                        case 'U':
                            if (core.Length > 8)
                                return null;
                            return (char)number;
                    }
                }
                return null;
            }
        }

        public static bool IsSpecialOperator(TokenType kind)
        {
            switch (kind)
            {
                //case TokenType.KEYWORD_NEW: // TODO: CHECK IF THIS IS CORRECT
                case TokenType.OP_ASSIGN:
                case TokenType.OP_COMMA:
                case TokenType.OP_COLON:
                case TokenType.OP_PERIOD:
                    return true;
                default:
                    return false;
            }
        }

        public static TokenType? GetKeyword(string word)
        {
            switch(word)
            {
                case "new": return TokenType.KEYWORD_NEW;
                case "static": return TokenType.KEYWORD_STATIC;
                case "class": return TokenType.KEYWORD_CLASS;
                case "this": return TokenType.KEYWORD_THIS;

                case "const": return TokenType.KEYWORD_CONST;

                case "if": return TokenType.KEYWORD_IF;
                case "while": return TokenType.KEYWORD_WHILE;
                case "do": return TokenType.KEYWORD_DO;
                case "for": return TokenType.KEYWORD_FOR;
                case "switch": return TokenType.KEYWORD_SWITCH;
                case "case": return TokenType.KEYWORD_CASE;
                case "default": return TokenType.KEYWORD_DEFAULT;
                case "else": return TokenType.KEYWORD_ELSE;

                case "break": return TokenType.KEYWORD_BREAK;
                case "continue": return TokenType.KEYWORD_CONTINUE;

                case "return": return TokenType.KEYWORD_RETURN;

                case "operator": return TokenType.KEYWORD_OPERATOR;
                case "prefix": return TokenType.KEYWORD_PREFIX;
                case "postfix": return TokenType.KEYWORD_POSTFIX;

                case "public": return TokenType.KEYWORD_PUBLIC;
                case "private": return TokenType.KEYWORD_PRIVATE;
                case "protected": return TokenType.KEYWORD_PROTECTED;

                case "true": return TokenType.KEYWORD_TRUE;
                case "false": return TokenType.KEYWORD_FALSE;

                case "implicit": return TokenType.KEYWORD_IMPLICIT;
                case "explicit": return TokenType.KEYWORD_EXPLICIT;

                case "get": return TokenType.KEYWORD_GET;
                case "set": return TokenType.KEYWORD_SET;

                case "primitive": return TokenType.KEYWORD_PRIMITIVE;
                default:
                    return null;
            }
        }

        // This is used in the ToString methods of various Token and Expression types.
        // So that when printing a string containing a newline for example doesn't actually print a newline but rather
        // \<16-Bit-CodePoint>
        public static string TokenDisplayEscapeContent(string content)
        {
            StringBuilder sb = new StringBuilder();
            const string additionalNonPrintables = "\"'\\";
            foreach (char c in content)
            {
                if (c >= 32 && c <= 126 && !additionalNonPrintables.Contains(c))
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append($"\\{(int)c:X4}");
                }
            }
            return sb.ToString();
        }
    }
}
