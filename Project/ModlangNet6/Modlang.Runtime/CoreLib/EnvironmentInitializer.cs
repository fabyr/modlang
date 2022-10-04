using Modlang.Abstract;
using Modlang.CommonUtilities;
using System;

namespace Modlang.Runtime.CoreLib
{
    public abstract class EnvironmentInitializer
    {
        public static EnvironmentInitializer GetDefault(string coreFile)
        {
            return new DefaultEnvironmentInitializer(coreFile);
        }

        public enum NumOp
        {
            INVALID,
            EQ,
            NEQ,
            ADD,
            SUB,
            MUL,
            DIV,
            MOD,
            NEG,
            IDENTITY,
            NOT,
            AND,
            OR,
            XOR,
            LOG_AND,
            LOG_OR,
            LOG_NOT,
            GT,
            LT,
            GTE,
            LTE,
            RSH,
            LSH,
            INC_PRE,
            INC_POST,
            DEC_PRE,
            DEC_POST
        }

        public delegate MlObject NativeCodeFunction(Environment env, params MlObject[] args);

        public static NumOp GetNumOpFromOperatorString(string op, bool isUnary, bool isPrefix, ref string outType)
        {
            if(isUnary)
                switch (op)
                {
                    case "++":
                        return isPrefix ? NumOp.INC_PRE : NumOp.INC_POST;
                    case "--":
                        return isPrefix ? NumOp.DEC_PRE : NumOp.DEC_POST;
                    default:
                        if (!isPrefix)
                            return NumOp.INVALID;
                        break;
                }
            switch (op)
            {
                case "!":
                    return isUnary ? NumOp.LOG_NOT : NumOp.INVALID;
                case "+":
                    return isUnary ? NumOp.IDENTITY : NumOp.ADD;
                case "-":
                    return isUnary ? NumOp.NEG : NumOp.SUB;
                case "*":
                    return isUnary ? NumOp.INVALID : NumOp.MUL;
                case "/":
                    return isUnary ? NumOp.INVALID : NumOp.DIV;
                case "%":
                    return isUnary ? NumOp.INVALID : NumOp.MOD;
                case "~":
                    return isUnary ? NumOp.NOT : NumOp.INVALID;
                case "&":
                    return isUnary ? NumOp.INVALID : NumOp.AND;
                case "|":
                    return isUnary ? NumOp.INVALID : NumOp.OR;
                case "^":
                    return isUnary ? NumOp.INVALID : NumOp.XOR;
                case "&&":
                    return isUnary ? NumOp.INVALID : NumOp.LOG_AND;
                case "||":
                    return isUnary ? NumOp.INVALID : NumOp.LOG_OR;
                case "==":
                    outType = CoreLib.Constants.TYPE_BOOL;
                    return NumOp.EQ;
                case "!=":
                    outType = CoreLib.Constants.TYPE_BOOL;
                    return NumOp.NEQ;
                case ">":
                    outType = CoreLib.Constants.TYPE_BOOL;
                    return isUnary ? NumOp.INVALID : NumOp.GT;
                case "<":
                    outType = CoreLib.Constants.TYPE_BOOL;
                    return isUnary ? NumOp.INVALID : NumOp.LT;
                case ">=":
                    outType = CoreLib.Constants.TYPE_BOOL;
                    return isUnary ? NumOp.INVALID : NumOp.GTE;
                case "<=":
                    outType = CoreLib.Constants.TYPE_BOOL;
                    return isUnary ? NumOp.INVALID : NumOp.LTE;
                case ">>":
                    return isUnary ? NumOp.INVALID : NumOp.RSH;
                case "<<":
                    return isUnary ? NumOp.INVALID : NumOp.LSH;
                default:
                    return NumOp.INVALID;
            }
        }

        public static MlObject CreateNativeObject(Environment env, object v, string cname)
        {
            if (cname == CoreLib.Constants.TYPE_NULL)
                return MlObject.MlNull;
            object nat = v;// (object)ShrinkenNumericType(v, ref cname);
            MlObject obj = MlObject.FromClassNonInit(env.FindNativeClass(cname), env);
            obj.NativeValue.Value = nat;
            //if (Constants.NativeOpTypes.InArray(cname)) obj.NativeValue.InformationLevel = Constants.NativeTypeInformationLevel[cname];
            obj.Init(env);
            return obj;
        }

        public static MlObject NativeCastOperation(Environment env, MlObject obj, MlType toType, bool implicitOnly)
        {
            if (obj.UnderlyingType.IsArray || toType.IsArray)
                return null;
            if (implicitOnly
                && (!ObjectOperations.Eq(obj.NativeValue.Value, 0) && Constants.NativeTypeInformationLevel[obj.UnderlyingType.TypeStr] > Constants.NativeTypeInformationLevel[toType.TypeStr]))
                return null;
            string typeStr = toType.ToString();
            object a = obj.NativeValue.Value;
            object res = CastNativeConvert(a, typeStr);
            if (res == null)
                return null;
            return CreateNativeObject(env, res, typeStr);
        }

        public static MlObject DefaultValueForNativeType(Environment env, string natType)
        {
            object v = Constants.NativeOpTypeDefaults[Array.IndexOf(Constants.NativeOpTypes, natType)];
            return CreateNativeObject(env, v, natType);
        }

        public static MlObject NativeOperatorOperation(Environment env, string op, bool isUnary, bool isPrefix, params MlObject[] obj)
        {
            MlObject a = obj[0];
            string t = a.WorkingType.TypeStr;
            NumOp no = GetNumOpFromOperatorString(op, isUnary, isPrefix, ref t);

            // Null Comparison
            if((no == NumOp.EQ || no == NumOp.NEQ)
                && (obj[0].UnderlyingType.IsNull() || obj[1].UnderlyingType.IsNull()))
            {
                return CreateNativeObject(env, obj[0].UnderlyingType.IsNull()
                    && obj[1].UnderlyingType.IsNull(), CoreLib.Constants.TYPE_BOOL);
            }

            if (no == NumOp.INVALID)
                return null;
            object result = isUnary ? UnOperatorOperation(no, a) : BinOperatorOperation(no, a, obj[1]);
            string type = !isUnary && t == a.WorkingType.TypeStr ? GreaterType(a.WorkingType.TypeStr, obj[1].WorkingType.TypeStr) : t;
            object nat = CastNative(result, type);
            return CreateNativeObject(env, nat, type);
        }

        public static void ObjectifyMlObject(MlObject o, Environment env)
        {
            void F(FunctionExpression fe)
            {
                if (o.BaseObject != null)
                {
                    o.BaseObject.Functions.RemoveAll(_ => _.Name == fe.Name); // For now we can just replace all of them, because we assume the base object is always of type "object"
                    o.BaseObject.Functions.Add(fe);
                }

                o.Functions.Add(fe);
            }

            void O(OperatorFunctionExpression fe)
            {
                if (o.BaseObject != null)
                {
                    o.BaseObject.OperatorsList.RemoveAll(_ => _.Name.Operator == fe.Name.Operator); // For now we can just replace all of them, because we assume the base object is always of type "object"
                    o.BaseObject.OperatorsList.Add(fe);
                }

                o.OperatorsList.Add(fe);
            }

            NativeCodeFunction opEQ = (e, obj) =>
            {
                MlObject other = obj[0];
                bool val = false;
                if (other is MlTypeObject mlto)
                {
                    val = o.Equals(mlto);
                }
                return EnvironmentInitializer.CreateNativeObject(e, val, CoreLib.Constants.TYPE_BOOL);
            };

            F(new NativeFunction(CoreLib.Constants.OBJ_FUNC_TOSTRING, 0, (e, obj) =>
            {
                return EnvironmentInitializer.CreateNativeObject(e, o.ToString(e), CoreLib.Constants.TYPE_STRING);
            }));

            F(new NativeFunction(CoreLib.Constants.OBJ_FUNC_GETHASHCODE, 0, (e, obj) =>
            {
                return EnvironmentInitializer.CreateNativeObject(e, (long)o.GetHashCode(), CoreLib.Constants.TYPE_LONG);
            }));

            F(new NativeFunction(CoreLib.Constants.OBJ_FUNC_EQUALS, 0, opEQ));

            F(new NativeFunction(CoreLib.Constants.OBJ_FUNC_GETTYPE, 0, (e, obj) =>
            {
                return EnvironmentInitializer.CreateNativeObject(e, o.UnderlyingType.ToString(), CoreLib.Constants.TYPE_STRING);
            }));

            O(new NativeOperatorFunction("==", OperatorFunctionExpression.OperatorModifiers.NORMAL, 1, opEQ));
            O(new NativeOperatorFunction("!=", OperatorFunctionExpression.OperatorModifiers.NORMAL, 1, (e, obj) =>
            {
                MlObject other = obj[0];
                bool val = false;
                if (other is MlTypeObject mlto)
                {
                    val = !o.Equals(mlto);
                }
                return EnvironmentInitializer.CreateNativeObject(e, val, CoreLib.Constants.TYPE_BOOL);
            }));
        }

        public abstract void Init(Environment env);

        protected static readonly string[] NumericTypesScale = new string[]
        {
            Constants.TYPE_BYTE,
            Constants.TYPE_SBYTE,
            Constants.TYPE_SHORT,
            Constants.TYPE_USHORT,
            Constants.TYPE_INT,
            Constants.TYPE_UINT,
            Constants.TYPE_LONG,
            Constants.TYPE_ULONG,
            Constants.TYPE_FLOAT,
            Constants.TYPE_DOUBLE
        };

        protected static string GreaterType(string a, string b)
        {
            int idx1 = Array.IndexOf(NumericTypesScale, a);
            int idx2 = Array.IndexOf(NumericTypesScale, b);
            if (idx1 == -1 || idx2 == -1)
                return a; // TODO: Proper implementation (this is a quickfix)
            return NumericTypesScale[Math.Max(idx1, idx2)];
        }

        protected static object CastNativeConvert(object nat, string t)
        {
            unchecked
            {
                switch (t)
                {
                    case Constants.TYPE_OBJECT:
                        return nat;
                    case Constants.TYPE_BOOL:
                        return Convert.ToBoolean(nat);
                    case Constants.TYPE_BYTE:
                        return Convert.ToByte(nat);
                    case Constants.TYPE_SBYTE:
                        return Convert.ToSByte(nat);
                    case Constants.TYPE_CHAR:
                        return Convert.ToChar(nat);
                    case Constants.TYPE_DOUBLE:
                        return Convert.ToDouble(nat);
                    case Constants.TYPE_FLOAT:
                        return Convert.ToSingle(nat);
                    case Constants.TYPE_SHORT:
                        return Convert.ToInt16(nat);
                    case Constants.TYPE_USHORT:
                        return Convert.ToUInt16(nat);
                    case Constants.TYPE_INT:
                        return Convert.ToInt32(nat);
                    case Constants.TYPE_UINT:
                        return Convert.ToUInt32(nat);
                    case Constants.TYPE_LONG:
                        return Convert.ToInt64(nat);
                    case Constants.TYPE_ULONG:
                        return Convert.ToUInt64(nat);
                    default:
                        return null;
                }
            }
        }

        protected static object CastNative(object nat, string t)
        {
            return CastNativeConvert(nat, t);
        }

        protected static object UnOperatorOperation(NumOp op, MlObject obj)
        {
            object o = CastNative(obj.NativeValue.Value, obj.WorkingType.TypeStr);
            object result;
            switch (op)
            {
                case NumOp.NEG:
                    result = ObjectOperations.Neg(o);
                    break;
                case NumOp.IDENTITY:
                    result = o;
                    break;
                case NumOp.NOT:
                    result = ObjectOperations.BitNot(o);
                    break;
                case NumOp.LOG_NOT:
                    result = !(bool)o;
                    break;
                case NumOp.INC_PRE:
                    result = ObjectOperations.Add(o, 1);
                    obj.NativeValue.Value = result;
                    break;
                case NumOp.INC_POST:
                    result = o;
                    obj.NativeValue.Value = ObjectOperations.Add(o, 1);
                    break;
                case NumOp.DEC_PRE:
                    result = ObjectOperations.Sub(o, 1);
                    obj.NativeValue.Value = result;
                    break;
                case NumOp.DEC_POST:
                    result = o;
                    obj.NativeValue.Value = ObjectOperations.Sub(o, 1);
                    break;
                default:
                    return null;
            }
            return result;
        }

        protected static object BinOperatorOperation(NumOp op, MlObject objA, MlObject objB)
        {
            object a = CastNative(objA.NativeValue.Value, objA.WorkingType.TypeStr), b = CastNative(objB.NativeValue.Value, objB.WorkingType.TypeStr);
            object result;
            switch (op)
            {
                case NumOp.EQ:
                    result = ObjectOperations.Eq(a, b);
                    break;
                case NumOp.NEQ:
                    result = !ObjectOperations.Eq(a, b);
                    break;
                case NumOp.ADD:
                    result = ObjectOperations.Add(a, b);
                    break;
                case NumOp.SUB:
                    result = ObjectOperations.Sub(a, b);
                    break;
                case NumOp.MUL:
                    result = ObjectOperations.Mul(a, b);
                    break;
                case NumOp.DIV:
                    result = ObjectOperations.Div(a, b);
                    break;
                case NumOp.MOD:
                    result = ObjectOperations.Mod(a, b);
                    break;
                case NumOp.AND:
                    result = ObjectOperations.BitAnd(a, b);
                    break;
                case NumOp.OR:
                    result = ObjectOperations.BitOr(a, b);
                    break;
                case NumOp.XOR:
                    result = ObjectOperations.BitXor(a, b);
                    break;
                case NumOp.LOG_AND:
                    result = (bool)a && (bool)b;
                    break;
                case NumOp.LOG_OR:
                    result = (bool)a || (bool)b;
                    break;
                case NumOp.GT:
                    result = ObjectOperations.Gt(a, b);
                    break;
                case NumOp.LT:
                    result = ObjectOperations.Lt(a, b);
                    break;
                case NumOp.GTE:
                    result = ObjectOperations.Gte(a, b);
                    break;
                case NumOp.LTE:
                    result = ObjectOperations.Lte(a, b);
                    break;
                case NumOp.RSH:
                    result = ObjectOperations.Rsh(a, Convert.ToInt32(b));
                    break;
                case NumOp.LSH:
                    result = ObjectOperations.Lsh(a, Convert.ToInt32(b));
                    break;
                default:
                    return null;
            }
            return result;
        }
    }
}
