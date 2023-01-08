using System.Runtime.InteropServices;
using System.Text;
using BitStreams;

namespace Woomiibo.Structs
{
    [Serializable]
    public struct NfpContent : IBitSerializable
    {
        private const int SaveDataLocalUniqueIdBits = 32;
        private const int NplnUserIdBits            = 32;
        private const int NameAryCountBits          = 4;
        private const int NameAryMax                = 10;
        private const int WinNumBits                = 27;
        private const int ModelTypeBits             = 4;
        private const int SkinColorBits             = 4;
        private const int EyeColorBits              = 5;
        private const int HairBits                  = 6;
        private const int BottomBits                = 6;
        private const int BottomVariationBits       = 4;
        private const int EyebrowBits               = 5;
        private const int MySetAryCountBits         = 3;
        private const int MySetAryMax               = 5;

        public uint SaveDataLocalUniqueId = 0;
        public uint NplnUserId = 0;
        private NfpU16[] NameAry = Array.Empty<NfpU16>();
        public string Name
        {
            get
            {
                return new string(NameAry
                        .Select(x => Convert.ToChar(x.Value))
                        .TakeWhile(x => x != '\x00')
                    .ToArray());
            }
            set
            {
                var bytes = Encoding.Unicode.GetBytes(value);
                var chars = MemoryMarshal.Cast<byte, ushort>(bytes);
                
                NameAry = new NfpU16[chars.Length];
                for (var i = 0; i < chars.Length; i++)
                {
                    NameAry[i].Value = chars[i];
                }
            }
        }

        public int WinNum = -1;
        public byte ModelType = 0;
        public byte SkinColor = 0;
        public byte EyeColor = 0;
        public sbyte Hair = -1;
        public sbyte Bottom = -1;
        public sbyte BottomVariation = -1;
        public byte Eyebrow = 0;
        public MySet EquipAndCtrl = default;
        public MySet[] MySetAry = Array.Empty<MySet>();

        public NfpContent()
        {
        }

        public void Read(BitStream stream)
        {
            stream.ReadBits(out SaveDataLocalUniqueId, SaveDataLocalUniqueIdBits);
            stream.ReadBits(out NplnUserId, NplnUserIdBits);
            stream.ReadBits(out NameAry, NameAryCountBits, NameAryMax);
            stream.ReadBits(out WinNum, WinNumBits);
            stream.ReadBits(out ModelType, ModelTypeBits);
            stream.ReadBits(out SkinColor, SkinColorBits);
            stream.ReadBits(out EyeColor, EyeColorBits);
            stream.ReadBits(out Hair, HairBits);
            stream.ReadBits(out Bottom, BottomBits);
            stream.ReadBits(out BottomVariation, BottomVariationBits);
            stream.ReadBits(out Eyebrow, EyebrowBits);
            stream.ReadBits(out EquipAndCtrl);
            stream.ReadBits(out MySetAry, MySetAryCountBits, MySetAryMax);
        }

        public void Write(BitStream stream)
        {
            stream.WriteBits(SaveDataLocalUniqueId, SaveDataLocalUniqueIdBits);
            stream.WriteBits(NplnUserId, NplnUserIdBits);
            stream.WriteBits(NameAry, NameAryCountBits, NameAryMax);
            stream.WriteBits(WinNum, WinNumBits);
            stream.WriteBits(ModelType, ModelTypeBits);
            stream.WriteBits(SkinColor, SkinColorBits);
            stream.WriteBits(EyeColor, EyeColorBits);
            stream.WriteBits(Hair, HairBits);
            stream.WriteBits(Bottom, BottomBits);
            stream.WriteBits(BottomVariation, BottomVariationBits);
            stream.WriteBits(Eyebrow, EyebrowBits);
            stream.WriteBits(EquipAndCtrl);
            stream.WriteBits(MySetAry, MySetAryCountBits, MySetAryMax);
        }
    }
}
