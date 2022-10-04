using Modlang.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Exceptions
{
    public class LexerException : ModlangException
    {
        public LexerException(FromFileObject subject, string msg) : base(subject, msg)
        {
        }

        public LexerException(string filename, long line, long pos, long length, long col, string msg) 
            : this(new FromFileObject(filename, line, pos, length, col), msg)
        {
        }

        public LexerException(Token t, string msg) : this((FromFileObject)t, msg)
        { }

        public override string ToString()
        {
            return $"{Origin.Filename}:{Origin.Line},{Origin.Column}\r\n\t{Message.Replace("\n", "\n\t")}";
        }
    }
}
