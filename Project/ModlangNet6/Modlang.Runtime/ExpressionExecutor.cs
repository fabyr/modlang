using Modlang.Abstract;
using Modlang.Abstract.ControlStructures;
using Modlang.Abstract.Imperative;
using Modlang.Abstract.Literals;
using Modlang.Core.ForRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime
{
    internal class ExpressionExecutor : IExpressionExecutor
    {
        public object Execute(UnaryOperatorExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(OperatorFunctionExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(NOPExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(IdentifierExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(FunctionExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(FunctionBaseExpression exp, object b)
        {
            throw new InvalidOperationException();
            //return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(DeclarationExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(ConstructorExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(CodeBlock exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(ClassExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(CastExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(BraceExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(BinaryOperatorExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(ULongIntegerLiteralExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(UIntegerLiteralExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(StringLiteralExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(OperatorLiteralExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(LongIntegerLiteralExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(IntegerLiteralExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(FloatLiteralExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(DoubleLiteralExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(CharLiteralExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(BoolLiteralExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(ReturnExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(ContinueExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(BreakExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(WhileExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(SwitchExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(IfExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }

        public object Execute(ForExpression exp, object b)
        {
            return Execution.ExecuteInternal(exp, (Environment)b);
        }
    }
}
