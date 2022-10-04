using Modlang.Abstract.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Modlang.Abstract
{
    public class AttributeFunctionExpression : FunctionExpression
    {
        public bool IsSetter { get; set; }
        public AttributeFunctionExpression() { }
        public AttributeFunctionExpression(string name, AccessModifier modifier, Expression inner, Expression returnType, bool isSetter)
            : base(name, modifier, isSetter ? new Expression[] { new DeclarationExpression(returnType, new IdentifierExpression("value")) } : new Expression[0], inner, returnType)
        {
            IsSetter = isSetter;
        }

        // TODO: ToString
    }
}
