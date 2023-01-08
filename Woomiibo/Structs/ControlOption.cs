using BitStreams;

namespace Woomiibo.Structs
{
    public struct ControlOption : IBitSerializable
    {
        private const int CameraSpeedStickDigitalBits   = 5;
        private const int CameraSpeedGyroDigitalBits    = 5;

        public byte CameraSpeedStickDigital;
        public byte CameraSpeedGyroDigital;
        public bool IsEnableGyro;
        public bool IsReverseUD;
        public bool IsReverseLR;

        public void Read(BitStream stream)
        {
            stream.ReadBits(out CameraSpeedStickDigital, CameraSpeedStickDigitalBits);
            stream.ReadBits(out CameraSpeedGyroDigital, CameraSpeedGyroDigitalBits);
            stream.ReadBits(out IsEnableGyro);
            stream.ReadBits(out IsReverseUD);
            stream.ReadBits(out IsReverseLR);
        }

        public void Write(BitStream stream)
        {
            stream.WriteBits(CameraSpeedStickDigital, CameraSpeedStickDigitalBits);
            stream.WriteBits(CameraSpeedGyroDigital, CameraSpeedGyroDigitalBits);
            stream.WriteBits(IsEnableGyro);
            stream.WriteBits(IsReverseUD);
            stream.WriteBits(IsReverseLR);
        }
    }
}
