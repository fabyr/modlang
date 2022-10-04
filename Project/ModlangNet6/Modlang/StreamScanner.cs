using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang
{
    public class StreamScanner
    {
        public int BufferSize { get; }

        private StreamReader _sr;

        public bool EndOfStream { get => _sr.EndOfStream; }

        private char?[] _buffer;
        private int _bufIdx = 0;

        public StreamScanner(StreamReader sr, int bufferSize = 10)
        {
            _sr = sr;
            BufferSize = bufferSize;

            _buffer = new char?[BufferSize];
        }

        public char NextChar()
        {
            int ch = _sr.Read();
            if (ch == -1)
                return '\0';

            char c = (char)ch;

            _buffer[_bufIdx++] = c;

            if(_bufIdx >= BufferSize)
            {
                Array.Copy(_buffer, 1, _buffer, 0, BufferSize - 1); // ShiftLeft by 1
                _buffer[BufferSize - 1] = null;
                _bufIdx = BufferSize - 1;
            }

            //Console.WriteLine($"[{string.Join(" ", _buffer.Select(_ => _ == null ? "-" : Util.TokenDisplayEscapeContent(_.ToString())))}]");

            return c;
        }

        public bool MatchWindow(string str, int offset)
        {
            if (offset > 0)
                throw new ArgumentOutOfRangeException("offset", "This value must be negative or zero.");
            int startIdx = _bufIdx + offset - 1;
            if (startIdx < 0)
                return false;
            for (int i = 0; i < str.Length; i++)
            {
                if(!_buffer[i + startIdx].HasValue || _buffer[i + startIdx].Value != str[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
