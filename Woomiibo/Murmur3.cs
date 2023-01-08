using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Woomiibo
{
    public class Murmur3
    {
        private const uint C1 = 0xcc9e2d51;
        private const uint C2 = 0x1b873593;
        private const int R1 = 15;
        private const int R2 = 13;
        private const uint M = 5;
        private const uint N = 0xe6546b64;

        private enum Endian
        {
            Little, Big
        }

        private const Endian TargetEndian = Endian.Little;

        private uint Length = 0;
        private uint H = 0;

        private static uint GetBlock(ReadOnlySpan<uint> blocks, int i)
        {
            var block = blocks[i];

            if(BitConverter.IsLittleEndian != (TargetEndian == Endian.Little))
                block.ReverseEndianness();

            return block;
        }

        private static ReadOnlySpan<uint> BytesToBlocks(ReadOnlySpan<byte> span) {
            return MemoryMarshal.Cast<byte, uint>(span);
        }


        private static uint Scramble(uint k)
        {
            k *= C1;
            k = BitOperations.RotateLeft(k, R1);
            k *= C2;
            return k;
        }

        public void Initialize(uint seed = 0)
        {
            H = seed;
            Length = 0;
        }

        public void Update(ReadOnlySpan<byte> data)
        {
            var blocknum = BlockNum(data);
            var blocks = BytesToBlocks(data);

            for (var i = 0; i < blocknum; i++)
            {
                var k1 = GetBlock(blocks, i);

                H ^= Scramble(k1);
                H = BitOperations.RotateLeft(H, R2);
                H = H * M + N;
            }

            var tail = TailOfBlocks(data);

            uint k = 0;

            switch (data.Length % Unsafe.SizeOf<uint>())
            {
                case 3:
                    k ^= (uint)(tail[2] << 16);
                    goto case 2;
                case 2:
                    k ^= (uint)(tail[1] << 8);
                    goto case 1;
                case 1:
                    k ^= (uint)(tail[0] << 0);
                    H ^= Scramble(k);
                    break;
            }

            Length += (uint)data.Length;
        }

        public uint Finalize()
        {
            H ^= Length;

            var h = H;
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;

            Initialize(0);
            return h;
        }
        private static int BlockNum(ReadOnlySpan<byte> span)
        {
            return span.Length / Unsafe.SizeOf<uint>();
        }

        public static ReadOnlySpan<byte> TailOfBlocks(ReadOnlySpan<byte> span)
        {
            return span[(BlockNum(span) * Unsafe.SizeOf<uint>())..];
        }

        public static uint Compute(ReadOnlySpan<byte> data, uint seed = 0)
        {
            var m = new Murmur3();
            m.Initialize(seed);

            var lengthRoundedDown = BlockNum(data) * Unsafe.SizeOf<uint>();

            m.Update(data[..lengthRoundedDown]);
            m.Update(data[lengthRoundedDown..]);

            return m.Finalize();
        }

        public static uint Compute(string str, uint seed = 0)
        {
            return Compute(Encoding.UTF8.GetBytes(str), seed);
        }
    }
}
