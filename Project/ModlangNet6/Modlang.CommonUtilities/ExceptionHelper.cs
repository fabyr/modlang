using Modlang.Abstract;
using Modlang.Abstract.ControlStructures;
using Modlang.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.CommonUtilities
{
    public class ExceptionHelper
    {
        private static void PrintSites(StreamReader sr, List<Expression> expressions)
        {
            foreach (Expression exp in expressions)
            {
                Console.WriteLine(exp.Origin.GetSiteFromStream(sr));

                PropertyInfo pi;
                if (exp is CodeBlock cbexp)
                {
                    PrintSites(sr, cbexp.Expressions.ToList());
                }
                else
                if (exp is IfExpression ifexp)
                {
                    PrintSites(sr, new List<Expression>() { ifexp.Inner, ifexp.ElseInner });
                }
                else
                if ((pi = exp.GetType().GetProperty("Inner")) != null)
                {
                    object expInner = pi.GetValue(exp);
                    PrintSites(sr, expInner is Expression[] expInnerList ? expInnerList.ToList() : new List<Expression>() { (Expression)expInner });
                }
            }
        }
    }
}
