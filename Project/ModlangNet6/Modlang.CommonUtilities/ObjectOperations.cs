using System;
using System.Collections.Generic;
using System.Text;

namespace Modlang.CommonUtilities
{
    // Due to the restrictions on the use of "dynamic" this class is necessary
    // (Very not pretty :C)
    public static class ObjectOperations
    {
        public static object Add(object a, object b)
        {
            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa + (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa + (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa + (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa + (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa + (int)b;
                }
                else
                if (b is uint)
                {
                    return aa + (uint)b;
                }
                else
                if (b is long)
                {
                    return aa + (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa + (ulong)b;
                }
                else if (b is char)
                {
                    return aa + (char)b;
                }
                else if(b is float)
                {
                    return aa + (float)b;
                } else if (b is double)
                {
                    return aa + (double)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa + (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa + (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa + (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa + (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa + (int)b;
                }
                else
                if (b is uint)
                {
                    return aa + (uint)b;
                }
                else
                if (b is long)
                {
                    return aa + (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa + (ulong)b;
                }
                else if (b is char)
                {
                    return aa + (char)b;
                }
                else if (b is float)
                {
                    return aa + (float)b;
                }
                else if (b is double)
                {
                    return aa + (double)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa + (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa + (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa + (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa + (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa + (int)b;
                }
                else
                if (b is uint)
                {
                    return aa + (uint)b;
                }
                else
                if (b is long)
                {
                    return aa + (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa + (ulong)b;
                }
                else if(b is char)
                {
                    return aa + (char)b;
                }
                else if (b is float)
                {
                    return aa + (float)b;
                }
                else if (b is double)
                {
                    return aa + (double)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa + (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa + (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa + (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa + (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa + (int)b;
                }
                else
                if (b is uint)
                {
                    return aa + (uint)b;
                }
                else
                if (b is long)
                {
                    return aa + (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa + (ulong)b;
                }
                else if (b is char)
                {
                    return aa + (char)b;
                }
                else if (b is float)
                {
                    return aa + (float)b;
                }
                else if (b is double)
                {
                    return aa + (double)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa + (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa + (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa + (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa + (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa + (int)b;
                }
                else
                if (b is uint)
                {
                    return aa + (uint)b;
                }
                else
                if (b is long)
                {
                    return aa + (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa + (ulong)b;
                }
                else if (b is char)
                {
                    return aa + (char)b;
                }
                else if (b is float)
                {
                    return aa + (float)b;
                }
                else if (b is double)
                {
                    return aa + (double)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa + (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa + (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa + (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa + (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa + (int)b;
                }
                else
                if (b is uint)
                {
                    return aa + (uint)b;
                }
                else
                if (b is long)
                {
                    return aa + (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa + (ulong)b;
                }
                else if (b is char)
                {
                    return aa + (char)b;
                }
                else if (b is float)
                {
                    return aa + (float)b;
                }
                else if (b is double)
                {
                    return aa + (double)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa + (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa + (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa + (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa + (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa + (int)b;
                }
                else
                if (b is uint)
                {
                    return aa + (uint)b;
                }
                else
                if (b is long)
                {
                    return aa + (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa + (ulong)b;
                }
                else if (b is char)
                {
                    return aa + (char)b;
                }
                else if (b is float)
                {
                    return aa + (float)b;
                }
                else if (b is double)
                {
                    return aa + (double)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa + (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa + (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa + (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa + (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa + (int)b;
                }
                else
                if (b is uint)
                {
                    return aa + (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa + (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa + (ulong)b;
                }
                else if (b is char)
                {
                    return aa + (char)b;
                }
                else if (b is float)
                {
                    return aa + (float)b;
                }
                else if (b is double)
                {
                    return aa + (double)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa + (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa + (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa + (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa + (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa + (int)b;
                }
                else
                if (b is uint)
                {
                    return aa + (uint)b;
                }
                else
                if (b is long)
                {
                    return aa + (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa + (ulong)b;
                }
                else if (b is char)
                {
                    return aa + (char)b;
                }
                else if (b is float)
                {
                    return aa + (float)b;
                }
                else if (b is double)
                {
                    return aa + (double)b;
                }
            }
            else
            if (a is float)
            {
                float aa = (float)a;
                if (b is byte)
                {
                    return aa + (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa + (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa + (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa + (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa + (int)b;
                }
                else
                if (b is uint)
                {
                    return aa + (uint)b;
                }
                else
                if (b is long)
                {
                    return aa + (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa + (ulong)b;
                }
                else if (b is char)
                {
                    return aa + (char)b;
                }
                else if (b is float)
                {
                    return aa + (float)b;
                }
                else if (b is double)
                {
                    return aa + (double)b;
                }
            }
            else
            if (a is double)
            {
                double aa = (double)a;
                if (b is byte)
                {
                    return aa + (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa + (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa + (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa + (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa + (int)b;
                }
                else
                if (b is uint)
                {
                    return aa + (uint)b;
                }
                else
                if (b is long)
                {
                    return aa + (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa + (ulong)b;
                }
                else if (b is char)
                {
                    return aa + (char)b;
                }
                else if (b is float)
                {
                    return aa + (float)b;
                }
                else if (b is double)
                {
                    return aa + (double)b;
                }
            }
            throw new Exception();
        }

        public static object Sub(object a, object b)
        {
            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa - (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa - (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa - (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa - (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa - (int)b;
                }
                else
                if (b is uint)
                {
                    return aa - (uint)b;
                }
                else
                if (b is long)
                {
                    return aa - (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa - (ulong)b;
                }
                else if (b is char)
                {
                    return aa - (char)b;
                }
                else if (b is float)
                {
                    return aa - (float)b;
                }
                else if (b is double)
                {
                    return aa - (double)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa - (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa - (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa - (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa - (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa - (int)b;
                }
                else
                if (b is uint)
                {
                    return aa - (uint)b;
                }
                else
                if (b is long)
                {
                    return aa - (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa - (ulong)b;
                }
                else if (b is char)
                {
                    return aa - (char)b;
                }
                else if (b is float)
                {
                    return aa - (float)b;
                }
                else if (b is double)
                {
                    return aa - (double)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa - (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa - (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa - (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa - (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa - (int)b;
                }
                else
                if (b is uint)
                {
                    return aa - (uint)b;
                }
                else
                if (b is long)
                {
                    return aa - (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa - (ulong)b;
                }
                else if (b is char)
                {
                    return aa - (char)b;
                }
                else if (b is float)
                {
                    return aa - (float)b;
                }
                else if (b is double)
                {
                    return aa - (double)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa - (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa - (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa - (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa - (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa - (int)b;
                }
                else
                if (b is uint)
                {
                    return aa - (uint)b;
                }
                else
                if (b is long)
                {
                    return aa - (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa - (ulong)b;
                }
                else if (b is char)
                {
                    return aa - (char)b;
                }
                else if (b is float)
                {
                    return aa - (float)b;
                }
                else if (b is double)
                {
                    return aa - (double)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa - (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa - (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa - (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa - (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa - (int)b;
                }
                else
                if (b is uint)
                {
                    return aa - (uint)b;
                }
                else
                if (b is long)
                {
                    return aa - (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa - (ulong)b;
                }
                else if (b is char)
                {
                    return aa - (char)b;
                }
                else if (b is float)
                {
                    return aa - (float)b;
                }
                else if (b is double)
                {
                    return aa - (double)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa - (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa - (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa - (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa - (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa - (int)b;
                }
                else
                if (b is uint)
                {
                    return aa - (uint)b;
                }
                else
                if (b is long)
                {
                    return aa - (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa - (ulong)b;
                }
                else if (b is char)
                {
                    return aa - (char)b;
                }
                else if (b is float)
                {
                    return aa - (float)b;
                }
                else if (b is double)
                {
                    return aa - (double)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa - (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa - (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa - (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa - (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa - (int)b;
                }
                else
                if (b is uint)
                {
                    return aa - (uint)b;
                }
                else
                if (b is long)
                {
                    return aa - (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa - (ulong)b;
                }
                else if (b is char)
                {
                    return aa - (char)b;
                }
                else if (b is float)
                {
                    return aa - (float)b;
                }
                else if (b is double)
                {
                    return aa - (double)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa - (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa - (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa - (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa - (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa - (int)b;
                }
                else
                if (b is uint)
                {
                    return aa - (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa - (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa - (ulong)b;
                }
                else if (b is char)
                {
                    return aa - (char)b;
                }
                else if (b is float)
                {
                    return aa - (float)b;
                }
                else if (b is double)
                {
                    return aa - (double)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa - (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa - (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa - (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa - (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa - (int)b;
                }
                else
                if (b is uint)
                {
                    return aa - (uint)b;
                }
                else
                if (b is long)
                {
                    return aa - (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa - (ulong)b;
                }
                else if (b is char)
                {
                    return aa - (char)b;
                }
                else if (b is float)
                {
                    return aa - (float)b;
                }
                else if (b is double)
                {
                    return aa - (double)b;
                }
            }
            else
            if (a is float)
            {
                float aa = (float)a;
                if (b is byte)
                {
                    return aa - (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa - (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa - (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa - (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa - (int)b;
                }
                else
                if (b is uint)
                {
                    return aa - (uint)b;
                }
                else
                if (b is long)
                {
                    return aa - (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa - (ulong)b;
                }
                else if (b is char)
                {
                    return aa - (char)b;
                }
                else if (b is float)
                {
                    return aa - (float)b;
                }
                else if (b is double)
                {
                    return aa - (double)b;
                }
            }
            else
            if (a is double)
            {
                double aa = (double)a;
                if (b is byte)
                {
                    return aa - (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa - (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa - (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa - (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa - (int)b;
                }
                else
                if (b is uint)
                {
                    return aa - (uint)b;
                }
                else
                if (b is long)
                {
                    return aa - (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa - (ulong)b;
                }
                else if (b is char)
                {
                    return aa - (char)b;
                }
                else if (b is float)
                {
                    return aa - (float)b;
                }
                else if (b is double)
                {
                    return aa - (double)b;
                }
            }
            throw new Exception();
        }

        public static object Mul(object a, object b)
        {
            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa * (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa * (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa * (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa * (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa * (int)b;
                }
                else
                if (b is uint)
                {
                    return aa * (uint)b;
                }
                else
                if (b is long)
                {
                    return aa * (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa * (ulong)b;
                }
                else if (b is char)
                {
                    return aa * (char)b;
                }
                else if (b is float)
                {
                    return aa * (float)b;
                }
                else if (b is double)
                {
                    return aa * (double)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa * (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa * (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa * (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa * (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa * (int)b;
                }
                else
                if (b is uint)
                {
                    return aa * (uint)b;
                }
                else
                if (b is long)
                {
                    return aa * (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa * (ulong)b;
                }
                else if (b is char)
                {
                    return aa * (char)b;
                }
                else if (b is float)
                {
                    return aa * (float)b;
                }
                else if (b is double)
                {
                    return aa * (double)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa * (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa * (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa * (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa * (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa * (int)b;
                }
                else
                if (b is uint)
                {
                    return aa * (uint)b;
                }
                else
                if (b is long)
                {
                    return aa * (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa * (ulong)b;
                }
                else if (b is char)
                {
                    return aa * (char)b;
                }
                else if (b is float)
                {
                    return aa * (float)b;
                }
                else if (b is double)
                {
                    return aa * (double)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa * (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa * (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa * (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa * (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa * (int)b;
                }
                else
                if (b is uint)
                {
                    return aa * (uint)b;
                }
                else
                if (b is long)
                {
                    return aa * (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa * (ulong)b;
                }
                else if (b is char)
                {
                    return aa * (char)b;
                }
                else if (b is float)
                {
                    return aa * (float)b;
                }
                else if (b is double)
                {
                    return aa * (double)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa * (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa * (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa * (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa * (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa * (int)b;
                }
                else
                if (b is uint)
                {
                    return aa * (uint)b;
                }
                else
                if (b is long)
                {
                    return aa * (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa * (ulong)b;
                }
                else if (b is char)
                {
                    return aa * (char)b;
                }
                else if (b is float)
                {
                    return aa * (float)b;
                }
                else if (b is double)
                {
                    return aa * (double)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa * (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa * (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa * (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa * (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa * (int)b;
                }
                else
                if (b is uint)
                {
                    return aa * (uint)b;
                }
                else
                if (b is long)
                {
                    return aa * (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa * (ulong)b;
                }
                else if (b is char)
                {
                    return aa * (char)b;
                }
                else if (b is float)
                {
                    return aa * (float)b;
                }
                else if (b is double)
                {
                    return aa * (double)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa * (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa * (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa * (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa * (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa * (int)b;
                }
                else
                if (b is uint)
                {
                    return aa * (uint)b;
                }
                else
                if (b is long)
                {
                    return aa * (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa * (ulong)b;
                }
                else if (b is char)
                {
                    return aa * (char)b;
                }
                else if (b is float)
                {
                    return aa * (float)b;
                }
                else if (b is double)
                {
                    return aa * (double)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa * (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa * (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa * (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa * (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa * (int)b;
                }
                else
                if (b is uint)
                {
                    return aa * (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa * (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa * (ulong)b;
                }
                else if (b is char)
                {
                    return aa * (char)b;
                }
                else if (b is float)
                {
                    return aa * (float)b;
                }
                else if (b is double)
                {
                    return aa * (double)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa * (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa * (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa * (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa * (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa * (int)b;
                }
                else
                if (b is uint)
                {
                    return aa * (uint)b;
                }
                else
                if (b is long)
                {
                    return aa * (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa * (ulong)b;
                }
                else if (b is char)
                {
                    return aa * (char)b;
                }
                else if (b is float)
                {
                    return aa * (float)b;
                }
                else if (b is double)
                {
                    return aa * (double)b;
                }
            }
            else
            if (a is float)
            {
                float aa = (float)a;
                if (b is byte)
                {
                    return aa * (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa * (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa * (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa * (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa * (int)b;
                }
                else
                if (b is uint)
                {
                    return aa * (uint)b;
                }
                else
                if (b is long)
                {
                    return aa * (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa * (ulong)b;
                }
                else if (b is char)
                {
                    return aa * (char)b;
                }
                else if (b is float)
                {
                    return aa * (float)b;
                }
                else if (b is double)
                {
                    return aa * (double)b;
                }
            }
            else
            if (a is double)
            {
                double aa = (double)a;
                if (b is byte)
                {
                    return aa * (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa * (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa * (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa * (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa * (int)b;
                }
                else
                if (b is uint)
                {
                    return aa * (uint)b;
                }
                else
                if (b is long)
                {
                    return aa * (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa * (ulong)b;
                }
                else if (b is char)
                {
                    return aa * (char)b;
                }
                else if (b is float)
                {
                    return aa * (float)b;
                }
                else if (b is double)
                {
                    return aa * (double)b;
                }
            }
            throw new Exception();
        }

        public static object Div(object a, object b)
        {
            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa / (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa / (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa / (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa / (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa / (int)b;
                }
                else
                if (b is uint)
                {
                    return aa / (uint)b;
                }
                else
                if (b is long)
                {
                    return aa / (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa / (ulong)b;
                }
                else if (b is char)
                {
                    return aa / (char)b;
                }
                else if (b is float)
                {
                    return aa / (float)b;
                }
                else if (b is double)
                {
                    return aa / (double)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa / (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa / (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa / (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa / (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa / (int)b;
                }
                else
                if (b is uint)
                {
                    return aa / (uint)b;
                }
                else
                if (b is long)
                {
                    return aa / (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa / (ulong)b;
                }
                else if (b is char)
                {
                    return aa / (char)b;
                }
                else if (b is float)
                {
                    return aa / (float)b;
                }
                else if (b is double)
                {
                    return aa / (double)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa / (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa / (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa / (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa / (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa / (int)b;
                }
                else
                if (b is uint)
                {
                    return aa / (uint)b;
                }
                else
                if (b is long)
                {
                    return aa / (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa / (ulong)b;
                }
                else if (b is char)
                {
                    return aa / (char)b;
                }
                else if (b is float)
                {
                    return aa / (float)b;
                }
                else if (b is double)
                {
                    return aa / (double)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa / (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa / (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa / (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa / (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa / (int)b;
                }
                else
                if (b is uint)
                {
                    return aa / (uint)b;
                }
                else
                if (b is long)
                {
                    return aa / (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa / (ulong)b;
                }
                else if (b is char)
                {
                    return aa / (char)b;
                }
                else if (b is float)
                {
                    return aa / (float)b;
                }
                else if (b is double)
                {
                    return aa / (double)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa / (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa / (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa / (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa / (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa / (int)b;
                }
                else
                if (b is uint)
                {
                    return aa / (uint)b;
                }
                else
                if (b is long)
                {
                    return aa / (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa / (ulong)b;
                }
                else if (b is char)
                {
                    return aa / (char)b;
                }
                else if (b is float)
                {
                    return aa / (float)b;
                }
                else if (b is double)
                {
                    return aa / (double)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa / (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa / (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa / (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa / (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa / (int)b;
                }
                else
                if (b is uint)
                {
                    return aa / (uint)b;
                }
                else
                if (b is long)
                {
                    return aa / (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa / (ulong)b;
                }
                else if (b is char)
                {
                    return aa / (char)b;
                }
                else if (b is float)
                {
                    return aa / (float)b;
                }
                else if (b is double)
                {
                    return aa / (double)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa / (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa / (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa / (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa / (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa / (int)b;
                }
                else
                if (b is uint)
                {
                    return aa / (uint)b;
                }
                else
                if (b is long)
                {
                    return aa / (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa / (ulong)b;
                }
                else if (b is char)
                {
                    return aa / (char)b;
                }
                else if (b is float)
                {
                    return aa / (float)b;
                }
                else if (b is double)
                {
                    return aa / (double)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa / (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa / (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa / (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa / (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa / (int)b;
                }
                else
                if (b is uint)
                {
                    return aa / (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa / (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa / (ulong)b;
                }
                else if (b is char)
                {
                    return aa / (char)b;
                }
                else if (b is float)
                {
                    return aa / (float)b;
                }
                else if (b is double)
                {
                    return aa / (double)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa / (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa / (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa / (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa / (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa / (int)b;
                }
                else
                if (b is uint)
                {
                    return aa / (uint)b;
                }
                else
                if (b is long)
                {
                    return aa / (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa / (ulong)b;
                }
                else if (b is char)
                {
                    return aa / (char)b;
                }
                else if (b is float)
                {
                    return aa / (float)b;
                }
                else if (b is double)
                {
                    return aa / (double)b;
                }
            }
            else
            if (a is float)
            {
                float aa = (float)a;
                if (b is byte)
                {
                    return aa / (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa / (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa / (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa / (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa / (int)b;
                }
                else
                if (b is uint)
                {
                    return aa / (uint)b;
                }
                else
                if (b is long)
                {
                    return aa / (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa / (ulong)b;
                }
                else if (b is char)
                {
                    return aa / (char)b;
                }
                else if (b is float)
                {
                    return aa / (float)b;
                }
                else if (b is double)
                {
                    return aa / (double)b;
                }
            }
            else
            if (a is double)
            {
                double aa = (double)a;
                if (b is byte)
                {
                    return aa / (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa / (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa / (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa / (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa / (int)b;
                }
                else
                if (b is uint)
                {
                    return aa / (uint)b;
                }
                else
                if (b is long)
                {
                    return aa / (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa / (ulong)b;
                }
                else if (b is char)
                {
                    return aa / (char)b;
                }
                else if (b is float)
                {
                    return aa / (float)b;
                }
                else if (b is double)
                {
                    return aa / (double)b;
                }
            }
            throw new Exception();
        }

        public static object Mod(object a, object b)
        {
            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa % (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa % (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa % (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa % (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa % (int)b;
                }
                else
                if (b is uint)
                {
                    return aa % (uint)b;
                }
                else
                if (b is long)
                {
                    return aa % (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa % (ulong)b;
                }
                else if (b is char)
                {
                    return aa % (char)b;
                }
                else if (b is float)
                {
                    return aa % (float)b;
                }
                else if (b is double)
                {
                    return aa % (double)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa % (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa % (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa % (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa % (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa % (int)b;
                }
                else
                if (b is uint)
                {
                    return aa % (uint)b;
                }
                else
                if (b is long)
                {
                    return aa % (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa % (ulong)b;
                }
                else if (b is char)
                {
                    return aa % (char)b;
                }
                else if (b is float)
                {
                    return aa % (float)b;
                }
                else if (b is double)
                {
                    return aa % (double)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa % (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa % (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa % (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa % (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa % (int)b;
                }
                else
                if (b is uint)
                {
                    return aa % (uint)b;
                }
                else
                if (b is long)
                {
                    return aa % (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa % (ulong)b;
                }
                else if (b is char)
                {
                    return aa % (char)b;
                }
                else if (b is float)
                {
                    return aa % (float)b;
                }
                else if (b is double)
                {
                    return aa % (double)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa % (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa % (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa % (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa % (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa % (int)b;
                }
                else
                if (b is uint)
                {
                    return aa % (uint)b;
                }
                else
                if (b is long)
                {
                    return aa % (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa % (ulong)b;
                }
                else if (b is char)
                {
                    return aa % (char)b;
                }
                else if (b is float)
                {
                    return aa % (float)b;
                }
                else if (b is double)
                {
                    return aa % (double)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa % (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa % (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa % (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa % (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa % (int)b;
                }
                else
                if (b is uint)
                {
                    return aa % (uint)b;
                }
                else
                if (b is long)
                {
                    return aa % (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa % (ulong)b;
                }
                else if (b is char)
                {
                    return aa % (char)b;
                }
                else if (b is float)
                {
                    return aa % (float)b;
                }
                else if (b is double)
                {
                    return aa % (double)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa % (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa % (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa % (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa % (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa % (int)b;
                }
                else
                if (b is uint)
                {
                    return aa % (uint)b;
                }
                else
                if (b is long)
                {
                    return aa % (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa % (ulong)b;
                }
                else if (b is char)
                {
                    return aa % (char)b;
                }
                else if (b is float)
                {
                    return aa % (float)b;
                }
                else if (b is double)
                {
                    return aa % (double)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa % (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa % (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa % (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa % (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa % (int)b;
                }
                else
                if (b is uint)
                {
                    return aa % (uint)b;
                }
                else
                if (b is long)
                {
                    return aa % (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa % (ulong)b;
                }
                else if (b is char)
                {
                    return aa % (char)b;
                }
                else if (b is float)
                {
                    return aa % (float)b;
                }
                else if (b is double)
                {
                    return aa % (double)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa % (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa % (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa % (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa % (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa % (int)b;
                }
                else
                if (b is uint)
                {
                    return aa % (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa % (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa % (ulong)b;
                }
                else if (b is char)
                {
                    return aa % (char)b;
                }
                else if (b is float)
                {
                    return aa % (float)b;
                }
                else if (b is double)
                {
                    return aa % (double)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa % (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa % (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa % (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa % (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa % (int)b;
                }
                else
                if (b is uint)
                {
                    return aa % (uint)b;
                }
                else
                if (b is long)
                {
                    return aa % (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa % (ulong)b;
                }
                else if (b is char)
                {
                    return aa % (char)b;
                }
                else if (b is float)
                {
                    return aa % (float)b;
                }
                else if (b is double)
                {
                    return aa % (double)b;
                }
            }
            else
            if (a is float)
            {
                float aa = (float)a;
                if (b is byte)
                {
                    return aa % (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa % (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa % (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa % (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa % (int)b;
                }
                else
                if (b is uint)
                {
                    return aa % (uint)b;
                }
                else
                if (b is long)
                {
                    return aa % (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa % (ulong)b;
                }
                else if (b is char)
                {
                    return aa % (char)b;
                }
                else if (b is float)
                {
                    return aa % (float)b;
                }
                else if (b is double)
                {
                    return aa % (double)b;
                }
            }
            else
            if (a is double)
            {
                double aa = (double)a;
                if (b is byte)
                {
                    return aa % (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa % (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa % (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa % (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa % (int)b;
                }
                else
                if (b is uint)
                {
                    return aa % (uint)b;
                }
                else
                if (b is long)
                {
                    return aa % (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa % (ulong)b;
                }
                else if (b is char)
                {
                    return aa % (char)b;
                }
                else if (b is float)
                {
                    return aa % (float)b;
                }
                else if (b is double)
                {
                    return aa % (double)b;
                }
            }
            throw new Exception();
        }

        public static object BitAnd(object a, object b)
        {
            if (a is bool && b is bool)
                return (bool)a && (bool)b;

            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa & (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa & (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa & (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa & (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa & (int)b;
                }
                else
                if (b is uint)
                {
                    return aa & (uint)b;
                }
                else
                if (b is long)
                {
                    return aa & (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa & (ulong)b;
                }
                else if (b is char)
                {
                    return aa & (char)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa & (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa & (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa & (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa & (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa & (int)b;
                }
                else
                if (b is uint)
                {
                    return aa & (uint)b;
                }
                else
                if (b is long)
                {
                    return aa & (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa & (ulong)b;
                }
                else if (b is char)
                {
                    return aa & (char)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa & (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa & (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa & (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa & (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa & (int)b;
                }
                else
                if (b is uint)
                {
                    return aa & (uint)b;
                }
                else
                if (b is long)
                {
                    return aa & (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa & (ulong)b;
                }
                else if (b is char)
                {
                    return aa & (char)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa & (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa & (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa & (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa & (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa & (int)b;
                }
                else
                if (b is uint)
                {
                    return aa & (uint)b;
                }
                else
                if (b is long)
                {
                    return aa & (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa & (ulong)b;
                }
                else if (b is char)
                {
                    return aa & (char)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa & (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa & (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa & (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa & (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa & (int)b;
                }
                else
                if (b is uint)
                {
                    return aa & (uint)b;
                }
                else
                if (b is long)
                {
                    return aa & (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa & (ulong)b;
                }
                else if (b is char)
                {
                    return aa & (char)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa & (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa & (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa & (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa & (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa & (int)b;
                }
                else
                if (b is uint)
                {
                    return aa & (uint)b;
                }
                else
                if (b is long)
                {
                    return aa & (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa & (ulong)b;
                }
                else if (b is char)
                {
                    return aa & (char)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa & (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa & (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa & (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa & (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa & (int)b;
                }
                else
                if (b is uint)
                {
                    return aa & (uint)b;
                }
                else
                if (b is long)
                {
                    return aa & (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa & (ulong)b;
                }
                else if (b is char)
                {
                    return aa & (char)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa & (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa & (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa & (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa & (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa & (int)b;
                }
                else
                if (b is uint)
                {
                    return aa & (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa & (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa & (ulong)b;
                }
                else if (b is char)
                {
                    return aa & (char)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa & (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa & (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa & (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa & (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa & (int)b;
                }
                else
                if (b is uint)
                {
                    return aa & (uint)b;
                }
                else
                if (b is long)
                {
                    return aa & (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa & (ulong)b;
                }
                else if (b is char)
                {
                    return aa & (char)b;
                }
            }
            throw new Exception();
        }

        public static object BitOr(object a, object b)
        {
            if (a is bool && b is bool)
                return (bool)a || (bool)b;

            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa | (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa | (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa | (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa | (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa | (int)b;
                }
                else
                if (b is uint)
                {
                    return aa | (uint)b;
                }
                else
                if (b is long)
                {
                    return aa | (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa | (ulong)b;
                }
                else if (b is char)
                {
                    return aa | (char)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa | (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa | (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa | (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa | (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa | (int)b;
                }
                else
                if (b is uint)
                {
                    return aa | (uint)b;
                }
                else
                if (b is long)
                {
                    return aa | (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa | (ulong)b;
                }
                else if (b is char)
                {
                    return aa | (char)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa | (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa | (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa | (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa | (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa | (int)b;
                }
                else
                if (b is uint)
                {
                    return aa | (uint)b;
                }
                else
                if (b is long)
                {
                    return aa | (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa | (ulong)b;
                }
                else if (b is char)
                {
                    return aa | (char)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa | (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa | (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa | (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa | (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa | (int)b;
                }
                else
                if (b is uint)
                {
                    return aa | (uint)b;
                }
                else
                if (b is long)
                {
                    return aa | (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa | (ulong)b;
                }
                else if (b is char)
                {
                    return aa | (char)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa | (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa | (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa | (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa | (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa | (int)b;
                }
                else
                if (b is uint)
                {
                    return aa | (uint)b;
                }
                else
                if (b is long)
                {
                    return aa | (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa | (ulong)b;
                }
                else if (b is char)
                {
                    return aa | (char)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa | (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa | (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa | (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa | (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa | (int)b;
                }
                else
                if (b is uint)
                {
                    return aa | (uint)b;
                }
                else
                if (b is long)
                {
                    return aa | (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa | (ulong)b;
                }
                else if (b is char)
                {
                    return aa | (char)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa | (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa | (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa | (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa | (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa | (int)b;
                }
                else
                if (b is uint)
                {
                    return aa | (uint)b;
                }
                else
                if (b is long)
                {
                    return aa | (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa | (ulong)b;
                }
                else if (b is char)
                {
                    return aa | (char)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa | (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa | (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa | (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa | (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa | (int)b;
                }
                else
                if (b is uint)
                {
                    return aa | (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa | (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa | (ulong)b;
                }
                else if (b is char)
                {
                    return aa | (char)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa | (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa | (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa | (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa | (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa | (int)b;
                }
                else
                if (b is uint)
                {
                    return aa | (uint)b;
                }
                else
                if (b is long)
                {
                    return aa | (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa | (ulong)b;
                }
                else if (b is char)
                {
                    return aa | (char)b;
                }
            }
            throw new Exception();
        }

        public static object BitXor(object a, object b)
        {
            if (a is bool && b is bool)
                return (bool)a ^ (bool)b;

            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa ^ (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa ^ (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa ^ (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa ^ (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa ^ (int)b;
                }
                else
                if (b is uint)
                {
                    return aa ^ (uint)b;
                }
                else
                if (b is long)
                {
                    return aa ^ (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa ^ (ulong)b;
                }
                else if (b is char)
                {
                    return aa ^ (char)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa ^ (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa ^ (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa ^ (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa ^ (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa ^ (int)b;
                }
                else
                if (b is uint)
                {
                    return aa ^ (uint)b;
                }
                else
                if (b is long)
                {
                    return aa ^ (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa ^ (ulong)b;
                }
                else if (b is char)
                {
                    return aa ^ (char)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa ^ (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa ^ (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa ^ (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa ^ (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa ^ (int)b;
                }
                else
                if (b is uint)
                {
                    return aa ^ (uint)b;
                }
                else
                if (b is long)
                {
                    return aa ^ (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa ^ (ulong)b;
                }
                else if (b is char)
                {
                    return aa ^ (char)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa ^ (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa ^ (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa ^ (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa ^ (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa ^ (int)b;
                }
                else
                if (b is uint)
                {
                    return aa ^ (uint)b;
                }
                else
                if (b is long)
                {
                    return aa ^ (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa ^ (ulong)b;
                }
                else if (b is char)
                {
                    return aa ^ (char)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa ^ (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa ^ (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa ^ (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa ^ (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa ^ (int)b;
                }
                else
                if (b is uint)
                {
                    return aa ^ (uint)b;
                }
                else
                if (b is long)
                {
                    return aa ^ (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa ^ (ulong)b;
                }
                else if (b is char)
                {
                    return aa ^ (char)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa ^ (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa ^ (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa ^ (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa ^ (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa ^ (int)b;
                }
                else
                if (b is uint)
                {
                    return aa ^ (uint)b;
                }
                else
                if (b is long)
                {
                    return aa ^ (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa ^ (ulong)b;
                }
                else if (b is char)
                {
                    return aa ^ (char)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa ^ (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa ^ (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa ^ (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa ^ (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa ^ (int)b;
                }
                else
                if (b is uint)
                {
                    return aa ^ (uint)b;
                }
                else
                if (b is long)
                {
                    return aa ^ (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa ^ (ulong)b;
                }
                else if (b is char)
                {
                    return aa ^ (char)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa ^ (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa ^ (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa ^ (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa ^ (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa ^ (int)b;
                }
                else
                if (b is uint)
                {
                    return aa ^ (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa ^ (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa ^ (ulong)b;
                }
                else if (b is char)
                {
                    return aa ^ (char)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa ^ (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa ^ (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa ^ (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa ^ (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa ^ (int)b;
                }
                else
                if (b is uint)
                {
                    return aa ^ (uint)b;
                }
                else
                if (b is long)
                {
                    return aa ^ (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa ^ (ulong)b;
                }
                else if (b is char)
                {
                    return aa ^ (char)b;
                }
            }
            throw new Exception();
        }

        public static bool Gt(object a, object b)
        {
            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa > (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa > (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa > (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa > (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa > (int)b;
                }
                else
                if (b is uint)
                {
                    return aa > (uint)b;
                }
                else
                if (b is long)
                {
                    return aa > (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa > (ulong)b;
                }
                else if (b is char)
                {
                    return aa > (char)b;
                }
                else if (b is float)
                {
                    return aa > (float)b;
                }
                else if (b is double)
                {
                    return aa > (double)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa > (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa > (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa > (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa > (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa > (int)b;
                }
                else
                if (b is uint)
                {
                    return aa > (uint)b;
                }
                else
                if (b is long)
                {
                    return aa > (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa > (ulong)b;
                }
                else if (b is char)
                {
                    return aa > (char)b;
                }
                else if (b is float)
                {
                    return aa > (float)b;
                }
                else if (b is double)
                {
                    return aa > (double)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa > (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa > (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa > (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa > (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa > (int)b;
                }
                else
                if (b is uint)
                {
                    return aa > (uint)b;
                }
                else
                if (b is long)
                {
                    return aa > (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa > (ulong)b;
                }
                else if (b is char)
                {
                    return aa > (char)b;
                }
                else if (b is float)
                {
                    return aa > (float)b;
                }
                else if (b is double)
                {
                    return aa > (double)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa > (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa > (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa > (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa > (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa > (int)b;
                }
                else
                if (b is uint)
                {
                    return aa > (uint)b;
                }
                else
                if (b is long)
                {
                    return aa > (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa > (ulong)b;
                }
                else if (b is char)
                {
                    return aa > (char)b;
                }
                else if (b is float)
                {
                    return aa > (float)b;
                }
                else if (b is double)
                {
                    return aa > (double)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa > (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa > (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa > (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa > (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa > (int)b;
                }
                else
                if (b is uint)
                {
                    return aa > (uint)b;
                }
                else
                if (b is long)
                {
                    return aa > (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa > (ulong)b;
                }
                else if (b is char)
                {
                    return aa > (char)b;
                }
                else if (b is float)
                {
                    return aa > (float)b;
                }
                else if (b is double)
                {
                    return aa > (double)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa > (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa > (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa > (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa > (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa > (int)b;
                }
                else
                if (b is uint)
                {
                    return aa > (uint)b;
                }
                else
                if (b is long)
                {
                    return aa > (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa > (ulong)b;
                }
                else if (b is char)
                {
                    return aa > (char)b;
                }
                else if (b is float)
                {
                    return aa > (float)b;
                }
                else if (b is double)
                {
                    return aa > (double)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa > (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa > (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa > (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa > (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa > (int)b;
                }
                else
                if (b is uint)
                {
                    return aa > (uint)b;
                }
                else
                if (b is long)
                {
                    return aa > (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa > (ulong)b;
                }
                else if (b is char)
                {
                    return aa > (char)b;
                }
                else if (b is float)
                {
                    return aa > (float)b;
                }
                else if (b is double)
                {
                    return aa > (double)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa > (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa > (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa > (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa > (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa > (int)b;
                }
                else
                if (b is uint)
                {
                    return aa > (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa > (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa > (ulong)b;
                }
                else if (b is char)
                {
                    return aa > (char)b;
                }
                else if (b is float)
                {
                    return aa > (float)b;
                }
                else if (b is double)
                {
                    return aa > (double)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa > (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa > (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa > (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa > (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa > (int)b;
                }
                else
                if (b is uint)
                {
                    return aa > (uint)b;
                }
                else
                if (b is long)
                {
                    return aa > (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa > (ulong)b;
                }
                else if (b is char)
                {
                    return aa > (char)b;
                }
                else if (b is float)
                {
                    return aa > (float)b;
                }
                else if (b is double)
                {
                    return aa > (double)b;
                }
            }
            else
            if (a is float)
            {
                float aa = (float)a;
                if (b is byte)
                {
                    return aa > (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa > (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa > (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa > (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa > (int)b;
                }
                else
                if (b is uint)
                {
                    return aa > (uint)b;
                }
                else
                if (b is long)
                {
                    return aa > (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa > (ulong)b;
                }
                else if (b is char)
                {
                    return aa > (char)b;
                }
                else if (b is float)
                {
                    return aa > (float)b;
                }
                else if (b is double)
                {
                    return aa > (double)b;
                }
            }
            else
            if (a is double)
            {
                double aa = (double)a;
                if (b is byte)
                {
                    return aa > (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa > (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa > (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa > (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa > (int)b;
                }
                else
                if (b is uint)
                {
                    return aa > (uint)b;
                }
                else
                if (b is long)
                {
                    return aa > (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa > (ulong)b;
                }
                else if (b is char)
                {
                    return aa > (char)b;
                }
                else if (b is float)
                {
                    return aa > (float)b;
                }
                else if (b is double)
                {
                    return aa > (double)b;
                }
            }
            throw new Exception();
        }

        public static bool Lt(object a, object b)
        {
            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa < (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa < (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa < (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa < (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa < (int)b;
                }
                else
                if (b is uint)
                {
                    return aa < (uint)b;
                }
                else
                if (b is long)
                {
                    return aa < (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa < (ulong)b;
                }
                else if (b is char)
                {
                    return aa < (char)b;
                }
                else if (b is float)
                {
                    return aa < (float)b;
                }
                else if (b is double)
                {
                    return aa < (double)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa < (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa < (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa < (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa < (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa < (int)b;
                }
                else
                if (b is uint)
                {
                    return aa < (uint)b;
                }
                else
                if (b is long)
                {
                    return aa < (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa < (ulong)b;
                }
                else if (b is char)
                {
                    return aa < (char)b;
                }
                else if (b is float)
                {
                    return aa < (float)b;
                }
                else if (b is double)
                {
                    return aa < (double)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa < (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa < (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa < (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa < (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa < (int)b;
                }
                else
                if (b is uint)
                {
                    return aa < (uint)b;
                }
                else
                if (b is long)
                {
                    return aa < (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa < (ulong)b;
                }
                else if (b is char)
                {
                    return aa < (char)b;
                }
                else if (b is float)
                {
                    return aa < (float)b;
                }
                else if (b is double)
                {
                    return aa < (double)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa < (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa < (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa < (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa < (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa < (int)b;
                }
                else
                if (b is uint)
                {
                    return aa < (uint)b;
                }
                else
                if (b is long)
                {
                    return aa < (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa < (ulong)b;
                }
                else if (b is char)
                {
                    return aa < (char)b;
                }
                else if (b is float)
                {
                    return aa < (float)b;
                }
                else if (b is double)
                {
                    return aa < (double)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa < (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa < (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa < (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa < (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa < (int)b;
                }
                else
                if (b is uint)
                {
                    return aa < (uint)b;
                }
                else
                if (b is long)
                {
                    return aa < (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa < (ulong)b;
                }
                else if (b is char)
                {
                    return aa < (char)b;
                }
                else if (b is float)
                {
                    return aa < (float)b;
                }
                else if (b is double)
                {
                    return aa < (double)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa < (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa < (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa < (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa < (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa < (int)b;
                }
                else
                if (b is uint)
                {
                    return aa < (uint)b;
                }
                else
                if (b is long)
                {
                    return aa < (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa < (ulong)b;
                }
                else if (b is char)
                {
                    return aa < (char)b;
                }
                else if (b is float)
                {
                    return aa < (float)b;
                }
                else if (b is double)
                {
                    return aa < (double)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa < (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa < (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa < (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa < (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa < (int)b;
                }
                else
                if (b is uint)
                {
                    return aa < (uint)b;
                }
                else
                if (b is long)
                {
                    return aa < (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa < (ulong)b;
                }
                else if (b is char)
                {
                    return aa < (char)b;
                }
                else if (b is float)
                {
                    return aa < (float)b;
                }
                else if (b is double)
                {
                    return aa < (double)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa < (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa < (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa < (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa < (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa < (int)b;
                }
                else
                if (b is uint)
                {
                    return aa < (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa < (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa < (ulong)b;
                }
                else if (b is char)
                {
                    return aa < (char)b;
                }
                else if (b is float)
                {
                    return aa < (float)b;
                }
                else if (b is double)
                {
                    return aa < (double)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa < (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa < (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa < (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa < (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa < (int)b;
                }
                else
                if (b is uint)
                {
                    return aa < (uint)b;
                }
                else
                if (b is long)
                {
                    return aa < (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa < (ulong)b;
                }
                else if (b is char)
                {
                    return aa < (char)b;
                }
                else if (b is float)
                {
                    return aa < (float)b;
                }
                else if (b is double)
                {
                    return aa < (double)b;
                }
            }
            else
            if (a is float)
            {
                float aa = (float)a;
                if (b is byte)
                {
                    return aa < (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa < (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa < (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa < (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa < (int)b;
                }
                else
                if (b is uint)
                {
                    return aa < (uint)b;
                }
                else
                if (b is long)
                {
                    return aa < (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa < (ulong)b;
                }
                else if (b is char)
                {
                    return aa < (char)b;
                }
                else if (b is float)
                {
                    return aa < (float)b;
                }
                else if (b is double)
                {
                    return aa < (double)b;
                }
            }
            else
            if (a is double)
            {
                double aa = (double)a;
                if (b is byte)
                {
                    return aa < (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa < (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa < (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa < (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa < (int)b;
                }
                else
                if (b is uint)
                {
                    return aa < (uint)b;
                }
                else
                if (b is long)
                {
                    return aa < (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa < (ulong)b;
                }
                else if (b is char)
                {
                    return aa < (char)b;
                }
                else if (b is float)
                {
                    return aa < (float)b;
                }
                else if (b is double)
                {
                    return aa < (double)b;
                }
            }
            throw new Exception();
        }

        public static bool Gte(object a, object b)
        {
            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa >= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa >= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa >= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa >= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa >= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa >= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa >= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa >= (ulong)b;
                }
                else if (b is char)
                {
                    return aa >= (char)b;
                }
                else if (b is float)
                {
                    return aa >= (float)b;
                }
                else if (b is double)
                {
                    return aa >= (double)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa >= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa >= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa >= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa >= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa >= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa >= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa >= (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa >= (ulong)b;
                }
                else if (b is char)
                {
                    return aa >= (char)b;
                }
                else if (b is float)
                {
                    return aa >= (float)b;
                }
                else if (b is double)
                {
                    return aa >= (double)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa >= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa >= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa >= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa >= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa >= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa >= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa >= (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa >= (ulong)b;
                }
                else if (b is char)
                {
                    return aa >= (char)b;
                }
                else if (b is float)
                {
                    return aa >= (float)b;
                }
                else if (b is double)
                {
                    return aa >= (double)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa >= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa >= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa >= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa >= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa >= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa >= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa >= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa >= (ulong)b;
                }
                else if (b is char)
                {
                    return aa >= (char)b;
                }
                else if (b is float)
                {
                    return aa >= (float)b;
                }
                else if (b is double)
                {
                    return aa >= (double)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa >= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa >= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa >= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa >= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa >= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa >= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa >= (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa >= (ulong)b;
                }
                else if (b is char)
                {
                    return aa >= (char)b;
                }
                else if (b is float)
                {
                    return aa >= (float)b;
                }
                else if (b is double)
                {
                    return aa >= (double)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa >= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa >= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa >= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa >= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa >= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa >= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa >= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa >= (ulong)b;
                }
                else if (b is char)
                {
                    return aa >= (char)b;
                }
                else if (b is float)
                {
                    return aa >= (float)b;
                }
                else if (b is double)
                {
                    return aa >= (double)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa >= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa >= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa >= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa >= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa >= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa >= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa >= (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa >= (ulong)b;
                }
                else if (b is char)
                {
                    return aa >= (char)b;
                }
                else if (b is float)
                {
                    return aa >= (float)b;
                }
                else if (b is double)
                {
                    return aa >= (double)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa >= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa >= (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa >= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa >= (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa >= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa >= (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa >= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa >= (ulong)b;
                }
                else if (b is char)
                {
                    return aa >= (char)b;
                }
                else if (b is float)
                {
                    return aa >= (float)b;
                }
                else if (b is double)
                {
                    return aa >= (double)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa >= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa >= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa >= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa >= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa >= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa >= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa >= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa >= (ulong)b;
                }
                else if (b is char)
                {
                    return aa >= (char)b;
                }
                else if (b is float)
                {
                    return aa >= (float)b;
                }
                else if (b is double)
                {
                    return aa >= (double)b;
                }
            }
            else
            if (a is float)
            {
                float aa = (float)a;
                if (b is byte)
                {
                    return aa >= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa >= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa >= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa >= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa >= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa >= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa >= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa >= (ulong)b;
                }
                else if (b is char)
                {
                    return aa >= (char)b;
                }
                else if (b is float)
                {
                    return aa >= (float)b;
                }
                else if (b is double)
                {
                    return aa >= (double)b;
                }
            }
            else
            if (a is double)
            {
                double aa = (double)a;
                if (b is byte)
                {
                    return aa >= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa >= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa >= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa >= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa >= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa >= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa >= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa >= (ulong)b;
                }
                else if (b is char)
                {
                    return aa >= (char)b;
                }
                else if (b is float)
                {
                    return aa >= (float)b;
                }
                else if (b is double)
                {
                    return aa >= (double)b;
                }
            }
            throw new Exception();
        }

        public static bool Lte(object a, object b)
        {
            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa <= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa <= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa <= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa <= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa <= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa <= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa <= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa <= (ulong)b;
                }
                else if (b is char)
                {
                    return aa <= (char)b;
                }
                else if (b is float)
                {
                    return aa <= (float)b;
                }
                else if (b is double)
                {
                    return aa <= (double)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa <= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa <= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa <= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa <= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa <= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa <= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa <= (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa <= (ulong)b;
                }
                else if (b is char)
                {
                    return aa <= (char)b;
                }
                else if (b is float)
                {
                    return aa <= (float)b;
                }
                else if (b is double)
                {
                    return aa <= (double)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa <= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa <= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa <= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa <= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa <= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa <= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa <= (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa <= (ulong)b;
                }
                else if (b is char)
                {
                    return aa <= (char)b;
                }
                else if (b is float)
                {
                    return aa <= (float)b;
                }
                else if (b is double)
                {
                    return aa <= (double)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa <= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa <= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa <= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa <= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa <= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa <= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa <= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa <= (ulong)b;
                }
                else if (b is char)
                {
                    return aa <= (char)b;
                }
                else if (b is float)
                {
                    return aa <= (float)b;
                }
                else if (b is double)
                {
                    return aa <= (double)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa <= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa <= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa <= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa <= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa <= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa <= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa <= (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa <= (ulong)b;
                }
                else if (b is char)
                {
                    return aa <= (char)b;
                }
                else if (b is float)
                {
                    return aa <= (float)b;
                }
                else if (b is double)
                {
                    return aa <= (double)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa <= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa <= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa <= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa <= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa <= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa <= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa <= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa <= (ulong)b;
                }
                else if (b is char)
                {
                    return aa <= (char)b;
                }
                else if (b is float)
                {
                    return aa <= (float)b;
                }
                else if (b is double)
                {
                    return aa <= (double)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa <= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa <= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa <= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa <= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa <= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa <= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa <= (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa <= (ulong)b;
                }
                else if (b is char)
                {
                    return aa <= (char)b;
                }
                else if (b is float)
                {
                    return aa <= (float)b;
                }
                else if (b is double)
                {
                    return aa <= (double)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa <= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa <= (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa <= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa <= (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa <= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa <= (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa <= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa <= (ulong)b;
                }
                else if (b is char)
                {
                    return aa <= (char)b;
                }
                else if (b is float)
                {
                    return aa <= (float)b;
                }
                else if (b is double)
                {
                    return aa <= (double)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa <= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa <= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa <= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa <= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa <= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa <= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa <= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa <= (ulong)b;
                }
                else if (b is char)
                {
                    return aa <= (char)b;
                }
                else if (b is float)
                {
                    return aa <= (float)b;
                }
                else if (b is double)
                {
                    return aa <= (double)b;
                }
            }
            else
            if (a is float)
            {
                float aa = (float)a;
                if (b is byte)
                {
                    return aa <= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa <= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa <= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa <= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa <= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa <= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa <= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa <= (ulong)b;
                }
                else if (b is char)
                {
                    return aa <= (char)b;
                }
                else if (b is float)
                {
                    return aa <= (float)b;
                }
                else if (b is double)
                {
                    return aa <= (double)b;
                }
            }
            else
            if (a is double)
            {
                double aa = (double)a;
                if (b is byte)
                {
                    return aa <= (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa <= (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa <= (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa <= (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa <= (int)b;
                }
                else
                if (b is uint)
                {
                    return aa <= (uint)b;
                }
                else
                if (b is long)
                {
                    return aa <= (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa <= (ulong)b;
                }
                else if (b is char)
                {
                    return aa <= (char)b;
                }
                else if (b is float)
                {
                    return aa <= (float)b;
                }
                else if (b is double)
                {
                    return aa <= (double)b;
                }
            }
            throw new Exception();
        }

        public static object Rsh(object a, int b)
        {
            if (a is byte)
            {
                return (byte)a >> b;
            }
            else
            if (a is sbyte)
            {
                return (sbyte)a >> b;
            }
            else
            if (a is short)
            {
                return (short)a >> b;
            }
            else
            if (a is ushort)
            {
                return (ushort)a >> b;
            }
            else
            if (a is int)
            {
                return (int)a >> b;
            }
            else
            if (a is uint)
            {
                return (uint)a >> b;
            }
            else
            if (a is long)
            {
                return (long)a >> b;
            }
            else
            if (a is ulong)
            {
                return (ulong)a >> b;
            }
            else
            if (a is char)
            {
                return (char)a >> b;
            }
            throw new Exception();
        }

        public static object Lsh(object a, int b)
        {
            if (a is byte)
            {
                return (byte)a << b;
            }
            else
            if (a is sbyte)
            {
                return (sbyte)a << b;
            }
            else
            if (a is short)
            {
                return (short)a << b;
            }
            else
            if (a is ushort)
            {
                return (ushort)a << b;
            }
            else
            if (a is int)
            {
                return (int)a << b;
            }
            else
            if (a is uint)
            {
                return (uint)a << b;
            }
            else
            if (a is long)
            {
                return (long)a << b;
            }
            else
            if (a is ulong)
            {
                return (ulong)a << b;
            }
            else
            if (a is char)
            {
                return (char)a << b;
            }
            throw new Exception();
        }
        
        public static object Neg(object a)
        {
            if (a is byte)
            {
                return -(byte)a;
            }
            else
            if (a is sbyte)
            {
                return -(sbyte)a;
            }
            else
            if (a is short)
            {
                return -(short)a;
            }
            else
            if (a is ushort)
            {
                return -(ushort)a;
            }
            else
            if (a is int)
            {
                return -(int)a;
            }
            else
            if (a is uint)
            {
                return -(uint)a;
            }
            else
            if (a is long)
            {
                return -(long)a;
            }
            else
            if (a is ulong)
            {
                throw new Exception();
                //return -(ulong)a;
            }
            else
            if (a is char)
            {
                return -(char)a;
            }
            else
            if (a is float)
            {
                return -(float)a;
            }
            else
            if (a is double)
            {
                return -(double)a;
            }
            throw new Exception();
        }

        public static object BitNot(object a)
        {
            if (a is byte)
            {
                return ~(byte)a;
            }
            else
            if (a is sbyte)
            {
                return ~(sbyte)a;
            }
            else
            if (a is short)
            {
                return ~(short)a;
            }
            else
            if (a is ushort)
            {
                return ~(ushort)a;
            }
            else
            if (a is int)
            {
                return ~(int)a;
            }
            else
            if (a is uint)
            {
                return ~(uint)a;
            }
            else
            if (a is long)
            {
                return ~(long)a;
            }
            else
            if (a is ulong)
            {
                return ~(ulong)a;
            }
            else
            if (a is char)
            {
                return ~(char)a;
            }
            throw new Exception();
        }

        public static bool Eq(object a, object b)
        {
            if (a == null ^ b == null)
                return false;
            if (a == null && b == null)
                return true;

            if (a is bool ^ b is bool)
                return false;
            if(a is bool && b is bool)
                return (bool)a == (bool)b;


            if (a is byte)
            {
                byte aa = (byte)a;
                if (b is byte)
                {
                    return aa == (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa == (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa == (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa == (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa == (int)b;
                }
                else
                if (b is uint)
                {
                    return aa == (uint)b;
                }
                else
                if (b is long)
                {
                    return aa == (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa == (ulong)b;
                }
                else if (b is char)
                {
                    return aa == (char)b;
                }
                else if (b is float)
                {
                    return aa == (float)b;
                }
                else if (b is double)
                {
                    return aa == (double)b;
                }
            }
            else
            if (a is sbyte)
            {
                sbyte aa = (sbyte)a;
                if (b is byte)
                {
                    return aa == (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa == (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa == (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa == (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa == (int)b;
                }
                else
                if (b is uint)
                {
                    return aa == (uint)b;
                }
                else
                if (b is long)
                {
                    return aa == (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa == (ulong)b;
                }
                else if (b is char)
                {
                    return aa == (char)b;
                }
                else if (b is float)
                {
                    return aa == (float)b;
                }
                else if (b is double)
                {
                    return aa == (double)b;
                }
            }
            else
            if (a is short)
            {
                short aa = (short)a;
                if (b is byte)
                {
                    return aa == (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa == (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa == (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa == (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa == (int)b;
                }
                else
                if (b is uint)
                {
                    return aa == (uint)b;
                }
                else
                if (b is long)
                {
                    return aa == (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa == (ulong)b;
                }
                else if (b is char)
                {
                    return aa == (char)b;
                }
                else if (b is float)
                {
                    return aa == (float)b;
                }
                else if (b is double)
                {
                    return aa == (double)b;
                }
            }
            else
            if (a is ushort)
            {
                ushort aa = (ushort)a;
                if (b is byte)
                {
                    return aa == (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa == (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa == (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa == (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa == (int)b;
                }
                else
                if (b is uint)
                {
                    return aa == (uint)b;
                }
                else
                if (b is long)
                {
                    return aa == (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa == (ulong)b;
                }
                else if (b is char)
                {
                    return aa == (char)b;
                }
                else if (b is float)
                {
                    return aa == (float)b;
                }
                else if (b is double)
                {
                    return aa == (double)b;
                }
            }
            else
            if (a is int)
            {
                int aa = (int)a;
                if (b is byte)
                {
                    return aa == (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa == (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa == (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa == (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa == (int)b;
                }
                else
                if (b is uint)
                {
                    return aa == (uint)b;
                }
                else
                if (b is long)
                {
                    return aa == (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa == (ulong)b;
                }
                else if (b is char)
                {
                    return aa == (char)b;
                }
                else if (b is float)
                {
                    return aa == (float)b;
                }
                else if (b is double)
                {
                    return aa == (double)b;
                }
            }
            else
            if (a is uint)
            {
                uint aa = (uint)a;
                if (b is byte)
                {
                    return aa == (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa == (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa == (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa == (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa == (int)b;
                }
                else
                if (b is uint)
                {
                    return aa == (uint)b;
                }
                else
                if (b is long)
                {
                    return aa == (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa == (ulong)b;
                }
                else if (b is char)
                {
                    return aa == (char)b;
                }
                else if (b is float)
                {
                    return aa == (float)b;
                }
                else if (b is double)
                {
                    return aa == (double)b;
                }
            }
            else
            if (a is long)
            {
                long aa = (long)a;
                if (b is byte)
                {
                    return aa == (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa == (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa == (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa == (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa == (int)b;
                }
                else
                if (b is uint)
                {
                    return aa == (uint)b;
                }
                else
                if (b is long)
                {
                    return aa == (long)b;
                }
                else
                if (b is ulong)
                {
                    throw new Exception();
                    //return aa == (ulong)b;
                }
                else if (b is char)
                {
                    return aa == (char)b;
                }
                else if (b is float)
                {
                    return aa == (float)b;
                }
                else if (b is double)
                {
                    return aa == (double)b;
                }
            }
            else
            if (a is ulong)
            {
                ulong aa = (ulong)a;
                if (b is byte)
                {
                    return aa == (byte)b;
                }
                else
                if (b is sbyte)
                {
                    throw new Exception();
                    //return aa == (sbyte)b;
                }
                else
                if (b is short)
                {
                    throw new Exception();
                    //return aa == (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa == (ushort)b;
                }
                else
                if (b is int)
                {
                    throw new Exception();
                    //return aa == (int)b;
                }
                else
                if (b is uint)
                {
                    return aa == (uint)b;
                }
                else
                if (b is long)
                {
                    throw new Exception();
                    //return aa == (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa == (ulong)b;
                }
                else if (b is char)
                {
                    return aa == (char)b;
                }
                else if (b is float)
                {
                    return aa == (float)b;
                }
                else if (b is double)
                {
                    return aa == (double)b;
                }
            }
            else
            if (a is char)
            {
                char aa = (char)a;
                if (b is byte)
                {
                    return aa == (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa == (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa == (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa == (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa == (int)b;
                }
                else
                if (b is uint)
                {
                    return aa == (uint)b;
                }
                else
                if (b is long)
                {
                    return aa == (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa == (ulong)b;
                }
                else if (b is char)
                {
                    return aa == (char)b;
                }
                else if (b is float)
                {
                    return aa == (float)b;
                }
                else if (b is double)
                {
                    return aa == (double)b;
                }
            }
            else
            if (a is float)
            {
                float aa = (float)a;
                if (b is byte)
                {
                    return aa == (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa == (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa == (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa == (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa == (int)b;
                }
                else
                if (b is uint)
                {
                    return aa == (uint)b;
                }
                else
                if (b is long)
                {
                    return aa == (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa == (ulong)b;
                }
                else if (b is char)
                {
                    return aa == (char)b;
                }
                else if (b is float)
                {
                    return aa == (float)b;
                }
                else if (b is double)
                {
                    return aa == (double)b;
                }
            }
            else
            if (a is double)
            {
                double aa = (double)a;
                if (b is byte)
                {
                    return aa == (byte)b;
                }
                else
                if (b is sbyte)
                {
                    return aa == (sbyte)b;
                }
                else
                if (b is short)
                {
                    return aa == (short)b;
                }
                else
                if (b is ushort)
                {
                    return aa == (ushort)b;
                }
                else
                if (b is int)
                {
                    return aa == (int)b;
                }
                else
                if (b is uint)
                {
                    return aa == (uint)b;
                }
                else
                if (b is long)
                {
                    return aa == (long)b;
                }
                else
                if (b is ulong)
                {
                    return aa == (ulong)b;
                }
                else if (b is char)
                {
                    return aa == (char)b;
                }
                else if (b is float)
                {
                    return aa == (float)b;
                }
                else if (b is double)
                {
                    return aa == (double)b;
                }
            }
            throw new Exception();
        }
    }
}
