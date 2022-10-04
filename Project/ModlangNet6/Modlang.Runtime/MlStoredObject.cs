using Modlang.Runtime.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    public class MlStoredObject
    {
        public List<MlStoredObject> Attached = new List<MlStoredObject>();

        public MlObject Value;
        public MlObjectAccess Access;
        public MlType Type;
        public bool IsStatic;
        public bool IsConst;

        public virtual void ChangeValue(MlObject newValue, Environment env, bool allowConstAssignment = false)
        {
            if (Value is MlTypeObject)
                throw new InternalRuntimeException("Tried to change a type.");
            if (IsConst && !allowConstAssignment)
                throw new InternalRuntimeException("Tried to change constant variable.");
            if (!Util.TypeCheck(newValue, Type))
                    newValue = Execution.CastObject(newValue, Type, env, true);
                
            //return false;
            Value = newValue.IsPrimitive ? newValue.CastClone(env) : newValue;
            Attached.ForEach(_ => _.ChangeValue(newValue, env));
        }

        public virtual MlStoredObject Clone(Environment env)
        {
            MlStoredObject newObj = new MlStoredObject
            {
                Value = Value/*.Clone(env)*/, // The value doesn't need to be cloned because whenever a value is set, it is replaced completely
                Access = Access,
                Type = Type,
                IsConst = IsConst,
                IsStatic = IsStatic
            };
            return newObj;
        }
    }
}
