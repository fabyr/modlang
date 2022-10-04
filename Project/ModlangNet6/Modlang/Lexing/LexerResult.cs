using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Modlang.Lexing
{
    public class LexerResult : IEnumerable<Token>
    {
        private List<Token> _list = new List<Token>();

        public void Add(Token t)
        {
            _list.Add(t);
        }

        public void Remove(Token t)
        {
            _list.Remove(t);
        }

        public void RemoveAt(int idx)
        {
            _list.RemoveAt(idx);
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public Token this[int i]
        {
            // If the index is below 0 or higher than the available token-list a special "UNDEFINED" token is returned
            // if there are no tokens to begin with we can return just a token with absolutely nothing
            get => _list.Count == 0 ? new Token() : i < _list.Count && i >= 0 ? _list[i] : new Token((FromFileObject)this[Math.Max(0, Math.Min(_list.Count - 1, i))], "", TokenType.UNDEFINED);
            //set => _list[i] = value;
        }

        // This was mainly used for testing purposes
        // It basically creates a string of all the tokens in order, so we can inspect them in the console window better xD
        public string BuildDisplayString()
        {
            StringBuilder sb = new StringBuilder();
            long line = 1;
            foreach(Token t in this)
            {
                if(line < t.Line) // if the line of the current token is greater then the previous,
                                  // we also add a line in the output string, just so it looks better ;)
                {
                    sb.AppendLine();
                    line = t.Line;
                }
                sb.Append(t);
                sb.Append(" ");
            }
            if(sb.Length > 0)
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}