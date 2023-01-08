using System.Text.Json;
using BitStreams;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Woomiibo.Structs
{
    [Serializable]
    public struct NfpData : IBitSerializable
    {
        private const int TotalSize = 0xD8;

        private static readonly byte[] Key = {
            0x7E, 0xDA, 0x9B, 0x54, 0xA4, 0x96, 0xF6, 0xCA, 0xB7, 0x8A, 0xB6, 0x5B, 0x0E, 0xE5, 0xEB, 0x03, 
            0x5C, 0x45, 0x68, 0x68, 0x99, 0x0D, 0xC9, 0x91, 0x1D, 0x77, 0x2B, 0x88, 0x97, 0xDE, 0x3E, 0xC5
        };


        private static readonly byte[] Iv = {
            0x28, 0x8D, 0xE9, 0xD3, 0x37, 0xBA, 0x84, 0x3A, 0xD2, 0x70, 0x18, 0x0A, 0x76, 0xE8, 0xD8, 0xF5
        };


        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            IncludeFields = true,
            WriteIndented = true,
        };


        public NfpHeader Header;
        public NfpContent Content;

        public void Read(BitStream stream)
        {
            stream.ReadBits(out Header);
            stream.ReadBits(out Content);
        }

        public void Write(BitStream stream)
        {
            stream.WriteBits(Header);
            stream.WriteBits(Content);
        }

        public string SerializeToJson()
        {
            return JsonSerializer.Serialize(this, JsonOptions);
        }

        public void SerializeToBin(Stream stream, bool encrypted = false)
        {
            var headerBitStream = new BitStream(new byte[NfpHeader.TotalSize]);

            /* Compute the content section. */
            var contentBitStream = new BitStream(new byte[TotalSize - NfpHeader.TotalSize]);
            Content.Write(contentBitStream);

            /* Compute the hash of the content section. */
            uint hash;
            var contentStream = (MemoryStream) contentBitStream.GetStream();
            var contentLength = (int)contentStream.Position - 1;
            using (contentStream.TemporarySeek(0, SeekOrigin.Begin))
            {
                var bytes = contentStream.ToArray().AsSpan();
                hash = Murmur3.Compute(bytes[..contentLength]);
            }

            /* Populate header with computed hash. */
            Header.Hash = hash;
            headerBitStream.WriteBits(Header);

            /* Pick stream to use. */
            stream = encrypted ? CreateCipherStream(stream, true) : stream;

            /* Write header and content to stream. */
            var headerStream = headerBitStream.GetStream();
            using(headerStream.TemporarySeek(0, SeekOrigin.Begin))
                headerBitStream.GetStream().CopyTo(stream);
            using (contentStream.TemporarySeek(0, SeekOrigin.Begin))
                contentBitStream.GetStream().CopyTo(stream);
        }

        public void SerializeToBin(FileInfo fi, bool encrypted = false)
        {
            using var stream = fi.Create();
            SerializeToBin(stream, encrypted);
        }

        public byte[] SerializeToBin(bool encrypted = false)
        {
            var bytes = new byte[TotalSize];
            SerializeToBin(new MemoryStream(bytes), encrypted);
            return bytes;
        }

        public static NfpData DeserializeFromJson(FileInfo fi)
        {
            using var stream = fi.OpenRead();
            return JsonSerializer.Deserialize<NfpData>(stream, JsonOptions);
        }

        public static NfpData DeserializeFromBin(Stream stream, bool encrypted = false)
        {
            stream = encrypted ? CreateCipherStream(stream, false) : stream;

            var bitStream = new BitStream(stream);
            bitStream.ReadBits(out NfpData nfp);
            return nfp;
        }

        public static NfpData DeserializeFromBin(FileInfo fi, bool encrypted = false)
        {
            using var stream = fi.OpenRead();
            return DeserializeFromBin(stream, encrypted);
        }

        public static NfpData DeserializeFromBin(byte[] data, bool encrypted = false)
        {
            using var stream = new MemoryStream(data);
            return DeserializeFromBin(stream, encrypted);
        }

        private static IBufferedCipher GetCipher(bool forEncrypt)
        {
            var cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
            cipher.Init(forEncrypt, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", Key), Iv));
            return cipher;
        }

        private static CipherStream CreateCipherStream(Stream stream, bool forEncrypt)
        {
            var cipher = GetCipher(forEncrypt);
            return new CipherStream(stream, cipher, cipher);
        }
    }
}
