using Modlang.Abstract;
using Modlang.Runtime.CoreLib;
using Modlang.Runtime.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    public class MlArray : MlObject
    {
        public MlStoredObject[] Collection;

        public MlType ItemType;
        public MlType UnderlyingItemType;
        //public new MlType UnderlyingType { get => base.UnderlyingType; set { if (!value.IsArray) throw new InternalRuntimeException("Non-Array Type cannot be used for arrays"); ItemType = new MlType(value.TypeStr, false); base.UnderlyingType = new MlType(ItemType.TypeStr, true); } }
        //public override MlType WorkingType;

        private MlArray(Environment env)
        {
            InheritFrom(this, env.FindNativeClass(CoreLib.Constants.TYPE_OBJECT), env);
            EnvironmentInitializer.ObjectifyMlObject(this, env);
            IsPrimitive = false;
        }

        public MlArray(MlType itemType, long length, Environment env) : this(env)
        {
            MlObject len = MlObject.FromClass(env.FindNativeClass(CoreLib.Constants.TYPE_LONG), env);
            len.NativeValue.Value = length;
            Attributes.Add(CoreLib.Constants.ARRAY_PROPERTY_LENGTH, new MlStoredObject() { Value = len, Access = MlObjectAccess.CLASS_CONTEXT_PUBLIC, IsConst = true, Type = new MlType(CoreLib.Constants.TYPE_LONG) });
            Collection = new MlStoredObject[length];
            for (long i = 0; i < length; i++)
                if(env.ExecutionOptions.InitializeNativeTypeArraysWithDefault && !itemType.IsArray && CoreLib.Constants.NativeOpTypes.Contains(itemType.TypeStr))
                    Collection[i] = new MlStoredObject() { Value = EnvironmentInitializer.DefaultValueForNativeType(env, itemType.TypeStr), Access = MlObjectAccess.CLASS_CONTEXT_PUBLIC, Type = itemType };
                else
                    Collection[i] = new MlStoredObject() { Value = env.ExecutionOptions.InitializeArraysWithMlNull ? MlNull : null, Access = MlObjectAccess.CLASS_CONTEXT_PUBLIC, Type = itemType };
            ItemType = itemType;
            UnderlyingItemType = itemType;
            UnderlyingType = new MlType(itemType.TypeStr, true);
            WorkingType = UnderlyingType;

        }

        [Obsolete]
        public override MlObject Clone(Environment env)
        {
            MlArray obj = new MlArray(env)
            {
                ItemType = ItemType,
                Collection = Collection,
                UnderlyingType = UnderlyingType,
                WorkingType = WorkingType
            };
            return obj;
        }

        public override string ToString(Environment env)
        {
            return EnvToString(env);
        }

        public string EnvToString(Environment env)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ItemType.ToString());
            sb.Append("(");
            sb.Append(UnderlyingItemType.ToString());
            sb.Append(")");
            sb.Append("[");
            sb.Append(Collection.Length);
            sb.Append("] { ");
            bool first = true;
            foreach (MlStoredObject mo in Collection)
            {
                string s = "<NULL>";
                if(mo.Value != null && !mo.Value.UnderlyingType.IsNull())
                {
                    env.EnterClassContext(mo.Value);
                    MlObject str = new BraceExpression(new IdentifierExpression(CoreLib.Constants.OBJ_FUNC_TOSTRING), new Expression[0], Lexing.BraceType.ROUND).Execute(env);
                    env.ExitClassContext();
                    if (!str.WorkingType.Is(CoreLib.Constants.TYPE_STRING))
                        throw new InternalRuntimeException("Something went terribly wrong");
                    s = str.NativeValue.Value as string;
                }

                if (first)
                    first = false;
                else
                    sb.Append(", ");
                sb.Append(s);
                
            }
            sb.Append(" }");
            return sb.ToString();
        }
    }
}
