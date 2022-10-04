using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Exceptions
{
    public static class Util
    {
        public static string GetSiteFromStream(this FromFileObject ffo, StreamReader sr, int tabSizeInSpaces = 4)
        {
            if (ffo == null)
                return string.Empty;
            if (!sr.BaseStream.CanSeek)
                throw new ArgumentException("Base stream must be seekable");

            const int prefSz = 50, postSz = 50;

            StringBuilder sb = new StringBuilder();
            int pref = (int)Math.Min(prefSz, ffo.Position), post = postSz;
            long posbuf = sr.BaseStream.Position;
            sr.BaseStream.Position = 0;
            sr.DiscardBufferedData();

            long charpos = 0;
            int actualPref = 0, actualPost = 0;
            int r;
            while((r = sr.Read()) != -1 && charpos < ffo.EndPos + post)
            {
                charpos++;
                char ch = (char)r;
                if (charpos >= ffo.Position - pref && charpos < ffo.Position)
                {
                    if(ch == '\r' || ch == '\n')
                    {
                        sb.Clear();
                        actualPref = 0;
                        continue;
                    }
                    if (sb.Length == 0 && char.IsWhiteSpace(ch)) // skip leading whitespace
                        continue;
                    actualPref += ch == '\t' ? tabSizeInSpaces : 1;
                    sb.Append((char)r);
                } else if(charpos >= ffo.Position && charpos <= ffo.EndPos)
                {
                    sb.Append((char)r);
                }
                else if (charpos >= ffo.EndPos)
                {
                    if (ch == '\r' || ch == '\n')
                    {
                        break;
                    }
                    actualPost += ch == '\t' ? tabSizeInSpaces : 1;
                    sb.Append((char)r);
                }
            }
            sb.AppendLine();
            sb.Append("".PadLeft(actualPref, '-'));
            sb.Append("^");
            sb.Append("".PadLeft(actualPost + (int)(ffo.Length - 1), ' '));
            sb.Replace("\t", "".PadLeft(tabSizeInSpaces, ' '));
            sr.BaseStream.Position = posbuf;

            return sb.ToString();
        }
    }
}
