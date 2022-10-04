using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Lexing
{
    public class Token : FromFileObject
    {
        public string Content { get; }
        public TokenType Kind { get; }

        internal Token() : this(string.Empty, -1, -1, -1, -1, string.Empty, TokenType.UNDEFINED)
        { }

        internal Token(FromFileObject foo, string content, TokenType tt) 
            : base(foo.Filename, foo.Line, foo.Position, foo.Length, foo.Column)
        {
            Content = content;
            Kind = tt;
        }

        public Token(string filename, long line, long pos, long length, long ENDcol, string content, TokenType tt)
            : base(filename, line, pos, length, ENDcol - length) // subtract length from end-pos-column to get starting column
        {
            Content = content;
            Kind = tt;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is Token))
                return false;
            Token other = (Token)obj;
            return other.Position == Position;
        }

        public override string ToString()
        {
            //return $"[{Position}:{Length}; {Kind}, \"{Content}\"]";
            return $"[{Kind}:\"{Util.TokenDisplayEscapeContent(Content)}\"]";
        }
    }
}
