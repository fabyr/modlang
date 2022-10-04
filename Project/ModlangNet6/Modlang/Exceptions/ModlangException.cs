using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Exceptions
{
    public class ModlangException : Exception
    {
        public FromFileObject Origin { get; }

        public ModlangException(string message) : base(message) { }
        public ModlangException(FromFileObject origin, string message) : this(message) { Origin = origin; }
    }
}
