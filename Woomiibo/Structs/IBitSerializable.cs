using BitStreams;

namespace Woomiibo.Structs
{
    public interface IBitSerializable
    {
        void Read(BitStream stream);
        void Write(BitStream stream);
    }
}
