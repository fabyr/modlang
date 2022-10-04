using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    public class MlCastedObject : MlObject
    {
        public MlObject Subject;

        public MlCastedObject(MlObject of, MlType to, Environment env)
        {
            Subject = of;

            this.BaseObject = of.BaseObject;
            this.TypeObject = of.TypeObject;
            this.Attributes = of.Attributes;
            this.Functions = of.Functions;
            this._env = of._env;
            this.IsInitialized = of.IsInitialized;
            this.IsPrimitive = of.IsPrimitive;
            this.NativeValue = of.NativeValue.Clone(env) as MlObjectNative;
            //this.Operators = of.Operators;
            this.UnderlyingType = of.UnderlyingType;
            this.WorkingType = to;
            this.OperatorsList = of.OperatorsList;
        }
    }
}
