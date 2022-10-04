using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Abstract.Types
{
    public enum AccessModifier : byte
    {
        PRIVATE = 0,
        PROTECTED = 1,
        PUBLIC = 2,
        DEFAULT = 3// = PRIVATE
    }

    public static class AccessModifierExtensions
    {
        public static string GetString(this AccessModifier mod)
        {
            switch (mod)
            {
                case AccessModifier.PRIVATE:
                    return "private";
                case AccessModifier.PROTECTED:
                    return "protected";
                case AccessModifier.PUBLIC:
                    return "public";
                case AccessModifier.DEFAULT:
                default:
                    return string.Empty;
            }
        }
    }
}
