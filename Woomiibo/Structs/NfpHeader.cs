using BitStreams;

namespace Woomiibo.Structs
{
    [Serializable]
    public struct NfpHeader : IBitSerializable
    {
        private const int VersionBits   = 8;
        private const int HashBits      = 32;

        public const int TotalSize = (VersionBits + HashBits) / 8;

        public byte Version;
        public uint Hash;
        public void Read(BitStream stream)
        {
            stream.ReadBits(out Version, VersionBits);
            stream.ReadBits(out Hash, HashBits);
        }

        public void Write(BitStream stream)
        {
            stream.WriteBits(Version, VersionBits);
            stream.WriteBits(Hash, HashBits);
        }
    }
}
