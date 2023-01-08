using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace Woomiibo
{
    public static class Endianness
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this ref ushort value)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this ref short value)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this ref uint value)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this ref int value)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this ref ulong value)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseEndianness(this ref long value)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
    }
}
