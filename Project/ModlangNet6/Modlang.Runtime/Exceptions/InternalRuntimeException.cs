using Modlang.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime.Exceptions
{
    public class InternalRuntimeException : ModlangException
    {
        public InternalRuntimeException(string msg) : base(null, msg)
        { }
    }
}
