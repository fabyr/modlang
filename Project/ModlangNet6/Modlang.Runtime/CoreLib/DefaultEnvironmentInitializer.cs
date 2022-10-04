using Modlang.Abstract;
using Modlang.Lexing;
using Modlang.Parsing;
using Modlang.Runtime.Exceptions;
using System;
using System.Linq;

namespace Modlang.Runtime.CoreLib
{
    public class DefaultEnvironmentInitializer : EnvironmentInitializer
    {
        public const string FUNC_N_EQUALITY = "__fnative_equality__";
        public const string FUNC_N_HASHCODE = "__fnative_hashcode__";
        public const string FUNC_N_TOSTRING = "__fnative_tostring__";

        public const string FUNC_N_NATCAST = "__fnative_natcast__";
        public const string FUNC_N_STRCONCAT = "__fnative_strconcat__";
        public const string FUNC_N_STRLEN = "__fnative_strlen__";
        public const string FUNC_N_STRGCHARAT = "__fnative_strgcharat__";
        public const string FUNC_N_STRSCHARAT = "__fnative_strscharat__";
        public const string FUNC_N_SYSTIME = "__fnative_systime__";

        public const string FUNC_N_PRINT = "__fnative_print__";
        public const string FUNC_N_READKEY = "__fnative_readkey__";
        public const string FUNC_N_CONSOLESCOLOR = "__fnative_consolescolor__";
        public const string FUNC_N_CONSOLEGCOLOR = "__fnative_consolegcolor__";

        public const string FUNC_N_EXIT = "__fnative_exit__";

        private string _coreFile;

        public DefaultEnvironmentInitializer(string coreFile)
        {
            _coreFile = coreFile;
        }

        public override void Init(Environment env)
        {
            {
                Lexer lx = new Lexer(_coreFile);
                LexerResult lr = lx.Lex();
                Parser par = new Parser(lr);
                ParseResult pr = par.Parse();
                //Console.WriteLine(pr.BuildDisplayString());
                Type[] validTypes = new Type[]
                {
                    typeof(ClassExpression),
                    typeof(FunctionExpression)
                };
                if (pr.Any(_ => !validTypes.Contains(_.GetType())))
                    throw new RuntimeException(null, "CoreFile is malformed");
                env.Execute(pr.ToArray());
            }

            MlObject RET(object v, string cname)
            {
                return CreateNativeObject(env, v, cname);
            }

            void F(FunctionExpression fexp) => env.NativeFunctions.Add(fexp.Name, new MlStoredObject() { Value = new MlObjectCallable(env) { Func = fexp }, Access = MlObjectAccess.GLOBAL, Type = Constants.TYPE_FUNC.GetNonArrayType() });

            object NatVal(MlObject obj)
            {
                if (obj is MlObjectNative.MlObjectNativeContainer cont)
                    return cont.NativeValue;
                return obj.NativeValue.Value;
            }

            F(new NativeFunction(FUNC_N_PRINT, 1, (e, obj) =>
            {
                string value = NatVal(obj[0]).ToString();
                Console.Write(value);
                return null;
            }));

            F(new NativeFunction(FUNC_N_READKEY, 1, (e, obj) =>
            {
                char v = Console.ReadKey(true).KeyChar;
                return RET(v, Constants.TYPE_CHAR);
            }));

            F(new NativeFunction(FUNC_N_CONSOLESCOLOR, 1, (e, obj) =>
            {
                int a = Convert.ToInt32(NatVal(obj[0]));
                int b = Convert.ToInt32(NatVal(obj[1]));
                if (a < 0 || b < 0 || a > 15 || b > 15)
                    throw new InternalRuntimeException("Invalid parameters");
                Console.BackgroundColor = (ConsoleColor)a;
                Console.ForegroundColor = (ConsoleColor)b;
                return null;
            }));

            F(new NativeFunction(FUNC_N_CONSOLEGCOLOR, 1, (e, obj) =>
            {
                int a = (int)Console.BackgroundColor;
                int b = (int)Console.ForegroundColor;
                byte v = (byte)((a << 4) | b);
                return RET(v, Constants.TYPE_BYTE);
            }));

            F(new NativeFunction(FUNC_N_EQUALITY, 1, (e, obj) =>
            {

                object a = NatVal(obj[0]);
                object b = NatVal(obj[1]);
                bool val;
                if (obj[0].UnderlyingType.IsNull() || obj[1].UnderlyingType.IsNull())
                    val = obj[0].UnderlyingType.IsNull() && obj[1].UnderlyingType.IsNull();
                else
                    val = a.Equals(b);
                return RET(val, Constants.TYPE_BOOL);
            }));

            F(new NativeFunction(FUNC_N_HASHCODE, 1, (e, obj) =>
            {
                object a = NatVal(obj[0]);
                return RET((long)a.GetHashCode(), Constants.TYPE_LONG);
            }));

            F(new NativeFunction(FUNC_N_TOSTRING, 1, (e, obj) =>
            {
                if (obj[0] is MlArray ar)
                    return RET(ar.EnvToString(e), Constants.TYPE_STRING);
                object a = NatVal(obj[0]);
                return RET(a == null ? obj[0].UnderlyingType.ToString() : a.ToString(), Constants.TYPE_STRING);
            }));

            F(new NativeFunction(FUNC_N_STRCONCAT, 1, (e, obj) =>
            {
                string a = NatVal(obj[0]) as string;
                string b = NatVal(obj[1]) as string;
                return RET(string.Concat(a, b), Constants.TYPE_STRING);
            }));

            F(new NativeFunction(FUNC_N_NATCAST, 1, (e, obj) =>
            {
                // TODO: Convert the type properly (so that array types also work)
                // I mean arrays cannot even be cast natively so it would just throw an error either way
                // but CLEANED CODE IS PRECIOUS
                // Currently arrays are cast somewhere else
                return NativeCastOperation(env, obj[0], (NatVal(obj[1]) as string).GetNonArrayType(), false);
            }));

            F(new NativeFunction(FUNC_N_STRLEN, 1, (e, obj) =>
            {
                string a = NatVal(obj[0]) as string;
                return RET(a.Length, Constants.TYPE_INT);
            }));

            F(new NativeFunction(FUNC_N_STRGCHARAT, 1, (e, obj) =>
            {
                string a = NatVal(obj[0]) as string;
                int b = Convert.ToInt32(NatVal(obj[1]));
                return RET(a[b], Constants.TYPE_CHAR);
            }));

            F(new NativeFunction(FUNC_N_STRSCHARAT, 1, (e, obj) =>
            {
                MlObject subj = obj[0];
                int idx = Convert.ToInt32(NatVal(obj[1]));
                char val = (char)NatVal(obj[2]);
                int b = Convert.ToInt32(NatVal(obj[1]));
                char[] ch = ((string)NatVal(subj)).ToCharArray();
                ch[idx] = val;
                return RET(new string(ch), Constants.TYPE_STRING);
            }));

            F(new NativeFunction(FUNC_N_SYSTIME, 1, (e, obj) =>
            {
                return RET(System.Environment.TickCount64, Constants.TYPE_LONG);
            }));

            F(new NativeFunction(FUNC_N_EXIT, 1, (e, obj) =>
            {
                int a = (int)NatVal(obj[0]);
                System.Environment.Exit(a);
                return null;
            }));
        }
    }
}
