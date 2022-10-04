using Modlang.Abstract;
using Modlang.Runtime.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    public class MlObjectCallable : MlObject
    {
        public FunctionBaseExpression Func;
        public MlObject From;

        public MlObjectCallable(Environment env)
        {
            _env = env;
            UnderlyingType = MlType.FUNC;
            WorkingType = UnderlyingType;

            EnvironmentInitializer.ObjectifyMlObject(this, env);
        }

        /*public override Dictionary<string, MlStoredObject> Attributes { get => null; set { } }
        public override List<FunctionExpression> Functions { get => null; set { } }
        public override MlObjectNative NativeValue { get => null; set { } }
        public override List<OperatorFunctionExpression> Operators { get => null; set { } }
        public override MlType UnderlyingType { get => MlType.FUNC; set { } }
        public override MlType WorkingType { get => UnderlyingType; set { } }*/
    }
}
