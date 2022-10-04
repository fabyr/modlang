using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    public class MlType
    {
        public static readonly MlType NULLTYPE = new MlType(CoreLib.Constants.TYPE_NULL);
        public static readonly MlType FUNC = new MlType(CoreLib.Constants.TYPE_FUNC);
        public static readonly MlType NATIVE = new MlType(CoreLib.Constants.TYPE_NATTYPE);

        // TODO: FIX NESTED TYPE ISSUE

        public string TypeStr;
        public bool IsArray;

        //public int NativeInformationLevel = -1;
        //public bool IsNativeType = false;

        public MlType(string typestr, bool array)
        {
            TypeStr = typestr;
            IsArray = array;
            /*if (CoreLib.Constants.NativeOpTypes.InArray(typestr))
            {
                IsNativeType = true;
                NativeInformationLevel = CoreLib.Constants.NativeTypeInformationLevel[typestr];
            }*/
        }

        public MlType(string typestr) : this(typestr, false)
        { }

        public bool Is(string pure)
        {
            return !IsArray && TypeStr == pure;
        }

        public static bool operator ==(MlType a, MlType b)
        {
            return !(a is null) && a.Equals(b);
        }

        public static bool operator !=(MlType a, MlType b)
        {
            return !(a is null) && !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if(!(obj is null) && obj is MlType other)
            {
                return other.TypeStr.Equals(TypeStr) && other.IsArray == IsArray;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 37; // prime

                result *= 397; // also prime
                if (TypeStr != null)
                    result += TypeStr.GetHashCode();

                result *= 397;
                result += IsArray.GetHashCode();

                return result;
            }
        }

        public override string ToString()
        {
            return TypeStr + (IsArray ? "[]" : string.Empty);
        }

        public bool IsNull()
        {
            return !IsArray && TypeStr == CoreLib.Constants.TYPE_NULL;
        }
    }
}
