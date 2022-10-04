using Modlang.Abstract;
using Modlang.Abstract.ControlStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Parsing
{
    public static class Extensions
    {
        public static List<Expression> RemoveNOPExpressions(this List<Expression> list)
        {
            void reform(CodeBlock _) => _.Expressions = _.Expressions.ToList().RemoveNOPExpressions().ToArray();

            list.RemoveAll(_ => _ is NOPExpression);
            list.ForEach(_ => 
            {
                { if (_ is CodeBlock cb) reform(cb); }
                { if (_ is IfExpression exp && exp.Inner is CodeBlock cb) reform(cb); }
                { if (_ is WhileExpression exp && exp.Inner is CodeBlock cb) reform(cb); }
                { if (_ is ForExpression exp && exp.Inner is CodeBlock cb) reform(cb); }
                { if (_ is FunctionExpression exp && exp.Inner is CodeBlock cb) reform(cb); }
            }); // TODO: Make it so empty code blocks are also removed (in the same loop to save resources)
            return list;
        }
    }
}
