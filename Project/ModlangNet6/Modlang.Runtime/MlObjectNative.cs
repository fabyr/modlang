using Modlang.Abstract;
using Modlang.Runtime.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    public class MlObjectNative : MlStoredObject
    {
        public class MlObjectNativeContainer : MlObject
        {
            public new object NativeValue;

            [Obsolete]
            public override MlObject Clone(Environment env)
            {
                MlObjectNativeContainer newObj = new MlObjectNativeContainer
                {
                    NativeValue = NativeValue
                };
                return newObj;
            }
        }

        private object _value = null;
        public new object Value { get => _value; set { _value = value; IsSet = true; } }
        public bool IsSet { get; private set; } = false;

        public MlObjectNative()
        {
            base.Access = MlObjectAccess.CLASS_CONTEXT_PRIVATE;
        }

        public void ChangeValue(object newValue)
        {
            Value = newValue;
        }

        public override void ChangeValue(MlObject newValue, Environment env, bool allowConstAssignment = false)
        {
            if (newValue.NativeValue == null)
                throw new InternalRuntimeException("The object which a native value should be set to has no native value");
            if (newValue is MlObjectNativeContainer mnat)
            {
                Value = mnat.NativeValue;
            } else

            Value = newValue.NativeValue.Value;
        }

        public override MlStoredObject Clone(Environment env)
        {
            MlObjectNative newObj = new MlObjectNative
            {
                _value = _value,
                IsSet = IsSet
            };
            return newObj;
        }
    }
}
