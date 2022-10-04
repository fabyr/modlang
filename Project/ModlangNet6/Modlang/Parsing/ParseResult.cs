using Modlang.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Parsing
{
    public class ParseResult : IEnumerable<Expression>
    {
        private List<Expression> _list = new List<Expression>();

        public void Add(Expression t)
        {
            _list.Add(t);
        }

        public void Remove(Expression t)
        {
            _list.Remove(t);
        }

        public void RemoveAt(int idx)
        {
            _list.RemoveAt(idx);
        }

        public IEnumerator<Expression> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public Expression this[int i]
        {
            get => _list[i];
            set => _list[i] = value;
        }

        public string BuildCodeString()
        {
            StringBuilder sb = new StringBuilder();
            //long line = 1;
            //int pads = (int)Math.Ceiling(Math.Log10(_list.Count)) + 1;
            foreach (Expression t in this)
            {
                //sb.Append(line.ToString().PadLeft(pads, '0'));
                //sb.Append("   ");
                string tcs = t.ToCodeString();
                sb.Append(tcs);
                char last = tcs[tcs.Length - 1];
                if (last != ';' && last != '}')
                    sb.Append(";");
                //line++;
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public string BuildDisplayString()
        {
            StringBuilder sb = new StringBuilder();
            //long line = 1;
            foreach (Expression t in this)
            {
                sb.AppendLine();
                sb.Append(t);
                sb.Append(" ");
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
