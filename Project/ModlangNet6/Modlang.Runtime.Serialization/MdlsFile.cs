using Modlang.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime.Serialization
{
    public static class MdlsFile
    {
        public static void Write(Stream s, Expression[] expressions)
        {
            s.WriteInt32(expressions.Length);
            for (int i = 0; i < expressions.Length; i++)
                expressions[i].Perform(s);
        }

        public static Expression[] Read(Stream s)
        {
            int expc = s.ReadInt32();
            Expression[] res = new Expression[expc];
            for (int i = 0; i < expc; i++)
                res[i] = ExpressionSerializer.Build(s);
            return res;
        }

        public static void Write(string filename, Expression[] expressions)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                Write(fs, expressions);
        }

        public static Expression[] Read(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                return Read(fs);
        }
    }
}
