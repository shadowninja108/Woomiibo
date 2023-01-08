using System.Text;
using Woomiibo.Structs;

namespace Woomiibo
{
    internal class Program
    {
        enum Operation
        {
            Encode,
            Decode,
        }

        private static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Arguments: [encode|decode] <path to output> <path to input>");
                return;
            }

            var opStr = args[0];
            var outputStr = args[1];
            var inputStr = args[2];

            Operation op;
            switch (opStr)
            {
                case "encode":
                    op = Operation.Encode;
                    break;
                case "decode":
                    op = Operation.Decode;
                    break;
                default:
                    Console.WriteLine("Invalid operation.");
                    return;
            }

            var output = new FileInfo(outputStr);
            var input = new FileInfo(inputStr);

            if (!input.Exists)
            {
                Console.WriteLine("Input does not exist!");
                return;
            }

            switch (op)
            {
                case Operation.Encode:
                    Encode(output, input);
                    break;
                case Operation.Decode:
                    Decode(output, input);
                    break;
            }
        }

        private static void Encode(FileInfo output, FileInfo input)
        {
            /* Deserialize JSON input into NfpData. */
            var nfp = NfpData.DeserializeFromJson(input);
            /* Write NfpData to output as encrypted binary. */
            using var os = output.Create();
            os.Write(nfp.SerializeToBin(true));
        }

        private static void Decode(FileInfo output, FileInfo input)
        {
            /* Deserialize encrypted binary input into NfpData. */
            var nfp = NfpData.DeserializeFromBin(input, true);
            /* Write NfpData to output as JSON. */
            using var os = output.Create();
            os.Write(Encoding.UTF8.GetBytes(nfp.SerializeToJson()));
        }
    }
}