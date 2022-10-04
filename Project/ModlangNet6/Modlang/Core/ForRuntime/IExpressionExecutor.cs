using Modlang.Abstract;
using Modlang.Abstract.ControlStructures;
using Modlang.Abstract.Imperative;
using Modlang.Abstract.Literals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Core.ForRuntime
{
    public interface IExpressionExecutor
    {
        object Execute(UnaryOperatorExpression exp, object b);
        object Execute(OperatorFunctionExpression exp, object b);
        object Execute(NOPExpression exp, object b);
        object Execute(IdentifierExpression exp, object b);
        object Execute(FunctionExpression exp, object b);
        object Execute(FunctionBaseExpression exp, object b);
        object Execute(DeclarationExpression exp, object b);
        object Execute(ConstructorExpression exp, object b);
        object Execute(CodeBlock exp, object b);
        object Execute(ClassExpression exp, object b);
        object Execute(CastExpression exp, object b);
        object Execute(BraceExpression exp, object b);
        object Execute(BinaryOperatorExpression exp, object b);

        object Execute(ULongIntegerLiteralExpression exp, object b);
        object Execute(UIntegerLiteralExpression exp, object b);
        object Execute(StringLiteralExpression exp, object b);
        object Execute(OperatorLiteralExpression exp, object b);
        object Execute(LongIntegerLiteralExpression exp, object b);
        object Execute(IntegerLiteralExpression exp, object b);
        object Execute(FloatLiteralExpression exp, object b);
        object Execute(DoubleLiteralExpression exp, object b);
        object Execute(CharLiteralExpression exp, object b);
        object Execute(BoolLiteralExpression exp, object b);

        object Execute(ReturnExpression exp, object b);
        object Execute(ContinueExpression exp, object b);
        object Execute(BreakExpression exp, object b);

        object Execute(WhileExpression exp, object b);
        object Execute(SwitchExpression exp, object b);
        object Execute(IfExpression exp, object b);
        object Execute(ForExpression exp, object b);
    }
}
