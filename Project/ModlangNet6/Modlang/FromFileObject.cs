using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang
{
    public class FromFileObject // This can be used anywhere where positional info inside the source file is required
    {
        public string Filename { get; }
        public long Position { get; }
        public long Column { get; }
        public long Line { get; }
        public long Length { get; }
        public long EndPos { get => Position + Length; }

        public FromFileObject(string filename, long line, long pos, long length, long col)
        {
            Filename = filename;
            Position = pos;
            Line = line;
            Length = length;
            Column = col;
        }
    }
}
