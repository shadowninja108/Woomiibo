using BitStreams;

namespace Woomiibo.Structs
{
    [Serializable]
    public struct NfpU16 : IBitSerializable
    {
        private const int ValueBits = 16;

        public ushort Value;

        public void Read(BitStream stream)
        {
            stream.ReadBits(out Value, ValueBits);
        }

        public void Write(BitStream stream)
        {
            stream.WriteBits(Value, ValueBits);
        }
    }
}
