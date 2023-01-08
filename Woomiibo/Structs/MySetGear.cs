using BitStreams;

namespace Woomiibo.Structs
{
    [Serializable]
    public struct MySetGear : IBitSerializable
    {
        private const int IdBits            = 16;
        private const int VariationBits     = 4;
        private const int MainSkillBits     = 8;
        private const int ExSkillsCountBits = 2;
        private const int ExSkillsMax       = 3;

        public ushort Id = 0;
        public byte Variation = 0;
        public sbyte MainSkill = -1;
        public MySetGearExSkill[] ExSkills = Array.Empty<MySetGearExSkill>();

        public MySetGear()
        {
        }

        public void Read(BitStream stream)
        {
            stream.ReadBits(out Id, IdBits);
            stream.ReadBits(out Variation, VariationBits);
            stream.ReadBits(out MainSkill, MainSkillBits);
            stream.ReadBits(out ExSkills, ExSkillsCountBits, ExSkillsMax);
        }

        public void Write(BitStream stream)
        {
            stream.WriteBits(Id, IdBits);
            stream.WriteBits(Variation, VariationBits);
            stream.WriteBits(MainSkill, MainSkillBits);
            stream.WriteBits(ExSkills, ExSkillsCountBits, ExSkillsMax);
        }
    }
}
