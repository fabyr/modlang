using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime.CoreLib
{
    public static class Constants
    {
        public const string SPECIAL_OP_LOG_AND = "&&";
        public const string SPECIAL_OP_LOG_OR = "||";

        public const string CLASS_PROPERTY_NATIVE_VALUE = "_native_value_";
        public const string CLASS_PROPERTY_TYPE = "_type_";

        public const string ARRAY_PROPERTY_LENGTH = "length";
        public const string FUNC_N_PRFIX = "__fnative";

        public const string TYPE_STRING = "string";
        public const string TYPE_OBJECT = "object";

        public const string TYPE_BOOL = "bool";
        public const string TYPE_BYTE = "byte";
        public const string TYPE_SBYTE = "sbyte";
        public const string TYPE_CHAR = "char";
        //public const string TYPE_DECIMAL = "decimal";
        public const string TYPE_DOUBLE = "double";
        public const string TYPE_FLOAT = "float";

        public const string TYPE_SHORT = "short";
        public const string TYPE_USHORT = "ushort";
        public const string TYPE_INT = "int";
        public const string TYPE_UINT = "uint";
        public const string TYPE_LONG = "long";
        public const string TYPE_ULONG = "ulong";

        public const string TYPE_NULL = "__null_t";
        public const string TYPE_FUNC = "__func_t";
        public const string TYPE_NATTYPE = "__native_t";
        public const string TYPE_TYPE = "__type_t";

        public static readonly object[] NativeOpTypeDefaults = new object[]
        {
            null,
            false,
            (byte)0,
            (sbyte)0,
            (char)0,
            0.0d,
            0.0f,
            (short)0,
            (ushort)0,
            0,
            0U,
            0L,
            0UL
        };

        public static readonly string[] NativeOpTypes = new string[]
        {
            TYPE_OBJECT,
            TYPE_BOOL ,
            TYPE_BYTE ,
            TYPE_SBYTE ,
            TYPE_CHAR ,
            TYPE_DOUBLE,
            TYPE_FLOAT ,

            TYPE_SHORT ,
            TYPE_USHORT,
            TYPE_INT ,
            TYPE_UINT ,
            TYPE_LONG ,
            TYPE_ULONG ,

            /*TYPE_NULL ,
            TYPE_FUNC ,
            TYPE_NATTYPE,
            TYPE_TYPE*/
        };

        public static readonly Dictionary<string, int> NativeTypeInformationLevel = new Dictionary<string, int>()
        {
            { TYPE_OBJECT, -1 },
            { TYPE_BOOL, 0 },
            { TYPE_SBYTE, 1 },
            { TYPE_BYTE, 2 },

            { TYPE_SHORT, 3 },
            { TYPE_USHORT, 4 },
            { TYPE_INT, 5 },
            { TYPE_CHAR, 5 },
            { TYPE_UINT, 6 },
            { TYPE_LONG, 7 },
            { TYPE_ULONG, 8 },
            { TYPE_FLOAT, 9 },
            { TYPE_DOUBLE, 10 }
        };

        /*public static readonly Dictionary<string, Tuple<int, int>> NativeNumericLimits = new Dictionary<string, Tuple<int, int>>()
        {
            { TYPE_BOOL, Tuple.Create(0, 1) },
            { TYPE_SBYTE, Tuple.Create(-128, 127) },
            { TYPE_BYTE, Tuple.Create(0, 255) },

            { TYPE_SHORT, Tuple.Create(-32768, 32767) },
            { TYPE_USHORT, Tuple.Create(0, 65535) },
            { TYPE_INT, Tuple.Create(-2147483648, 2147483647) },
            { TYPE_CHAR, Tuple.Create(-2147483648, 2147483647) },
            { TYPE_UINT, Tuple.Create(0, 4294967295) },
            { TYPE_LONG, 7 },
            { TYPE_ULONG, 8 },
            { TYPE_FLOAT, 9 },
            { TYPE_DOUBLE, 10 }
        };*/

        public static readonly string[] NativeTypes = new string[]
        {    
            TYPE_OBJECT,
            TYPE_STRING,
            TYPE_BOOL ,
            TYPE_BYTE ,
            TYPE_SBYTE ,
            TYPE_CHAR ,
            TYPE_DOUBLE,
            TYPE_FLOAT ,

            TYPE_SHORT ,
            TYPE_USHORT,
            TYPE_INT ,
            TYPE_UINT ,
            TYPE_LONG ,
            TYPE_ULONG ,

            TYPE_NULL ,
            TYPE_FUNC ,
            TYPE_NATTYPE,
            TYPE_TYPE
       };

        public static readonly string[] SpecialNativeTypes = new string[]
        {
            TYPE_FUNC,
            TYPE_NATTYPE,
            TYPE_TYPE
        };

        public const string OBJ_FUNC_EQUALS = "equals";
        public const string OBJ_FUNC_GETHASHCODE = "hashcode";
        public const string OBJ_FUNC_GETTYPE = "gettype";
        public const string OBJ_FUNC_TOSTRING = "tostring";

        public const string FUNC_OTHER_CTOR = "_ctor_";

        public static MlType GetNonArrayType(this string type)
        {
            return new MlType(type);
        }
    }
}
