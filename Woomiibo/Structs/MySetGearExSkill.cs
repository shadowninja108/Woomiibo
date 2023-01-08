using BitStreams;

namespace Woomiibo.Structs
{
    [Serializable]
    public struct MySetGearExSkill : IBitSerializable
    {
        private const int ValueBits = 8;

        public sbyte Value = -1;

        public MySetGearExSkill()
        {
        }

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
