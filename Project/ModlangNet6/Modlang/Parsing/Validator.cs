using Modlang.Abstract;
using Modlang.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Parsing
{
    public static class Validator
    {
        public static void Validate(ParseResult pr)
        {
            Validate(pr.AsEnumerable());
        }

        public static void Validate(params Expression[] expressions)
        {
            Validate(expressions.AsEnumerable());
        }

        public static void Validate(IEnumerable<Expression> expressions)
        {
            foreach (Expression exp in expressions)
            {
                exp.Process();
            }
        }

        public static void ValidateFunctionArguments(FromFileObject origin, Expression[] expressions)
        {
            if (!expressions.All(_ => _ is DeclarationExpression))
                throw new ParserException(origin, "Invalid parameter construct.");

            IEnumerable<DeclarationExpression> decls = expressions.Cast<DeclarationExpression>();
            foreach(DeclarationExpression dexp in decls)
            {
                dexp.Process();
                if(dexp.Assignments.Length != 1)
                    throw new ParserException(dexp.Origin, "Invalid parameter construct.");
                if (!dexp.Assignments.All(_ => _ is IdentifierExpression))
                    throw new ParserException(dexp.Origin, "Default parameters are not implemented yet. :/");
            }
        }
    }
}
