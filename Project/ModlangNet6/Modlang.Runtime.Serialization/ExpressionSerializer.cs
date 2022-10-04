using Modlang.Abstract;
using Modlang.Abstract.ControlStructures;
using Modlang.Abstract.Imperative;
using Modlang.Abstract.Literals;
using Modlang.Abstract.Types;
using Modlang.Lexing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime.Serialization
{
    public static class ExpressionSerializer
    {
        private delegate void Serialize(Expression e, Stream s, bool forward);

        private static Dictionary<Type, Serialize> _dict = new Dictionary<Type, Serialize>();
        private static readonly Type[] idxArray = new Type[]
        {
            typeof(UnaryOperatorExpression),
            typeof(OperatorFunctionExpression),
            typeof(NOPExpression),
            typeof(IdentifierExpression),
            typeof(FunctionExpression),
            typeof(DeclarationExpression),
            typeof(ConstructorExpression),
            typeof(CodeBlock),
            typeof(ClassExpression),
            typeof(CastExpression),
            typeof(BraceExpression),
            typeof(BinaryOperatorExpression),

            typeof(ULongIntegerLiteralExpression),
            typeof(UIntegerLiteralExpression),
            typeof(StringLiteralExpression),
            typeof(OperatorLiteralExpression),
            typeof(LongIntegerLiteralExpression),
            typeof(IntegerLiteralExpression),
            typeof(FloatLiteralExpression),
            typeof(DoubleLiteralExpression),
            typeof(CharLiteralExpression),
            typeof(BoolLiteralExpression),

            typeof(ReturnExpression),
            typeof(ContinueExpression),
            typeof(BreakExpression),

            typeof(WhileExpression),
            typeof(SwitchExpression),
            typeof(IfExpression),
            typeof(ForExpression)
        };

        public static byte TypeIndex(Expression t)
        {
            return (byte)Array.IndexOf(idxArray, t.GetType());
        }

        static ExpressionSerializer()
        {
            _dict.Add(typeof(UnaryOperatorExpression), (e, s, f) => Internal((UnaryOperatorExpression)e, s, f));
            _dict.Add(typeof(OperatorFunctionExpression), (e, s, f) => Internal((OperatorFunctionExpression)e, s, f));
            _dict.Add(typeof(NOPExpression), (e, s, f) => Internal((NOPExpression)e, s, f));
            _dict.Add(typeof(IdentifierExpression), (e, s, f) => Internal((IdentifierExpression)e, s, f));
            _dict.Add(typeof(FunctionExpression), (e, s, f) => Internal((FunctionExpression)e, s, f));
            _dict.Add(typeof(DeclarationExpression), (e, s, f) => Internal((DeclarationExpression)e, s, f));
            _dict.Add(typeof(ConstructorExpression), (e, s, f) => Internal((ConstructorExpression)e, s, f));
            _dict.Add(typeof(CodeBlock), (e, s, f) => Internal((CodeBlock)e, s, f));
            _dict.Add(typeof(ClassExpression), (e, s, f) => Internal((ClassExpression)e, s, f));
            _dict.Add(typeof(CastExpression), (e, s, f) => Internal((CastExpression)e, s, f));
            _dict.Add(typeof(BraceExpression), (e, s, f) => Internal((BraceExpression)e, s, f));
            _dict.Add(typeof(BinaryOperatorExpression), (e, s, f) => Internal((BinaryOperatorExpression)e, s, f));

            _dict.Add(typeof(ULongIntegerLiteralExpression), (e, s, f) => Internal((ULongIntegerLiteralExpression)e, s, f));
            _dict.Add(typeof(UIntegerLiteralExpression), (e, s, f) => Internal((UIntegerLiteralExpression)e, s, f));
            _dict.Add(typeof(StringLiteralExpression), (e, s, f) => Internal((StringLiteralExpression)e, s, f));
            _dict.Add(typeof(OperatorLiteralExpression), (e, s, f) => Internal((OperatorLiteralExpression)e, s, f));
            _dict.Add(typeof(LongIntegerLiteralExpression), (e, s, f) => Internal((LongIntegerLiteralExpression)e, s, f));
            _dict.Add(typeof(IntegerLiteralExpression), (e, s, f) => Internal((IntegerLiteralExpression)e, s, f));
            _dict.Add(typeof(FloatLiteralExpression), (e, s, f) => Internal((FloatLiteralExpression)e, s, f));
            _dict.Add(typeof(DoubleLiteralExpression), (e, s, f) => Internal((DoubleLiteralExpression)e, s, f));
            _dict.Add(typeof(CharLiteralExpression), (e, s, f) => Internal((CharLiteralExpression)e, s, f));
            _dict.Add(typeof(BoolLiteralExpression), (e, s, f) => Internal((BoolLiteralExpression)e, s, f));

            _dict.Add(typeof(ReturnExpression), (e, s, f) => Internal((ReturnExpression)e, s, f));
            _dict.Add(typeof(ContinueExpression), (e, s, f) => Internal((ContinueExpression)e, s, f));
            _dict.Add(typeof(BreakExpression), (e, s, f) => Internal((BreakExpression)e, s, f));

            _dict.Add(typeof(WhileExpression), (e, s, f) => Internal((WhileExpression)e, s, f));
            _dict.Add(typeof(SwitchExpression), (e, s, f) => Internal((SwitchExpression)e, s, f));
            _dict.Add(typeof(IfExpression), (e, s, f) => Internal((IfExpression)e, s, f));
            _dict.Add(typeof(ForExpression), (e, s, f) => Internal((ForExpression)e, s, f));
        }

        public static void Perform(this Expression exp, Stream s)
        {
            if(exp == null)
            {
                s.WriteBool(false);
            } else
            {
                s.WriteBool(true);
                s.WriteByte(TypeIndex(exp));
                _dict[exp.GetType()](exp, s, true);
            }
        }

        public static Expression Build(Stream s)
        {
            bool isNull = !s.ReadBool();
            if (isNull)
                return null;
            int idx = s.ReadByte();
            Type t = idxArray[idx];
            Expression exp = (Expression)Activator.CreateInstance(t);
            _dict[t](exp, s, false);
            return exp;
        }

        private static void Internal(UnaryOperatorExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteBool(exp.IsPrefix);
                s.WriteUTF8String(exp.Operator);
                exp.Inner.Perform(s);
            } else
            {
                exp.IsPrefix = s.ReadBool();
                exp.Operator = s.ReadUTF8String();
                exp.Inner = Build(s);
            }
            
        }

        private static void Internal(OperatorFunctionExpression exp, Stream s, bool forward)
        {
            if(forward)
            {
                s.WriteBool(exp.IsStatic);
                s.WriteByte((byte)exp.Modifier);
                exp.Name.Perform(s);
                s.WriteByte((byte)exp.OpModifier);
                s.WriteByte((byte)exp.BraceType);
                bool hasRetType = exp.ReturnType != null;
                s.WriteBool(hasRetType);
                if(hasRetType)
                    exp.ReturnType.Perform(s);
                s.WriteInt32(exp.Arguments.Length);
                for (int i = 0; i < exp.Arguments.Length; i++)
                    exp.Arguments[i].Perform(s);
                exp.Inner.Perform(s);
            } else
            {
                exp.IsStatic = s.ReadBool();
                exp.Modifier = (AccessModifier)s.ReadByte();
                exp.Name = (OperatorLiteralExpression)Build(s);
                exp.OpModifier = (OperatorFunctionExpression.OperatorModifiers)s.ReadByte();
                exp.BraceType = (BraceType)s.ReadByte();
                bool hasRetType = s.ReadBool();
                if (hasRetType)
                    exp.ReturnType = Build(s);
                else
                    exp.ReturnType = null;
                int argc = s.ReadInt32();
                exp.Arguments = new Expression[argc];
                for (int i = 0; i < argc; i++)
                    exp.Arguments[i] = Build(s);
                exp.Inner = Build(s);
            }
        }

        private static void Internal(NOPExpression exp, Stream s, bool forward)
        { }

        private static void Internal(IdentifierExpression exp, Stream s, bool forward)
        {
            if(forward)
            {
                s.WriteUTF8String(exp.Identifier);
            } else
            {
                exp.Identifier = s.ReadUTF8String();
            }
        }

        private static void Internal(FunctionExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteBool(exp.IsStatic);
                s.WriteByte((byte)exp.Modifier);
                s.WriteUTF8String(exp.Name);
                bool hasRetType = exp.ReturnType != null;
                s.WriteBool(hasRetType);
                if(hasRetType)
                    exp.ReturnType.Perform(s);
                s.WriteInt32(exp.Arguments.Length);
                for (int i = 0; i < exp.Arguments.Length; i++)
                    exp.Arguments[i].Perform(s);
                exp.Inner.Perform(s);
            }
            else
            {
                exp.IsStatic = s.ReadBool();
                exp.Modifier = (AccessModifier)s.ReadByte();
                exp.Name = s.ReadUTF8String();
                bool hasRetType = s.ReadBool();
                if (hasRetType)
                    exp.ReturnType = Build(s);
                else
                    exp.ReturnType = null;
                int argc = s.ReadInt32();
                exp.Arguments = new Expression[argc];
                for (int i = 0; i < argc; i++)
                    exp.Arguments[i] = Build(s);
                exp.Inner = Build(s);
            }
        }

        private static void Internal(DeclarationExpression exp, Stream s, bool forward)
        {
            if(forward)
            {
                s.WriteBool(exp.IsConst);
                s.WriteBool(exp.IsStatic);
                s.WriteByte((byte)exp.Modifier);
                exp.Datatype.Perform(s);
                s.WriteInt32(exp.Assignments.Length);
                for (int i = 0; i < exp.Assignments.Length; i++)
                    exp.Assignments[i].Perform(s);
            } else
            {
                exp.IsConst = s.ReadBool();
                exp.IsStatic = s.ReadBool();
                exp.Modifier = (AccessModifier)s.ReadByte();
                exp.Datatype = Build(s);
                int argc = s.ReadInt32();
                exp.Assignments = new Expression[argc];
                for (int i = 0; i < argc; i++)
                    exp.Assignments[i] = Build(s);
            }
        }

        private static void Internal(ConstructorExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteByte((byte)exp.Modifier);
                s.WriteInt32(exp.Arguments.Length);
                for (int i = 0; i < exp.Arguments.Length; i++)
                    exp.Arguments[i].Perform(s);
                exp.Inner.Perform(s);
            }
            else
            {
                exp.Modifier = (AccessModifier)s.ReadByte();
                int argc = s.ReadInt32();
                exp.Arguments = new Expression[argc];
                for (int i = 0; i < argc; i++)
                    exp.Arguments[i] = Build(s);
                exp.Inner = Build(s);
            }
        }

        private static void Internal(CodeBlock exp, Stream s, bool forward)
        {
            if(forward)
            {
                s.WriteInt32(exp.Expressions.Length);
                for (int i = 0; i < exp.Expressions.Length; i++)
                    exp.Expressions[i].Perform(s);
            } else
            {
                int argc = s.ReadInt32();
                exp.Expressions = new Expression[argc];
                for (int i = 0; i < argc; i++)
                    exp.Expressions[i] = Build(s);
            }
        }

        private static void Internal(ClassExpression exp, Stream s, bool forward)
        {
            if(forward)
            {
                s.WriteByte((byte)exp.Modifier);
                s.WriteBool(exp.IsPrimitive);
                int inherc = exp.InheritorExpressions == null ? 0 : exp.InheritorExpressions.Length;
                s.WriteInt32(inherc);
                for (int i = 0; i < inherc; i++)
                    exp.InheritorExpressions[i].Perform(s);
                s.WriteUTF8String(exp.Name);
                s.WriteInt32(exp.Inner.Length);
                for (int i = 0; i < exp.Inner.Length; i++)
                    exp.Inner[i].Perform(s);
            } else
            {
                exp.Modifier = (AccessModifier)s.ReadByte();
                exp.IsPrimitive = s.ReadBool();
                int inherc = s.ReadInt32();
                if (inherc != 0)
                    exp.InheritorExpressions = new Expression[inherc];
                else
                    exp.InheritorExpressions = null;
                for (int i = 0; i < inherc; i++)
                    exp.InheritorExpressions[i] = Build(s);
                exp.Name = s.ReadUTF8String();
                int innerc = s.ReadInt32();
                exp.Inner = new Expression[innerc];
                for (int i = 0; i < innerc; i++)
                    exp.Inner[i] = Build(s);
            }
        }

        private static void Internal(CastExpression exp, Stream s, bool forward)
        {
            if(forward)
            {
                exp.CastingExpression.Perform(s);
                exp.Subject.Perform(s);
            } else
            {
                exp.CastingExpression = Build(s);
                exp.Subject = Build(s);
            }
        }

        private static void Internal(BraceExpression exp, Stream s, bool forward)
        {
            if(forward)
            {
                s.WriteByte((byte)exp.Kind);
                exp.Subject.Perform(s);
                s.WriteInt32(exp.Arguments.Length);
                for (int i = 0; i < exp.Arguments.Length; i++)
                    exp.Arguments[i].Perform(s);
            } else
            {
                exp.Kind = (BraceType)s.ReadByte();
                exp.Subject = Build(s);
                int argc = s.ReadInt32();
                exp.Arguments = new Expression[argc];
                for (int i = 0; i < argc; i++)
                    exp.Arguments[i] = Build(s);
            }
        }

        private static void Internal(BinaryOperatorExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteUTF8String(exp.Operator);
                exp.Left.Perform(s);
                exp.Right.Perform(s);
            }
            else
            {
                exp.Operator = s.ReadUTF8String();
                exp.Left = Build(s);
                exp.Right = Build(s);
            }
        }

        private static void Internal(ULongIntegerLiteralExpression exp, Stream s, bool forward)
        {
            if(forward)
            {
                s.WriteUInt64(exp.Value);
            } else
            {
                exp.Value = s.ReadUInt64();
            }
        }

        private static void Internal(UIntegerLiteralExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteUInt32(exp.Value);
            }
            else
            {
                exp.Value = s.ReadUInt32();
            }
        }

        private static void Internal(StringLiteralExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteUTF8String(exp.Value);
            }
            else
            {
                exp.Value = s.ReadUTF8String();
            }
        }

        private static void Internal(OperatorLiteralExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteUTF8String(exp.Operator);
            }
            else
            {
                exp.Operator = s.ReadUTF8String();
            }
        }

        private static void Internal(LongIntegerLiteralExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteInt64(exp.Value);
            }
            else
            {
                exp.Value = s.ReadInt64();
            }
        }

        private static void Internal(IntegerLiteralExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteInt32(exp.Value);
            }
            else
            {
                exp.Value = s.ReadInt32();
            }
        }

        private static void Internal(FloatLiteralExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteFloat(exp.Value);
            }
            else
            {
                exp.Value = s.ReadFloat();
            }
        }

        private static void Internal(DoubleLiteralExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteDouble(exp.Value);
            }
            else
            {
                exp.Value = s.ReadDouble();
            }
        }

        private static void Internal(CharLiteralExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteUInt16((ushort)exp.Value);
            }
            else
            {
                exp.Value = (char)s.ReadInt16();
            }
        }

        private static void Internal(BoolLiteralExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                s.WriteBool(exp.Value);
            }
            else
            {
                exp.Value = s.ReadBool();
            }
        }

        private static void Internal(ReturnExpression exp, Stream s, bool forward)
        {
            if (forward)
            {
                bool hasExp = exp.Inner != null;
                s.WriteBool(hasExp);
                if (hasExp)
                    exp.Inner.Perform(s);
            }
            else
            {
                bool hasExp = s.ReadBool();
                if (hasExp)
                    exp.Inner = Build(s);
                else
                    exp.Inner = null;
            }
        }

        private static void Internal(ContinueExpression exp, Stream s, bool forward)
        { }

        private static void Internal(BreakExpression exp, Stream s, bool forward)
        { }

        private static void Internal(WhileExpression exp, Stream s, bool forward)
        {
            if(forward)
            {
                exp.Condition.Perform(s);
                exp.Inner.Perform(s);
            } else
            {
                exp.Condition = Build(s);
                exp.Inner = Build(s);
            }
        }

        private static void Internal(SwitchExpression exp, Stream s, bool forward)
        {
            if(forward)
            {
                exp.Subject.Perform(s);
                s.WriteInt32(exp.Cases.Length);
                for(int i = 0; i < exp.Cases.Length; i++)
                {
                    SwitchCase c = exp.Cases[i];
                    s.WriteBool(c.IsDefault);
                    if(!c.IsDefault)
                    {
                        c.Match.Perform(s);
                    }
                    s.WriteInt32(c.Inner.Length);
                    for (int j = 0; j < c.Inner.Length; j++)
                        c.Inner[j].Perform(s);
                }
            } else
            {
                exp.Subject = Build(s);
                int casec = s.ReadInt32();
                exp.Cases = new SwitchCase[casec];
                for (int i = 0; i < casec; i++)
                {
                    bool isDefault = s.ReadBool();
                    Expression match = null;
                    if (!isDefault)
                    {
                        match = Build(s);
                    }
                    int innerc = s.ReadInt32();
                    Expression[] inner = new Expression[innerc];
                    for (int j = 0; j < innerc; j++)
                        inner[j] = Build(s);
                    exp.Cases[i] = new SwitchCase(match, inner) { IsDefault = isDefault };
                }
            }
        }

        private static void Internal(IfExpression exp, Stream s, bool forward)
        {
            if(forward)
            {
                exp.Condition.Perform(s);
                exp.Inner.Perform(s);
                bool hasElse = exp.ElseInner != null;
                s.WriteBool(hasElse);
                if (hasElse)
                    exp.ElseInner.Perform(s);
            } else
            {
                exp.Condition = Build(s);
                exp.Inner = Build(s);
                bool hasElse = s.ReadBool();
                if (hasElse)
                    exp.ElseInner = Build(s);
                else
                    exp.ElseInner = null;
            }
        }

        private static void Internal(ForExpression exp, Stream s, bool forward)
        {
            if(forward)
            {
                s.WriteInt32(exp.StartingExpressions.Length);
                for (int i = 0; i < exp.StartingExpressions.Length; i++)
                    exp.StartingExpressions[i].Perform(s);

                exp.Condition.Perform(s);

                s.WriteInt32(exp.RoundExpressions.Length);
                for (int i = 0; i < exp.RoundExpressions.Length; i++)
                    exp.RoundExpressions[i].Perform(s);

                exp.Inner.Perform(s);
            } else
            {
                int sexpc = s.ReadInt32();
                exp.StartingExpressions = new Expression[sexpc];
                for (int i = 0; i < sexpc; i++)
                    exp.StartingExpressions[i] = Build(s);

                exp.Condition = Build(s);

                int rexpc = s.ReadInt32();
                exp.RoundExpressions = new Expression[rexpc];
                for (int i = 0; i < rexpc; i++)
                    exp.RoundExpressions[i] = Build(s);

                exp.Inner = Build(s);
            }
        }
    }
}
