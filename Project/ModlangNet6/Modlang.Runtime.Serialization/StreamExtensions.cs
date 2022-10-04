using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Runtime.Serialization
{
    public static class StreamExtensions
    {
        #region Writing
        public static void WriteVarInt32(this Stream s, int v)
        {
            // we can store how many bytes in 2 bits
            int filledBytes = 0; // 0 would be 1, 1 would be 2
            for(int i = 1; i < 4; i++)
            {
                bool byteSet = ((v >> (i * 8)) & 0xFF) > 0;
                if (byteSet)
                    filledBytes = i;
            }
            ulong res = 0;
            res |= (byte)filledBytes;
            res |= (ulong)v << 2;
            int neededBytes = filledBytes + 1;
            for (int i = 0; i < neededBytes; i++)
                s.WriteByte((byte)((res >> (i * 8)) & 0xFF));
        }

        public static void WriteBool(this Stream s, bool v)
        {
            s.WriteByte((byte)(v ? 1 : 0));
        }

        public static void WriteSByte(this Stream s, sbyte v)
        {
            s.WriteByte((byte)(v & 0xFF));
        }

        public static void WriteUInt16(this Stream s, ushort v)
        {
            s.WriteByte((byte)(v >> 8));
            s.WriteByte((byte)(v & 0xFF));
        }

        public static void WriteInt16(this Stream s, short v)
        {
            s.WriteByte((byte)(v >> 8));
            s.WriteByte((byte)(v & 0xFF));
        }

        public static void WriteUInt32(this Stream s, uint v)
        {
            s.WriteByte((byte)(v >> 24));
            s.WriteByte((byte)((v >> 16) & 0xFF));
            s.WriteByte((byte)((v >> 8) & 0xFF));
            s.WriteByte((byte)(v & 0xFF));
        }

        public static void WriteInt32(this Stream s, int v)
        {
            s.WriteVarInt32(v);
            /*s.WriteByte((byte)(v >> 24));
            s.WriteByte((byte)((v >> 16) & 0xFF));
            s.WriteByte((byte)((v >> 8) & 0xFF));
            s.WriteByte((byte)(v & 0xFF));*/
        }

        public static void WriteUInt64(this Stream s, ulong v)
        {
            s.WriteByte((byte)(v >> 56));
            s.WriteByte((byte)((v >> 48) & 0xFF));
            s.WriteByte((byte)((v >> 40) & 0xFF));
            s.WriteByte((byte)((v >> 32) & 0xFF));
            s.WriteByte((byte)((v >> 24) & 0xFF));
            s.WriteByte((byte)((v >> 16) & 0xFF));
            s.WriteByte((byte)((v >> 8) & 0xFF));
            s.WriteByte((byte)(v & 0xFF));
        }

        public static void WriteInt64(this Stream s, long v)
        {
            s.WriteByte((byte)(v >> 56));
            s.WriteByte((byte)((v >> 48) & 0xFF));
            s.WriteByte((byte)((v >> 40) & 0xFF));
            s.WriteByte((byte)((v >> 32) & 0xFF));
            s.WriteByte((byte)((v >> 24) & 0xFF));
            s.WriteByte((byte)((v >> 16) & 0xFF));
            s.WriteByte((byte)((v >> 8) & 0xFF));
            s.WriteByte((byte)(v & 0xFF));
        }

        public static void WriteString(this Stream s, string v)
        {
            byte[] val = Encoding.Default.GetBytes(v);
            s.WriteInt32(val.Length);
            s.Write(val, 0, val.Length);
        }

        public static void WriteASCIIString(this Stream s, string v)
        {
            byte[] val = Encoding.ASCII.GetBytes(v);
            s.WriteInt32(val.Length);
            s.Write(val, 0, val.Length);
        }

        public static void WriteUTF8String(this Stream s, string v)
        {
            byte[] val = Encoding.UTF8.GetBytes(v);
            s.WriteInt32(val.Length);
            s.Write(val, 0, val.Length);
        }

        public static void WriteFloat(this Stream s, float v)
        {
            unsafe
            {
                uint val = *((uint*)&v);
                s.WriteUInt32(val);
            }
        }

        public static void WriteDouble(this Stream s, double v)
        {
            unsafe
            {
                ulong val = *((ulong*)&v);
                s.WriteUInt64(val);
            }
        }

        public static void WriteStringArray(this Stream s, string[] v)
        {
            s.WriteInt32(v.Length);
            foreach (string str in v) s.WriteUTF8String(str);
        }
        #endregion

        #region Reading
        public static int ReadVarInt32(this Stream s)
        {
            // we can store how many bytes in 2 bits
            byte firstByte = (byte)s.ReadByte();
            int filledBytes = firstByte & 0x03;
            int neededBytes = filledBytes + 1;
            ulong result = 0;
            result |= firstByte;
            for (int i = 1; i < neededBytes; i++) // we already have the first one
                result |= (ulong)s.ReadByte() << (i * 8);
            return (int)(result >> 2);
        }

        public static bool ReadBool(this Stream s)
        {
            return s.ReadByte() == 1;
        }

        public static sbyte ReadSByte(this Stream s)
        {
            return (sbyte)(s.ReadByte() - 128);
        }

        public static ushort ReadUInt16(this Stream s)
        {
            ushort res = 0;
            res |= (ushort)((ushort)s.ReadByte() << 8);
            res |= ((ushort)s.ReadByte());
            return res;
        }

        public static short ReadInt16(this Stream s)
        {
            short res = 0;
            res |= (short)((short)s.ReadByte() << 8);
            res |= ((short)s.ReadByte());
            return res;
        }

        public static uint ReadUInt32(this Stream s)
        {
            uint res = 0;
            res |= ((uint)s.ReadByte() << 24);
            res |= ((uint)s.ReadByte() << 16);
            res |= ((uint)s.ReadByte() << 8);
            res |= ((uint)s.ReadByte());
            return res;
        }

        public static int ReadInt32(this Stream s)
        {
            return s.ReadVarInt32();
            /*int res = 0;
            res |= ((int)s.ReadByte() << 24);
            res |= ((int)s.ReadByte() << 16);
            res |= ((int)s.ReadByte() << 8);
            res |= ((int)s.ReadByte());
            return res;*/
        }

        public static ulong ReadUInt64(this Stream s)
        {
            ulong res = 0;
            res |= ((ulong)s.ReadByte() << 56);
            res |= ((ulong)s.ReadByte() << 48);
            res |= ((ulong)s.ReadByte() << 40);
            res |= ((ulong)s.ReadByte() << 32);
            res |= ((ulong)s.ReadByte() << 24);
            res |= ((ulong)s.ReadByte() << 16);
            res |= ((ulong)s.ReadByte() << 8);
            res |= ((ulong)s.ReadByte());
            return res;
        }

        public static long ReadInt64(this Stream s)
        {
            long res = 0;
            res |= ((long)s.ReadByte() << 56);
            res |= ((long)s.ReadByte() << 48);
            res |= ((long)s.ReadByte() << 40);
            res |= ((long)s.ReadByte() << 32);
            res |= ((long)s.ReadByte() << 24);
            res |= ((long)s.ReadByte() << 16);
            res |= ((long)s.ReadByte() << 8);
            res |= ((long)s.ReadByte());
            return res;
        }

        public static string ReadString(this Stream s)
        {
            int len = s.ReadInt32();
            byte[] val = new byte[len];
            s.Read(val, 0, val.Length);
            return Encoding.Default.GetString(val);
        }

        public static string ReadASCIIString(this Stream s)
        {
            int len = s.ReadInt32();
            byte[] val = new byte[len];
            s.Read(val, 0, val.Length);
            return Encoding.ASCII.GetString(val);
        }

        public static string ReadUTF8String(this Stream s)
        {
            int len = s.ReadInt32();
            byte[] val = new byte[len];
            s.Read(val, 0, val.Length);
            return Encoding.UTF8.GetString(val);
        }

        public static float ReadFloat(this Stream s)
        {
            unsafe
            {
                uint val = s.ReadUInt32();
                return *(((float*)&val));
            }
        }

        public static double ReadDouble(this Stream s)
        {
            unsafe
            {
                ulong val = s.ReadUInt64();
                return *(((double*)&val));
            }
        }

        public static string[] ReadStringArray(this Stream s)
        {
            string[] res = new string[s.ReadInt32()];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = s.ReadUTF8String();
            }
            return res;
        }
        #endregion
    }
}
