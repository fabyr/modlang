using Modlang.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime.Exceptions
{
    public class RuntimeException : ModlangException
    {
        //public FromFileObject Origin { get; set; }
        public RuntimeException(FromFileObject origin, string msg) : base(origin, msg)
        {
            //Origin = origin;
        }
    }
}
