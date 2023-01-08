using BitStreams;

namespace Woomiibo.Structs
{
    [Serializable]
    public struct MySet : IBitSerializable
    {
        private const int WeaponSetIdBits   = 16;
        private const int GearAryCountBits  = 2;
        private const int GearAryMax        = 3;

        public short WeaponSetId = -1;
        public MySetGear[] GearAry = Array.Empty<MySetGear>();
        public ControlOption ControlOptionHandheld = default;
        public ControlOption ControlOptionOther = default;

        public MySet()
        {
        }

        public void Read(BitStream stream)
        {
            stream.ReadBits(out WeaponSetId, WeaponSetIdBits);
            stream.ReadBits(out GearAry, GearAryCountBits, GearAryMax);
            stream.ReadBits(out ControlOptionHandheld);
            stream.ReadBits(out ControlOptionOther);
        }

        public void Write(BitStream stream)
        {
            stream.WriteBits(WeaponSetId, WeaponSetIdBits);
            stream.WriteBits(GearAry, GearAryCountBits, GearAryMax);
            stream.WriteBits(ControlOptionHandheld);
            stream.WriteBits(ControlOptionOther);
        }
    }
}
