using System.Diagnostics;
using BitStreams;
using Woomiibo.Structs;

namespace Woomiibo
{
    public static class BitStreamUtils
    {
        public static void ReadBits(this BitStream stream, out byte value, int bitCount)
        {
            Debug.Assert(bitCount <= 8);

            var i = bitCount;
            value = 0;
            while (i > 0)
            {
                var b = stream.ReadBit();
                value |= (byte)(b << (bitCount - i));
                i--;
            }
        }
        public static void ReadBits(this BitStream stream, out sbyte value, int bitCount)
        {
            ReadBits(stream, out byte b, bitCount);
            value = (sbyte)((sbyte)b - 1);
        }
        public static void ReadBits(this BitStream stream, out ushort value, int bitCount)
        {
            Debug.Assert(bitCount <= 16);

            var i = bitCount;
            value = 0;
            while (i > 0)
            {
                var b = stream.ReadBit();
                value |= (ushort)(b.AsInt() << (bitCount - i));
                i--;
            }

            //value.ReverseEndianness();
        }
        public static void ReadBits(this BitStream stream, out short value, int bitCount)
        {
            ReadBits(stream, out ushort b, bitCount);
            value = (short)((short)b - 1);
        }
        public static void ReadBits(this BitStream stream, out uint value, int bitCount)
        {
            Debug.Assert(bitCount <= 32);

            var i = bitCount;
            value = 0;
            while (i > 0)
            {
                var b = stream.ReadBit();
                value |= (uint)(b.AsInt() << (bitCount-i));
                i--;
            }
            
            //value.ReverseEndianness();
        }
        public static void ReadBits(this BitStream stream, out int value, int bitCount)
        {
            ReadBits(stream, out uint b, bitCount);
            value = (int)b - 1;
        }
        public static void ReadBits(this BitStream stream, out ulong value, int bitCount)
        {
            Debug.Assert(bitCount <= 64);

            var i = bitCount;
            value = 0;
            while (i > 0)
            {
                var b = stream.ReadBit();
                value |= (ulong)(b.AsInt() << (bitCount - i));
                i--;
            }

            //value.ReverseEndianness();
        }
        public static void ReadBits(this BitStream stream, out long value, int bitCount)
        {
            ReadBits(stream, out ulong b, bitCount);
            value = (long)b - 1;
        }
        public static void ReadBits<T>(this BitStream stream, out T[] value, int lengthBitCount, int maxCount) where T : struct, IBitSerializable
        {
            stream.ReadBits(out uint count, lengthBitCount);

            Debug.Assert(count <= maxCount);

            value = new T[count];
            foreach (ref var v in value.AsSpan())
            {
                v.Read(stream);
            }

            var blank = new T();
            for (var i = count; i < maxCount; i++)
            {
                blank.Read(stream);
            }
        }

        public static void ReadBits<T>(this BitStream stream, out T value) where T : struct, IBitSerializable
        {
            value = new T();
            value.Read(stream);
        }

        public static void ReadBits(this BitStream stream, out bool value)
        {
            value = stream.ReadBit();
        }

        public static void WriteBits<T>(this BitStream stream, T[] value, int lengthBitCount, int maxCount) where T : struct, IBitSerializable
        {
            stream.WriteBits((uint)value.Length, lengthBitCount);

            foreach (var v in value)
            {
                v.Write(stream);
            }

            var blank = new T();
            for (var i = value.Length; i < maxCount; i++)
            {
                blank.Write(stream);
            }
        }
        public static void WriteBits(this BitStream stream, byte value, int bitCount)
        {
            Debug.Assert(bitCount <= 8);
            Debug.Assert((value & ~((1 << bitCount) - 1)) == 0);

            var i = bitCount;
            while (i > 0)
            {
                stream.WriteBit(value >> (bitCount - i));
                i--;
            }
        }
        public static void WriteBits(this BitStream stream, sbyte value, int bitCount)
        {
            stream.WriteBits((byte)(value + 1), bitCount);
        }
        public static void WriteBits(this BitStream stream, ushort value, int bitCount)
        {
            Debug.Assert(bitCount <= 16);
            Debug.Assert((value & ~((1 << bitCount) - 1)) == 0);

            var i = bitCount;
            while (i > 0)
            {
                stream.WriteBit((value >> (bitCount - i)) & 1);
                i--;
            }
        }
        public static void WriteBits(this BitStream stream, short value, int bitCount)
        {
            stream.WriteBits((ushort)(value + 1), bitCount);
        }
        public static void WriteBits(this BitStream stream, uint value, int bitCount)
        {
            Debug.Assert(bitCount <= 32);
            Debug.Assert((value & ~((1 << bitCount) - 1)) == 0);

            var i = bitCount;
            while (i > 0)
            {
                stream.WriteBit((int)((value >> (bitCount - i))&1));
                i--;
            }
        }
        public static void WriteBits(this BitStream stream, int value, int bitCount)
        {
            stream.WriteBits((uint)(value + 1), bitCount);
        }
        public static void WriteBits(this BitStream stream, ulong value, int bitCount)
        {
            Debug.Assert(bitCount <= 64);
            Debug.Assert((value & ~((ulong)(1 << bitCount) - 1)) == 0);

            var i = bitCount;
            while (i > 0)
            {
                stream.WriteBit((int)((value >> (bitCount - i)) & 1));
                i--;
            }
        }
        public static void WriteBits(this BitStream stream, long value, int bitCount)
        {
            stream.WriteBits((ulong)(value + 1), bitCount);
        }

        public static void WriteBits<T>(this BitStream stream, T value) where T : struct, IBitSerializable
        {
            value.Write(stream);
        }
        public static void WriteBits(this BitStream stream, bool value)
        {
            stream.WriteBit(value);
        }
    }
}
