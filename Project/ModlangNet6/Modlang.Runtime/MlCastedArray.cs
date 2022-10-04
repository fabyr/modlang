using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    public class MlCastedArray : MlArray
    {
        public MlArray Subject;

        public MlCastedArray(MlArray of, MlType to, Environment env) : base(new MlType(to.TypeStr), of.Collection.LongLength, env)
        {
            Subject = of;
            this.ItemType = UnderlyingItemType;
            this.TypeObject = of.TypeObject;
            this.UnderlyingItemType = of.ItemType;
            //this.BaseObject = of.BaseObject;
            this.TypeObject = of.TypeObject;
            //this.Attributes = of.Attributes;
            //this.Functions = of.Functions;
            this._env = of._env;
            //this.IsInitialized = of.IsInitialized;
            //this.IsPrimitive = of.IsPrimitive;
            //this.NativeValue = of.NativeValue.Clone(env) as MlObjectNative;
            //this.Operators = of.Operators;
            //this.UnderlyingType = of.UnderlyingType;
            this.WorkingType = to;
            //this.OperatorsList = of.OperatorsList;

            
            //this.ItemType = nonArrayType;
            //this.Collection = new MlStoredObject[of.Collection.LongLength];
            for (long i = 0; i < of.Collection.LongLength; i++)
            {
                if (this.Collection[i].Value.UnderlyingType.IsNull())
                    this.Collection[i].Value.UnderlyingType = of.Collection[i].Value.UnderlyingType;
                this.Collection[i].ChangeValue(of.Collection[i].Value, env);
                this.Collection[i].Attached.Add(of.Collection[i]);
            }
        }
    }
}
