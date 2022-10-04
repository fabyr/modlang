using Modlang.Abstract;
using Modlang.Abstract.ControlStructures;
using Modlang.Abstract.Imperative;
using Modlang.Abstract.Literals;
using Modlang.Abstract.Types;
using Modlang.Exceptions;
using Modlang.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Parsing
{
    public class Parser
    {
        private delegate Expression CommaEvaluatorBuildingFunction(out bool shouldBreak); // this is used down in the "CommaEvaluator" method

        public LexerResult BaseResult { get; }

        private int _parsePos = 0;
        private int Position { get => _parsePos; set => _parsePos = value; }

        private Token GetToken(int idx)
        {
            return BaseResult[idx];
        }

        // Returns the current token at T(0) and the surrounding token according to the offset
        // T(-1) would return the previous token and T(1) the next, T(2) the one after that and so on
        private Token T(int offset) => GetToken(Position + offset);

        // Checks if this token (or also the next ones) match the types supplied by the parameter
        // if for example IDENTIFIER, IDENTIFIER, EOI is supplied, it
        // checks if T(0) is IDENTIFIER, T(1) is IDENTIFIER and T(2) is EOI
        private bool Match(params TokenType[] types)
        {
            TokenType[] fetched = new TokenType[types.Length];
            for (int i = 0; i < fetched.Length; i++)
                fetched[i] = T(i).Kind;

            return types.SequenceEqual(fetched);
        }

        public Parser(LexerResult lr)
        {
            BaseResult = lr;
        }

        public Parser(List<Token> lrList)
        {
            BaseResult = new LexerResult();
            lrList.ForEach(BaseResult.Add);
        }

        private void EOIAssert()
        {
            if (T(0).Kind != TokenType.EOI)
                throw new ParserException((FromFileObject)T(0), $"Expected '{Constants.EOI}'. Instead got {T(0)}");
        }

        // checks if the current token matches the brace type and if it's the same opening "kind"
        private void BraceAssert(bool opening, BraceType bt)
        {
            string expectStr = string.Empty;
            switch (bt)
            {
                case BraceType.ROUND:
                    expectStr = opening ? "(" : ")";
                    break;
                case BraceType.SQUARE:
                    expectStr = opening ? "[" : "]";
                    break;
                case BraceType.CURLY:
                    expectStr = opening ? "{" : "}";
                    break;
                default:
                    break;
            }

            TokenType kind = T(0).Kind;

            bool braceCheck = Util.CheckBrace(kind, bt);
            bool isOpening = Util.IsOpeningBrace(kind);
            bool isClosing = Util.IsClosingBrace(kind);

            // If it isn't the correct brace type or if it mismatches the opening-type
            if (!braceCheck || (opening && !isOpening) || (!opening && !isClosing))
                throw new ParserException((FromFileObject)T(0), $"Expected ('{expectStr}'). Instead got {T(0)}");
        }

        private List<Expression> ParseCore(bool haltAtCurlyClosingBrace = true, int count = -1, ParserContext context = ParserContext.GLOBAL, params TokenType[] additionalBreak)
        {
            List<Expression> res = new List<Expression>();
            int instructionCount = 0;
            Token lastAccessModifierToken = null;
            AccessModifier mod = AccessModifier.DEFAULT;
            AccessModifier? modTmp;
            Dictionary<ParserMakerFlags, Token> lastToken = new Dictionary<ParserMakerFlags, Token>()
            {
                { ParserMakerFlags.STATIC_FLAG, null },
                { ParserMakerFlags.CONST_FLAG, null },
                { ParserMakerFlags.OPERATOR_FLAG, null },
                { ParserMakerFlags.PREFIX_FLAG, null },
                { ParserMakerFlags.POSTFIX_FLAG, null },
                { ParserMakerFlags.IMPLICIT_FLAG, null },
                { ParserMakerFlags.EXPLICIT_FLAG, null },
                { ParserMakerFlags.PRIMITIVE_FLAG, null },
                { ParserMakerFlags.SET_FLAG, null },
                { ParserMakerFlags.GET_FLAG, null }
            };
            ParserMakerFlags flags = ParserMakerFlags.NONE;

            void NoFlagsAssert(params ParserMakerFlags[] allowed)
            {
                const string msg = "Invalid modifier '{0}'.";
                if (flags.HasFlag(ParserMakerFlags.STATIC_FLAG) && !allowed.Contains(ParserMakerFlags.STATIC_FLAG))
                    throw new ParserException((FromFileObject)lastToken[ParserMakerFlags.STATIC_FLAG], string.Format(msg, "static"));
                if (flags.HasFlag(ParserMakerFlags.CONST_FLAG) && !allowed.Contains(ParserMakerFlags.CONST_FLAG))
                    throw new ParserException((FromFileObject)lastToken[ParserMakerFlags.CONST_FLAG], string.Format(msg, "const"));
                if (flags.HasFlag(ParserMakerFlags.OPERATOR_FLAG) && !allowed.Contains(ParserMakerFlags.OPERATOR_FLAG))
                    throw new ParserException((FromFileObject)lastToken[ParserMakerFlags.OPERATOR_FLAG], string.Format(msg, "operator"));
                if (flags.HasFlag(ParserMakerFlags.PREFIX_FLAG) && !allowed.Contains(ParserMakerFlags.PREFIX_FLAG))
                    throw new ParserException((FromFileObject)lastToken[ParserMakerFlags.PREFIX_FLAG], string.Format(msg, "prefix"));
                if (flags.HasFlag(ParserMakerFlags.POSTFIX_FLAG) && !allowed.Contains(ParserMakerFlags.POSTFIX_FLAG))
                    throw new ParserException((FromFileObject)lastToken[ParserMakerFlags.POSTFIX_FLAG], string.Format(msg, "postfix"));

                if (flags.HasFlag(ParserMakerFlags.IMPLICIT_FLAG) && !allowed.Contains(ParserMakerFlags.IMPLICIT_FLAG))
                    throw new ParserException((FromFileObject)lastToken[ParserMakerFlags.IMPLICIT_FLAG], string.Format(msg, "implicit"));
                if (flags.HasFlag(ParserMakerFlags.EXPLICIT_FLAG) && !allowed.Contains(ParserMakerFlags.EXPLICIT_FLAG))
                    throw new ParserException((FromFileObject)lastToken[ParserMakerFlags.EXPLICIT_FLAG], string.Format(msg, "explicit"));

                if (flags.HasFlag(ParserMakerFlags.PRIMITIVE_FLAG) && !allowed.Contains(ParserMakerFlags.PRIMITIVE_FLAG))
                    throw new ParserException((FromFileObject)lastToken[ParserMakerFlags.PRIMITIVE_FLAG], string.Format(msg, "primitive"));

                if (flags.HasFlag(ParserMakerFlags.GET_FLAG) && !allowed.Contains(ParserMakerFlags.GET_FLAG))
                    throw new ParserException((FromFileObject)lastToken[ParserMakerFlags.GET_FLAG], string.Format(msg, "get"));
                if (flags.HasFlag(ParserMakerFlags.SET_FLAG) && !allowed.Contains(ParserMakerFlags.SET_FLAG))
                    throw new ParserException((FromFileObject)lastToken[ParserMakerFlags.SET_FLAG], string.Format(msg, "set"));
            }

            void NoModAssert()
            {
                if (mod != AccessModifier.DEFAULT)
                    throw new ParserException(lastAccessModifierToken, "An access modifier is invalid here.");
            }

            void ClearFlags()
            {
                flags &= ~ParserMakerFlags.CONST_FLAG;
                flags &= ~ParserMakerFlags.STATIC_FLAG;
                flags &= ~ParserMakerFlags.OPERATOR_FLAG;
                flags &= ~ParserMakerFlags.PREFIX_FLAG;
                flags &= ~ParserMakerFlags.POSTFIX_FLAG;
                flags &= ~ParserMakerFlags.IMPLICIT_FLAG;
                flags &= ~ParserMakerFlags.EXPLICIT_FLAG;
                flags &= ~ParserMakerFlags.PRIMITIVE_FLAG;
                flags &= ~ParserMakerFlags.GET_FLAG;
                flags &= ~ParserMakerFlags.SET_FLAG;
                mod = AccessModifier.DEFAULT;
            }

            bool CheckFlagKeywords()
            {
                // TODO: Do it like this. Just have to implement a mapping method to go from tokentype to parsermakerflags
                /*foreach(TokenType tt in Enum.GetValues(typeof(TokenType)))
                {
                    if (Match(tt))
                    {
                        flags |= ParserMakerFlags.CONST_FLAG;
                        lastToken[ParserMakerFlags.CONST_FLAG] = T(0);
                        Step();
                    }
                }*/

                if (Match(TokenType.KEYWORD_CONST))
                {
                    flags |= ParserMakerFlags.CONST_FLAG;
                    lastToken[ParserMakerFlags.CONST_FLAG] = T(0);
                    Step();
                }
                else

                if (Match(TokenType.KEYWORD_STATIC))
                {
                    flags |= ParserMakerFlags.STATIC_FLAG;
                    lastToken[ParserMakerFlags.STATIC_FLAG] = T(0);
                    Step();
                }
                else

                if (Match(TokenType.KEYWORD_OPERATOR))
                {
                    flags |= ParserMakerFlags.OPERATOR_FLAG;
                    lastToken[ParserMakerFlags.OPERATOR_FLAG] = T(0);
                    Step();
                }
                else

                if (Match(TokenType.KEYWORD_PREFIX))
                {
                    flags |= ParserMakerFlags.PREFIX_FLAG;
                    lastToken[ParserMakerFlags.PREFIX_FLAG] = T(0);
                    Step();
                }
                else

                if (Match(TokenType.KEYWORD_POSTFIX))
                {
                    flags |= ParserMakerFlags.POSTFIX_FLAG;
                    lastToken[ParserMakerFlags.POSTFIX_FLAG] = T(0);
                    Step();
                }
                else

                if (Match(TokenType.KEYWORD_IMPLICIT))
                {
                    flags |= ParserMakerFlags.IMPLICIT_FLAG;
                    lastToken[ParserMakerFlags.IMPLICIT_FLAG] = T(0);
                    Step();
                }
                else

                if (Match(TokenType.KEYWORD_EXPLICIT))
                {
                    flags |= ParserMakerFlags.EXPLICIT_FLAG;
                    lastToken[ParserMakerFlags.EXPLICIT_FLAG] = T(0);
                    Step();
                }

                if (Match(TokenType.KEYWORD_PRIMITIVE))
                {
                    flags |= ParserMakerFlags.PRIMITIVE_FLAG;
                    lastToken[ParserMakerFlags.PRIMITIVE_FLAG] = T(0);
                    Step();
                }
                else

                if (Match(TokenType.KEYWORD_GET))
                {
                    flags |= ParserMakerFlags.GET_FLAG;
                    lastToken[ParserMakerFlags.GET_FLAG] = T(0);
                    Step();
                }

                if (Match(TokenType.KEYWORD_SET))
                {
                    flags |= ParserMakerFlags.SET_FLAG;
                    lastToken[ParserMakerFlags.SET_FLAG] = T(0);
                    Step();
                }
                else
                    return false;
                return true;
            }

            while (instructionCount != count && (!Match(TokenType.UNDEFINED)) 
                && (!haltAtCurlyClosingBrace || !Match(TokenType.CURLY_RBRACE))
                && !additionalBreak.Contains(T(0).Kind)) // go through every token (or all the tokens until the next closing curly brace,
                                                         // recursive use of this method makes it work that we need no counter for opened braces)
            {
                //Console.WriteLine(T(0));
                if ((modTmp = Util.GetModifierKeyword(T(0).Kind)).HasValue) // Access Modifiers
                { // This is done similarly to the lexer's structure. if an access-modifier-keyword is encountered is basically saves that and is "in a different context now" (but not really tho xd)
                    lastAccessModifierToken = T(0);
                    mod = modTmp.Value;
                    Step();
                }
                else

                if (CheckFlagKeywords())
                    continue;
                else

                if (Match(TokenType.KEYWORD_BREAK))
                {
                    NoModAssert();
                    NoFlagsAssert();
                    res.Add(new BreakExpression() { Origin = T(0) });
                    instructionCount++;
                    Step();
                    EOIAssert();
                    Step();
                } else

                if (Match(TokenType.KEYWORD_CONTINUE))
                {
                    NoModAssert();
                    NoFlagsAssert();
                    res.Add(new ContinueExpression() { Origin = T(0) });
                    instructionCount++;
                    Step();
                    EOIAssert();
                    Step();
                }
                else

                if (Match(TokenType.KEYWORD_IF) || Match(TokenType.KEYWORD_WHILE)) // if-statement or while-loop (structure is identical)
                {
                    Token starterToken = T(0);
                    NoModAssert();
                    NoFlagsAssert();
                    bool isIf = Match(TokenType.KEYWORD_IF);
                    Step();
                    BraceAssert(true, BraceType.ROUND);
                    Step();
                    Expression condition = MakeExpression();
                    BraceAssert(false, BraceType.ROUND);
                    Step();
                    List<Expression> innerList = ParseCore(false, 1, context: ParserContext.LOCAL); // parse the statement afterwards (only 1)
                    if (innerList.Count != 1)
                        throw new ParserException((FromFileObject)T(0), "Unexpected end of code."); // TODO: Proper Error Message
                    Expression inner = innerList[0];

                    if (isIf)
                    {
                        Expression elseExp = null;
                        if(Match(TokenType.KEYWORD_ELSE))
                        {
                            Step(); // step over "else"
                            innerList = ParseCore(false, 1); // parse the statement afterwards (only 1)
                            if (innerList.Count != 1)
                                throw new ParserException((FromFileObject)T(0), "Unexpected end of code."); // TODO: Proper Error Message // TODO: Proper Error Message
                            elseExp = innerList[0];
                        }
                        res.Add(new IfExpression(condition, inner, elseExp) { Origin = starterToken });
                    }
                    else
                        res.Add(new WhileExpression(condition, inner) { Origin = starterToken });
                    instructionCount++;
                }

                else if(Match(TokenType.KEYWORD_FOR)) // for loop
                {
                    Token starterToken = T(0);
                    NoModAssert();
                    NoFlagsAssert();
                    Step();
                    BraceAssert(true, BraceType.ROUND);
                    Step();

                    List<Expression> startingExps = new List<Expression>();
                    Expression first = MakeExpression();
                    if (Match(TokenType.IDENTIFIER)) // Special Case where a declaration is allowed inside another statement
                    {
                        startingExps.Add(MakeDeclaration(first, stepLastEOI: false));
                    }
                    else // if it's not a declaration, we manually need to parse for comma-seperated expressions, in contrary to the declaration one which handles that internally already
                    {
                        startingExps.Add(first);
                        if(Match(TokenType.OP_COMMA)) // if a comma follows the first expression, we know more have to come
                        {
                            Step();
                            startingExps.AddRange(CommaEvaluator((out bool br) => // we can have multiple starting-expressions, seperated by commas
                            {
                                br = false;
                                return MakeExpression();
                            }));
                        }
                    }

                    EOIAssert();
                    Step();
                    Expression condition = MakeExpression();
                    EOIAssert();
                    Step();

                    List<Expression> roundExpressions = T(0).Kind == TokenType.RBRACE ? new List<Expression>() : CommaEvaluator((out bool br) => // we can have multiple round-expressions, seperated by commas
                    {
                        br = false;
                        return MakeExpression(additionalBreak: TokenType.OP_COMMA);
                    });

                    //Expression roundExp = MakeGeneralExpression(true);
                    
                    BraceAssert(false, BraceType.ROUND);
                    Step();

                    List<Expression> innerList = ParseCore(false, 1, ParserContext.LOCAL); // parse the statement afterwards (only 1)
                    if (innerList.Count != 1)
                        throw new ParserException((FromFileObject)T(0), "Unexpected end of code."); // TODO: Proper Error Message
                    Expression inner = innerList[0];
                    res.Add(new ForExpression(startingExps.ToArray(), roundExpressions.ToArray(), condition, inner) { Origin = starterToken });
                    instructionCount++;
                }

                else if(Match(TokenType.KEYWORD_SWITCH))
                {
                    Token starterToken = T(0);
                    NoModAssert();
                    NoFlagsAssert();
                    Step();
                    BraceAssert(true, BraceType.ROUND);
                    Step();
                    Expression subject = MakeExpression();
                    BraceAssert(false, BraceType.ROUND);
                    Step();
                    BraceAssert(true, BraceType.CURLY);
                    Step();

                    bool hadDefault = false; // Keeps track if there already has been a default case so we can throw an exception on a second occourance
                    List<SwitchCase> cases = new List<SwitchCase>();
                    while(Match(TokenType.KEYWORD_CASE) || Match(TokenType.KEYWORD_DEFAULT))
                    {
                        bool isDefault = Match(TokenType.KEYWORD_DEFAULT);
                        if (hadDefault && isDefault)
                            throw new ParserException((FromFileObject)T(0), "A switch-statement cannot have multiple default-cases.");
                        Step();
                        Expression match = MakeExpression(additionalBreak: TokenType.OP_COLON);
                        if (!Match(TokenType.OP_COLON))
                            throw new ParserException((FromFileObject)T(0), $"Expected '{Constants.ColonOperator}'. Instead got {T(0)}");
                        Step();
                        List<Expression> innerExps = ParseCore(false, additionalBreak: new[] { TokenType.KEYWORD_CASE, TokenType.KEYWORD_DEFAULT, TokenType.CURLY_RBRACE }, context: ParserContext.LOCAL);

                        cases.Add(new SwitchCase(match, innerExps.ToArray()) { IsDefault = isDefault });

                        hadDefault |= isDefault; // or it to the existing value
                    }
                    BraceAssert(false, BraceType.CURLY);
                    Step();

                    res.Add(new SwitchExpression(subject, cases.ToArray()) { Origin = starterToken });
                    instructionCount++;
                }

                else if(Match(TokenType.KEYWORD_RETURN))
                {
                    Token starterToken = T(0);
                    NoModAssert();
                    NoFlagsAssert();
                    Step();
                    Expression inner = null;
                    if(!Match(TokenType.EOI))
                        inner = MakeExpression();
                    EOIAssert();
                    Step();

                    res.Add(new ReturnExpression(inner) { Origin = starterToken });
                    instructionCount++;
                }

                else if(Match(TokenType.KEYWORD_CLASS))
                {
                    Token starterToken = T(0);
                    Step();
                    if (!Match(TokenType.IDENTIFIER))
                        throw new ParserException((FromFileObject)T(0), $"Expected class name. Instead got {T(0)}");

                    NoFlagsAssert(ParserMakerFlags.PRIMITIVE_FLAG); // TODO: Implement static classes

                    string name = T(0).Content;
                    List<Expression> inherits = null;
                    Step(); // step name
                    if(Match(TokenType.OP_COLON)) // if colon follows, the superclasses must follow
                    {
                        Step();
                        inherits = CommaEvaluator((out bool br) => // multiple seperated by comma
                        {
                            br = false;
                            return MakeExpression(additionalBreak: TokenType.CURLY_LBRACE, makerFlags: ExpressionMakerFlags.COMMA_HANDLED | ExpressionMakerFlags.ALLOW_CURLY_BRACE_EXPRESSION);
                        });
                    }
                    BraceAssert(true, BraceType.CURLY);
                    Step();
                    List<Expression> innerExps = ParseCore(context: ParserContext.CLASS); // // anything can be inside a class for now
                    BraceAssert(false, BraceType.CURLY);
                    Step();
                    res.Add(new ClassExpression(mod, name, innerExps.ToArray(), inherits?.ToArray()) { Origin = starterToken, IsPrimitive = flags.HasFlag(ParserMakerFlags.PRIMITIVE_FLAG) });
                    instructionCount++;
                    ClearFlags();
                }

                else if (context == ParserContext.CLASS && 
                    Match(TokenType.KEYWORD_THIS, TokenType.LBRACE)) // constructor for class
                {
                    Token starterToken = T(0);
                    Step();
                    Step();

                    NoFlagsAssert(); // TODO: Implement static constructors

                    List<Expression> args = Match(TokenType.RBRACE) ? new List<Expression>() : CommaEvaluator((out bool br) =>
                    {
                        br = false;
                        return MakeDeclaration(null, 1); // every parameter is in itself a declaration, but only 1 between each comma
                    });
                    BraceAssert(false, BraceType.ROUND);
                    Step();
                    List<Expression> innerList = ParseCore(false, 1, context: ParserContext.LOCAL); // parse the statement afterwards (only 1)
                    if (innerList.Count != 1)
                        throw new ParserException((FromFileObject)T(0), "Unexpected end of code.");
                    Expression inner = innerList[0];

                    // If the return-type expression is exactly the IdentifierExpression "void", then we give "null" as the return type for the function (handled internally then)
                    res.Add(new ConstructorExpression(mod, args.ToArray(), inner) { Origin = starterToken });
                    instructionCount++;
                    ClearFlags();
                }

                else // Everything else. i.e. every other normal statement
                {
                    Expression exp = MakeExpression();
                    while(CheckFlagKeywords()); // Check for keywords again (for example if the "operator" or similar keywords follow a function return type expression)
                    if (exp == null) // if exp is null, that means there was just a standalone ';', i.e. a NOP
                    {
                        // Just to be sure xD
                        NoModAssert();
                        NoFlagsAssert();

                        EOIAssert(); // Just to catch something if it were to behave weirdly
                        Step(); // Step over the EOI
                        res.Add(new NOPExpression() { Origin = T(0) });
                        instructionCount++;
                    }
                    else
                    {
                        if (T(-1).Kind != TokenType.CURLY_RBRACE) // if it is a code block end, it is allowed to have no EOI (also no declaration is possible)
                        {
                            // I'M JUST ABSURDLY DUMB. FOR A SECOND I THOUGHT SOMETHING WITHOUT A RETURN TYPE COULD BE A FUNCTION SMH
                            /*if(T(0).Kind == TokenType.CURLY_LBRACE && 
                                exp is BraceExpression bexp && bexp.Kind == BraceType.ROUND) // also a function
                            {
                                if (!Util.IsIdentifyingExpression(bexp.Subject))
                                    throw new ParserException();

                                List<Expression> innerList = ParseCore(false, 1); // parse the statement afterwards (only 1)
                                if (innerList.Count != 1)
                                    throw new ParserException((FromFileObject)T(0), "Unexpected end of code.");
                                Expression inner = innerList[0];

                                // If the return-type expression is exactly the IdentifierExpression "void", then we give "null" as the return type for the function (handled internally then)
                                exp = new FunctionExpression(name, mod, args?.ToArray(), inner, exp is IdentifierExpression expression && expression.Identifier.Equals("void") ? null : exp) { IsStatic = staticFlag };
                                flags &= ~ParseMakerFlags.CONST_FLAG;
                                flags &= ~ParseMakerFlags.STATIC_FLAG;
                                mod = AccessModifier.DEFAULT;
                            }
                            else*/
                            if(context == ParserContext.CLASS &&
                                flags.HasFlag(ParserMakerFlags.OPERATOR_FLAG))
                            {
                                NoFlagsAssert(ParserMakerFlags.OPERATOR_FLAG, ParserMakerFlags.POSTFIX_FLAG, ParserMakerFlags.PREFIX_FLAG, ParserMakerFlags.IMPLICIT_FLAG, ParserMakerFlags.EXPLICIT_FLAG, ParserMakerFlags.GET_FLAG,
                                    ParserMakerFlags.SET_FLAG);

                                bool postfix = false, prefix = false, set = false;
                                bool @implicit = false, @explicit = false, get = false;

                                if ((postfix = flags.HasFlag(ParserMakerFlags.POSTFIX_FLAG))
                                    & (prefix = flags.HasFlag(ParserMakerFlags.PREFIX_FLAG))) // check if only either one but not both have been supplied (also note the binary-& operator is used, so that both statements execute)
                                    throw new ParserException((FromFileObject)T(0), "Cannot have both prefix and postfix.");

                                if ((@implicit = flags.HasFlag(ParserMakerFlags.IMPLICIT_FLAG))
                                    & (@explicit = flags.HasFlag(ParserMakerFlags.EXPLICIT_FLAG))) // check if only either one but not both have been supplied (also note the binary-& operator is used, so that both statements execute)
                                    throw new ParserException((FromFileObject)T(0), "Cannot have both implicit and explicit.");

                                if ((get = flags.HasFlag(ParserMakerFlags.GET_FLAG))
                                    & (set = flags.HasFlag(ParserMakerFlags.SET_FLAG))) // check if only either one but not both have been supplied (also note the binary-& operator is used, so that both statements execute)
                                    throw new ParserException((FromFileObject)T(0), "Cannot have both set and get.");

                                // put the "unary-modifers", "cast-operator-modifiers" and "this"-operator modifiers in a list
                                // and count the "true" values
                                // if there are more than 1 "true" values we have an invalid mix
                                if (new[] { (postfix || prefix), (@implicit || @explicit), (set || get) }.Count(_ => _) > 1)
                                    throw new ParserException((FromFileObject)T(0), "Invalid mix of operator modifiers.");

                                OperatorFunctionExpression.OperatorModifiers opMod = postfix ? OperatorFunctionExpression.OperatorModifiers.POSTFIX : 
                                                                                      prefix ? OperatorFunctionExpression.OperatorModifiers.PREFIX  :
                                                                                      @implicit ? OperatorFunctionExpression.OperatorModifiers.IMPLICIT :
                                                                                      @explicit ? OperatorFunctionExpression.OperatorModifiers.EXPLICIT :
                                                                                      set ? OperatorFunctionExpression.OperatorModifiers.SET :
                                                                                      get ? OperatorFunctionExpression.OperatorModifiers.GET :
                                                                                               OperatorFunctionExpression.OperatorModifiers.NORMAL;
                                OperatorLiteralExpression name = null;
                                if (T(0).Kind == TokenType.OPERATOR)
                                {
                                    name = new OperatorLiteralExpression(T(0).Content);
                                    Step();
                                }
                                else if (T(0).Kind == TokenType.KEYWORD_THIS)
                                {
                                    name = new OperatorLiteralExpression(T(0).Content); // CHANGED (was an identifier expression previously)
                                    Step();
                                }
                                else
                                    throw new ParserException(T(0), "Invalid overloaded operator");
                                    //name = MakeExpression(additionalBreak: TokenType.LBRACE);

                                BraceType bt;
                                switch (T(0).Kind)
                                {
                                    case TokenType.LBRACE:
                                        bt = BraceType.ROUND;
                                        break;
                                    case TokenType.SQUARE_LBRACE: // TODO: Currently []-Operators are read only, need to implement get & set parts of the method (like C#-Properties)
                                        bt = BraceType.SQUARE;
                                        break;
                                    default:
                                        throw new ParserException((FromFileObject)T(0), $"Invalid Brace: {T(0)}");
                                }

                                // Everything following is exactly the same as in a normal function
                                Step();
                                // here we check if an rbrace immediately follows, if so, there are no parameters
                                List<Expression> args = Match(TokenType.RBRACE) ? new List<Expression>() : CommaEvaluator((out bool br) =>
                                {
                                    br = false;
                                    return MakeDeclaration(null, 1); 
                                });
                                BraceAssert(false, bt);
                                Step();
                                List<Expression> innerList = ParseCore(false, 1, context: ParserContext.LOCAL);
                                if (innerList.Count != 1)
                                    throw new ParserException((FromFileObject)T(0), "Unexpected end of code.");
                                Expression inner = innerList[0];

                                // If the return-type expression is exactly the IdentifierExpression "void", then we give "null" as the return type for the function (handled internally then)
                                exp = new OperatorFunctionExpression(name, opMod, mod, bt, args?.ToArray(), inner, exp is IdentifierExpression expression && expression.Identifier.Equals("void") ? null : exp) { IsStatic = flags.HasFlag(ParserMakerFlags.STATIC_FLAG), Origin = exp.Origin };
                                ClearFlags();
                            } else
                            if (Match(TokenType.IDENTIFIER))
                            {
                                if (T(1).Kind != TokenType.LBRACE) // identifier following expression, but no brace -> declaration
                                {
                                    // "attribute" which is actually a C#-Like property with either get or set
                                    // i.e. disguised method
                                    if(flags.HasFlag(ParserMakerFlags.GET_FLAG) ^ flags.HasFlag(ParserMakerFlags.SET_FLAG))
                                    {
                                        if (context == ParserContext.LOCAL)
                                            throw new ParserException(T(0), "A set- or get-function cannot be declared in a local context");
                                        NoFlagsAssert(ParserMakerFlags.GET_FLAG, ParserMakerFlags.SET_FLAG, ParserMakerFlags.STATIC_FLAG);
                                        bool isSet = flags.HasFlag(ParserMakerFlags.SET_FLAG);

                                        string name = T(0).Content;
                                        Step();
                                        List<Expression> innerList = ParseCore(false, 1, ParserContext.LOCAL); // parse the statement afterwards (only 1)
                                        if (innerList.Count != 1)
                                            throw new ParserException((FromFileObject)T(0), "Unexpected end of code.");
                                        Expression inner = innerList[0];

                                        // If the return-type expression is exactly the IdentifierExpression "void", then we give "null" as the return type for the function (handled internally then)
                                        exp = new AttributeFunctionExpression(name, mod, inner, exp is IdentifierExpression expression && expression.Identifier.Equals("void") ? null : exp, isSet) { IsStatic = flags.HasFlag(ParserMakerFlags.STATIC_FLAG), Origin = exp.Origin };
                                        ClearFlags();
                                    } else
                                    {
                                        NoFlagsAssert(ParserMakerFlags.CONST_FLAG, ParserMakerFlags.STATIC_FLAG);
                                        DeclarationExpression decl = MakeDeclaration(exp);
                                        decl.Modifier = mod;
                                        decl.IsConst = flags.HasFlag(ParserMakerFlags.CONST_FLAG);
                                        if (flags.HasFlag(ParserMakerFlags.CONST_FLAG) && flags.HasFlag(ParserMakerFlags.STATIC_FLAG))
                                            throw new ParserException((FromFileObject)lastToken[ParserMakerFlags.STATIC_FLAG], "Invalid modifier 'static'. If something is declared as constant, it implicitly is always static.");
                                        decl.IsStatic = flags.HasFlag(ParserMakerFlags.STATIC_FLAG);
                                        decl.Origin = exp.Origin;
                                        exp = decl;
                                        ClearFlags();
                                    }
                                } else // if an opening brace follows the identifier, it must be a function
                                {
                                    if (context == ParserContext.LOCAL)
                                        throw new ParserException(T(0), "A function cannot be declared in a local context");
                                    NoFlagsAssert(ParserMakerFlags.STATIC_FLAG);
                                    string name = T(0).Content;
                                    Step();
                                    BraceAssert(true, BraceType.ROUND);
                                    Step();
                                    // here we check if an rbrace immediately follows, if so, there are no parameters
                                    List<Expression> args = Match(TokenType.RBRACE) ? new List<Expression>() : CommaEvaluator((out bool br) =>
                                    {
                                        br = false;
                                        return MakeDeclaration(null, 1); // every parameter is in itself a declaration, but only 1 between each comma
                                    });
                                    BraceAssert(false, BraceType.ROUND);
                                    Step();
                                    List<Expression> innerList = ParseCore(false, 1, ParserContext.LOCAL); // parse the statement afterwards (only 1)
                                    if (innerList.Count != 1)
                                        throw new ParserException((FromFileObject)T(0), "Unexpected end of code.");
                                    Expression inner = innerList[0];

                                    // If the return-type expression is exactly the IdentifierExpression "void", then we give "null" as the return type for the function (handled internally then)
                                    exp = new FunctionExpression(name, mod, args?.ToArray(), inner, exp is IdentifierExpression expression && expression.Identifier.Equals("void") ? null : exp) { IsStatic = flags.HasFlag(ParserMakerFlags.STATIC_FLAG), Origin = exp.Origin };
                                    ClearFlags();
                                }
                            } else
                            {
                                //NoModAssert();
                                //NoFlagsAssert();
                                EOIAssert(); // Check if an end-of-instruction follows
                                Step(); // Step over the EOI
                            }
                        }
                        res.Add(exp);
                        instructionCount++;
                    }
                }
            }

            return res;
        }

        public ParseResult Parse()
        {
            ParseResult res = new ParseResult();

            _parsePos = 0;

            ParseCore(false).RemoveNOPExpressions().ForEach(res.Add); // Add all of the expressions returned by ParseCore to the result

            Validator.Validate(res);

            return res;
        }


        // basically returns a list of expressions, where each of them is seperated by OP_COMMA from the next one
        // but since in some cases entire declarations can be seperated this way, this method works using a user defined function,
        // which is called after each comma, and should - if made properly - return the expression up until the next comma
        private List<Expression> CommaEvaluator(CommaEvaluatorBuildingFunction builder)
        {
            List<Expression> list = new List<Expression>();
            bool first = true;
            do
            {
                if (!first)
                    Step();
                else
                    first = false;
                Expression exp = builder(out bool brCond); // Any expression can be in there
                

                if(exp != null) // inside the builder if you return "null" it is treated as discardable (makes sense in combination with the break condition, not to add the last one sometimes)
                    list.Add(exp);
                if (brCond)
                    break;
            } while (Match(TokenType.OP_COMMA)); // if there is a comma at the end of the expression, another one must follow

            return list;
        }

        // Makes a "Declaration" Object out of the current tokens
        private DeclarationExpression MakeDeclaration(Expression datatype, int count = -1, bool stepLastEOI = true)
        {
            FromFileObject starterToken;
            if (datatype == null)
            {
                starterToken = T(0);
                datatype = MakeExpression();
            }
            else
                starterToken = datatype.Origin;
            //string datatype = T(0).Content;
            //bool arrayType = false;
            //Step(); // Step over datatype
            /*if(MatchesTypes(TokenType.SQUARE_LBRACE, TokenType.SQUARE_RBRACE))
            {
                arrayType = true;
                Step(2);
            }*/
            int at = 0;
            List<Expression> list = CommaEvaluator((out bool br) => { at++; br = at == count; return MakeExpression(); });

            if (count == -1)
            {
                EOIAssert(); // if at the end there is no End of Instruction Token, the programmer screwed up
                if(stepLastEOI)
                    Step();
            }

            return new DeclarationExpression(datatype, list.ToArray()) { Origin = starterToken };
        }

        // Makes any Expression
        private Expression MakeExpression(ExpressionMakerFlags makerFlags = ExpressionMakerFlags.COMMA_HANDLED, params TokenType[] additionalBreak)
        {
            return ExpRec(Constants.MaxPrecedence, makerFlags, additionalBreak);
        }

        // Used internally by ExpRec. Whenever there are braces following the "left" expression, it is capsuled into a BraceExpression
        // For example identifier(4, 3, 2), where identifier is the left expression, would becomde such a brace expression with the parameters (each also allowed to be an entire expression)
        private Expression BraceExpression(Expression left, int precedence, ExpressionMakerFlags makerFlags)
        {
            // important: a while loop because multiple brace expressions can follow up another
            while (Util.IsOpeningBrace(T(0).Kind) && (!makerFlags.HasFlag(ExpressionMakerFlags.ALLOW_CURLY_BRACE_EXPRESSION) || !Util.CheckBrace(T(0).Kind, BraceType.CURLY)) && precedence == 1) // exp(a, b, ...) or exp[a, b, ...], basically calling expression or indexing expression
            {
                BraceType bt; // stores the opening-brace type, so that it can be checked, whether the closing one is of the correct type
                switch (T(0).Kind)
                {
                    case TokenType.LBRACE:
                        bt = BraceType.ROUND;
                        break;
                    case TokenType.SQUARE_LBRACE:
                        bt = BraceType.SQUARE;
                        break;
                    case TokenType.CURLY_LBRACE:
                        bt = BraceType.CURLY;
                        break;
                    default:
                        return left; // Just don't do anything in that case
                        //throw new ParserException((FromFileObject)T(0), $"Invalid Brace: {T(0)}"); // TODO: Proper Error Message
                }

                Step(); // Step over that opening brace

                // Basically the same code as in the declaration, parsed multiple expressions seperated by "OP_COMMA"
                List<Expression> list = CommaEvaluator((out bool br) =>
                {
                    if (Util.IsClosingBrace(T(0).Kind) && Util.CheckBrace(T(0).Kind, bt)) // if the closing brace is there already -> empty call
                    {
                        br = true;
                        return null;
                    }
                    br = false;
                    return MakeExpression();
                });
                /*do
                {
                    Step();
                    if (Util.IsClosingBrace(T(0).Kind) && Util.CheckBrace(T(0).Kind, bt)) // if the closing brace is there already -> empty call
                        break;
                    Expression expArg = MakeGeneralExpression(true);

                    list.Add(expArg);
                } while (T(0).Kind == TokenType.OP_COMMA);*/

                // If there is no closing brace or if the brace is of different type than the starting one, it is malformed code
                if (!Util.IsClosingBrace(T(0).Kind) || !Util.CheckBrace(T(0).Kind, bt))
                    throw new ParserException((FromFileObject)T(0), $"Expected closing brace. Instead got {T(0)}");
                Step(); // Skip this brace
                left = new BraceExpression(left, list.ToArray(), bt) { Origin = left.Origin };
            }
            return left; // if there are no braces or the precedence is not right, just return the original left
        }

        private Expression CastExpression(Expression left, int precedence, ExpressionMakerFlags makerFlags)
        {
            if (!Util.IsOperator(T(0).Kind) && !Util.IsClosingBrace(T(0).Kind) && precedence == Constants.CastPrecedence) // Casting-Functionality
            {

                Expression right = ExpRec(Constants.CastPrecedence - 1, makerFlags);

                if(right != null) // if the braced expression is at the end, this shouldn't become a cast expression xd
                    return new CastExpression(left, right);
            }
            return left;
        }

        // Recursive Function for parsing an arbitrary operator expression, precedence table is defined in "Modlang.Util.cs" in the Method "CheckTokenPrecedence"
        
        // BTW i don't know exactly why this "commaHandeled" parameter was necessary in the first place, as it is not now
        // but i'll leave it in anyway xD
        private Expression ExpRec(int precedence, ExpressionMakerFlags makerFlags, params TokenType[] additionalBreak)
        {
            bool commaHandled = makerFlags.HasFlag(ExpressionMakerFlags.COMMA_HANDLED);

            if (Util.IsEnding(T(0).Kind)
                || (!commaHandled && Match(TokenType.OP_COMMA))
                 || additionalBreak.Contains(T(0).Kind)
                /*|| Util.IsBrace(T(0).Kind)*/ // If we are at the end, then there is no expression
                )
            {
                //if(T(0).Kind == TokenType.EOI) Step();
                return null;
            }

            if (precedence < 0) // This is reached when there is no operator in this "Branch", i.e. it must be a literal or a parenthesized expression
            {
                // TODO: Maybe check if there are no weird edge cases with that
                if(Util.IsOperator(T(0).Kind, commaHandled) && !Util.IsSpecialOperator(T(0).Kind)) // Prefix Unary Handler, when nothing precedes it, e.g. ++identifier; as standalone statement
                {
                    OperatorLiteralExpression opExp = new OperatorLiteralExpression(T(0).Content) { Origin = T(0) };
                    string op = T(0).Content;
                    if (!Util.IsOperator(T(1).Kind, commaHandled))
                    {
                        Step();
                        Expression exp = ExpRec(Util.GetPrecedence(op, true, true), makerFlags, additionalBreak);
                        return new UnaryOperatorExpression(exp, opExp, true);
                    }

                    throw new ParserException((FromFileObject)T(1), $"Unexpected {T(1)}"); // TODO: Proper/Extensive Error Message
                } else

                if (Match(TokenType.KEYWORD_THIS)) // this
                {
                    Expression exp = new IdentifierExpression(T(0).Content);
                    Step();
                    return exp;
                } else

                if(Match(TokenType.IDENTIFIER)) // Is Identifer?
                {
                    Expression exp = new IdentifierExpression(T(0).Content);
                    Step();
                    return exp;
                } else if(Util.IsLiteral(T(0).Kind)) // Is Literal?
                {
                    Expression exp;
                    try
                    {
                        exp = Util.BuildLiteral(T(0));
                    } catch(Exception ex)
                    {
                        throw new ParserException((FromFileObject)T(0), ex.Message);
                    }
                    Step();
                    return exp;
                } else if(Util.IsOpeningBrace(T(0).Kind) && Util.CheckBrace(T(0).Kind, BraceType.ROUND)) // A Round-Paranethesis expression starts, e.g.   7 * (4 + 1)  <-- the round-braced part would be parsed here
                {
                    Step(); // Skip this brace               // TODO: Figure out if additionalBreak here is sometimes necessary
                    Expression exp = MakeExpression(makerFlags/*, additionalBreak*/); // make the expression inside it // TODO: commaHandled = true?
                    if(!Util.IsClosingBrace(T(0).Kind) || !Util.CheckBrace(T(0).Kind, BraceType.ROUND)) // if it's not the fitting closing brace, the input must be malformed
                    {
                        throw new ParserException((FromFileObject)T(0), $"Expected expression end. Instead got {T(0)}");
                    }
                    Step();

                    exp.Flags |= ExpressionFlags.BRACED;

                    return exp;
                }
                // Inside "BraceExpression", if a curly opening brace follows some identifier or other brace, it is instead processed there
                else if (Util.IsOpeningBrace(T(0).Kind) && Util.CheckBrace(T(0).Kind, BraceType.CURLY))
                {
                    Step(); // Skip this brace
                    CodeBlock exp = new CodeBlock(ParseCore(context: ParserContext.LOCAL).ToArray()); // parse all the expressions inside it
                    if (!Util.IsClosingBrace(T(0).Kind) || !Util.CheckBrace(T(0).Kind, BraceType.CURLY)) // if it's not the fitting closing brace, the input must be malformed
                    {
                        throw new ParserException((FromFileObject)T(0), $"Expected code-block end. Instead got {T(0)}");
                    }
                    Step();
                    return exp;
                }
                // If we somehow got here, without it being any of the above, something went wrong or the input is malformed
                throw new ParserException((FromFileObject)T(0), $"Unexpected {T(0)}"); // TODO: Proper/Extensive Error Message
            }

            // This is a very hard part
            // In my language every operator can either be unary or binary (normal)
            // but it is pre-defined which operators are "likely" to be unary.
            // for example in a expression like 7 * ++ i the "++" is chosen over the *, because it is common for it to be unary (list in Modlang.Util.cs in the "ShouldBeUnaryOperator"-Method)
            // in cases where it's obvious, like   7 * ++ * 8    the two *-operators will become unary and the "++" normal binary
            // and therefore it is necessary to know whether or not the current operator is unary, because the precedence changes
            
            bool isUnary = false;
            bool isPrefix = false;

            Token starterToken = T(0);

            Expression left, right;
            left = ExpRec(precedence - 1, makerFlags, additionalBreak);
            bool origLeftBraced = left.Flags.HasFlag(ExpressionFlags.BRACED); // stores if the originating left expression was braced

            if(!origLeftBraced) // You can only call on something which is not enclosed in braces
                left = BraceExpression(left, precedence, makerFlags); // If braces follow the left expression (this method returns the original "left" if there are no braces)

            if (origLeftBraced)
                left = CastExpression(left, precedence, makerFlags);

            ComputeUnary(ref isUnary, ref isPrefix, commaHandled); // This method identifies if the current operator is unary

            while (Util.CheckTokenPrecedence(T(0), isUnary, isPrefix && isUnary, precedence, commaHandled) 
                && !(additionalBreak.Contains(T(0).Kind)))
            {
                
                OperatorLiteralExpression opExp = new OperatorLiteralExpression(T(0).Content) { Origin = T(0) };
                bool rightAssociative = Util.IsAssignmentOperator(opExp.Operator);
                //string op = T(0).Content;
                if (isUnary && !isPrefix) // if a unary operator follows the current expression (left), e.g. identifier++ (Handler for the case when the "left" expression has an operator like that after it)
                {
                    left = new UnaryOperatorExpression(left, opExp, false); // Replace the current expression with the capsuled one with the operator
                    Step(); // We step now to the actualy binary operator, and re-enter the while loop if the condition is right
                } else
                {
                    Step();

                    //right = ExpRec(precedence - 1, commaHandled);

                    /* // THIS IS WRONG
                    // if we have another operator following the current one but the one after that one is not an operator, this one must be unary-prefix
                    if (Util.IsOperator(T(0).Kind, commaHandled) && !Util.IsOperator(T(1).Kind, commaHandled))
                    {
                        string opUn = T(0).Content; // store the unary-operator
                        Step(); // step over it and parse as normal
                        right = ExpRec(precedence - 1, commaHandled);
                        right = new UnaryOperatorExpression(right, opUn, true); // capsule the normal-right expression with the unary operator
                    } else*/
                        right = ExpRec(rightAssociative ? (precedence + 1) : (precedence - 1), makerFlags, additionalBreak); // otherwise just proceed as normal

                    // if we have another operator after the last expression (Handler for when the "right" expression is followed by a unary-postfix)
                    if(Util.IsOperator(T(0).Kind, commaHandled) && Util.IsEnding(T(1).Kind)
                        && Util.CheckTokenPrecedence(T(0), true, false, precedence, commaHandled)) // [] <++> ;
                    {
                        right = new UnaryOperatorExpression(right, new OperatorLiteralExpression(T(0).Content) { Origin = T(0) }, false); // capsule it
                        Step();
                    }
                    

                    if (right == null) // Sometimes happens, so we catch this
                        left = new UnaryOperatorExpression(left, opExp, false);
                    else
                        left = new BinaryOperatorExpression(left, right, opExp);

                    left = BraceExpression(left, precedence, makerFlags); // same as the above call
                }

                ComputeUnary(ref isUnary, ref isPrefix, commaHandled); // compute again, since we are now at a different operator

                // I don't think that's actually necessary // TODO: CHECK IF IT'S NECESSARY
                /*if (origLeftBraced) // cast-check again
                    left = CastExpression(left, precedence, commaHandled);*/
            }
            left.Origin = starterToken; // TODO: Better manage the assignment of the Origin Property. Currently this is sufficient
            return left;
        }

        // i'm too lazy to explain this method in detail (i kinda scratched this already above in the ExpRec method)
        // it basically identifies based on the surrounding operators,
        // if the current one is unary and if it is postfix or prefix 
        // this is BTW the method that "favors" ++ over * when it's a case where normally it can't be decided
        // for example:
        // 7 * * * 7 -> the center * will always be binary and the surrounding ones will be unary
        // 7 * ++ 7 -> normally there is no way to know which one should be unary, but by my definition the "++" is favored.
        // because if we were to go from left to right the * would become unary and the "++" a normal binary one, which i don't want >:C
        // BTW there is probably a bunch of optimization possible, BUT WHATEVER
        private void ComputeUnary(ref bool isUnary, ref bool isPrefix, bool comma)
        {
            if(!Util.IsOperator(T(0).Kind, comma) || Util.IsSpecialOperator(T(0).Kind)
                || Util.CannotBeUnaryOperator(T(0).Content))
            {
                isUnary = false;
                isPrefix = false;
                return;
            }
            /*bool[] opWindow = new bool[5];
            Func<int, bool> t = (_) => opWindow[_ + opWindow.Length / 2];
            for(int i = 0; i < opWindow.Length; i++)
            {
                opWindow[i] = Util.IsOperator(T(i - opWindow.Length / 2).Kind, comma);
            }

            if(t(-2))*/

            bool isLastOp = Util.IsOperator(T(-1).Kind, comma);
            bool isNextOp = Util.IsOperator(T(1).Kind, comma);
            bool shouldLast = Util.ShouldBeUnaryOperator(T(-1).Content, out _) && !Util.CannotBeUnaryOperator(T(-1).Content);
            bool shouldNext = Util.ShouldBeUnaryOperator(T(1).Content, out _) && !Util.CannotBeUnaryOperator(T(1).Content);

            // TODO: FIND A UNIFORM SOLUTION TO ALL OF THIS MADNESS

            bool? pref;
            bool p = isLastOp && !isNextOp;
            bool isUnaryShould = Util.ShouldBeUnaryOperator(T(0).Content, out pref) && !Util.CannotBeUnaryOperator(T(0).Content);
            if (isUnaryShould /*&& pref == null*/ && (!(isLastOp && isNextOp) && (Util.IsOpeningBrace(T(-1).Kind) || (Util.IsClosingBrace(T(1).Kind) || Util.IsEnding(T(1).Kind))) )
                && (pref == null || p == pref.Value)) // Added the pref == null because otherwise something breaks 🤷 // TODO: ACTUAL SOLUTION
            {
                isUnary = true;
                isPrefix = p;
                return;
            }

            // THIS IS WRONG
            /*if(shouldNext && !Util.IsOperator(T(2).Kind, comma)) // if the next one should be a unary one, don't make thise one a unary one, and also only if the operator after the next one is not an operator, otherwise it cannot be a unary operator due to the next one being in the middle of some operators
            {
                isUnary = false;
                isPrefix = false;
                return;
            }*/

            // ==== TMP
            if (!isLastOp && !isNextOp && Util.IsClosingBrace(T(1).Kind))
            {
                isUnary = true;
                isPrefix = false;
            } else

            if (!isLastOp && isNextOp && Util.IsOpeningBrace(T(-1).Kind))
            {
                isUnary = true;
                isPrefix = true;
            }
            else
            // ==== TMP

            if (isLastOp && isNextOp)
            {
                isUnary = false;
                isPrefix = false;
            } else if(isLastOp && !isNextOp && !shouldLast) // the !shouldLast makes it so if two unary operators are next to each other the first one is chosen
            {
                isUnary = true;
                isPrefix = true;
            }
            else if (!isLastOp && isNextOp && !shouldNext)
            {
                isUnary = true;
                isPrefix = false;
            } 
            /*else if(!isLastOp && !isNextOp)
            {
                isUnary = true;
                isPrefix = false; // This is wrong.  // TODO: Find out how to determine the "isPrefix" here
                // I guess it doesn't make too much of a difference tho
            }*/
            else
            {
                isUnary = false;
                isPrefix = false;
            }
        }

        // Used discretely at one point but i don't think it is strictly required anymore that we can step multiple at once
        private void Step(int amount)
        {
            Position += amount;
        }

        // Advance by one
        private void Step() => Step(1);
    }
}
