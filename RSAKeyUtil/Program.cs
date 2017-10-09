using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RSAKeyUtil
{
    class Program
    {
        private static (string, string) GenerateKeysXml(int keySizeInBits)
        {
            string privateKeyXml = null;
            string publicKeyXml = null;

            const int PROVIDER_RSA_FULL = 1;
            var cspParameter = new CspParameters(PROVIDER_RSA_FULL);
            using (var rsaProvider = new RSACryptoServiceProvider(keySizeInBits, cspParameter))
            {
                privateKeyXml = rsaProvider.ToXmlString(true);
                publicKeyXml = rsaProvider.ToXmlString(false);

                rsaProvider.Clear();
            }
            return (privateKeyXml, publicKeyXml);
        }

        private static void GenerateKeyPair(int keySizeInBits, string privateKeyFilename, string publicKeyFilename)
        {
            StreamWriter privateKeyXmlFile = null;
            StreamWriter publicKeyXmlFile = null;
            try
            {
                (string privateKeyXml, string publicKeyXml) = GenerateKeysXml(keySizeInBits);

                privateKeyFilename = Path.ChangeExtension(privateKeyFilename, "xml");
                privateKeyXmlFile = File.CreateText(privateKeyFilename);
                privateKeyXmlFile.Write(privateKeyXml);
                privateKeyXmlFile.Flush();

				publicKeyFilename = Path.ChangeExtension(publicKeyFilename, "xml");
				publicKeyXmlFile = File.CreateText(publicKeyFilename);
				publicKeyXmlFile.Write(publicKeyXml);
				publicKeyXmlFile.Flush();
			}
            finally
            {
                if (privateKeyXmlFile != null)
                {
                    privateKeyXmlFile.Close();
                }
                if (publicKeyXmlFile != null)
                {
                    publicKeyXmlFile.Close();
                }
            }
        }

		// https://stackoverflow.com/questions/28406888/c-sharp-rsa-public-key-output-not-correct/28407693#28407693
		private static void ExportPublicKey(RSACryptoServiceProvider csp, TextWriter outputStream)
        {
            var parameters = csp.ExportParameters(false);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                    EncodeLength(innerWriter, rsaEncryptionOid.Length);
                    innerWriter.Write(rsaEncryptionOid);
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using (var bitStringStream = new MemoryStream())
                    {
                        var bitStringWriter = new BinaryWriter(bitStringStream);
                        bitStringWriter.Write((byte)0x00); // # of unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using (var paramsStream = new MemoryStream())
                        {
                            var paramsWriter = new BinaryWriter(paramsStream);
                            EncodeIntegerBigEndian(paramsWriter, parameters.Modulus); // Modulus
                            EncodeIntegerBigEndian(paramsWriter, parameters.Exponent); // Exponent
                            var paramsLength = (int)paramsStream.Length;
                            EncodeLength(bitStringWriter, paramsLength);
                            bitStringWriter.Write(paramsStream.GetBuffer(), 0, paramsLength);
                        }
                        var bitStringLength = (int)bitStringStream.Length;
                        EncodeLength(innerWriter, bitStringLength);
                        innerWriter.Write(bitStringStream.GetBuffer(), 0, bitStringLength);
                    }
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                outputStream.WriteLine("-----BEGIN PUBLIC KEY-----");
                for (var i = 0; i < base64.Length; i += 64)
                {
                    outputStream.WriteLine(base64, i, Math.Min(64, base64.Length - i));
                }
                outputStream.WriteLine("-----END PUBLIC KEY-----");
            }
        }

		private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
		{
			stream.Write((byte)0x02); // INTEGER
			var prefixZeros = 0;
			for (var i = 0; i < value.Length; i++)
			{
				if (value[i] != 0) break;
				prefixZeros++;
			}
			if (value.Length - prefixZeros == 0)
			{
				EncodeLength(stream, 1);
				stream.Write((byte)0);
			}
			else
			{
				if (forceUnsigned && value[prefixZeros] > 0x7f)
				{
					// Add a prefix zero to force unsigned if the MSB is 1
					EncodeLength(stream, value.Length - prefixZeros + 1);
					stream.Write((byte)0);
				}
				else
				{
					EncodeLength(stream, value.Length - prefixZeros);
				}
				for (var i = prefixZeros; i < value.Length; i++)
				{
					stream.Write(value[i]);
				}
			}
		}

		private static void EncodeLength(BinaryWriter stream, int length)
		{
			if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
			if (length < 0x80)
			{
				// Short form
				stream.Write((byte)length);
			}
			else
			{
				// Long form
				var temp = length;
				var bytesRequired = 0;
				while (temp > 0)
				{
					temp >>= 8;
					bytesRequired++;
				}
				stream.Write((byte)(bytesRequired | 0x80));
				for (var i = bytesRequired - 1; i >= 0; i--)
				{
					stream.Write((byte)(length >> (8 * i) & 0xff));
				}
			}
		}

		private static void ExportPublicKey(string publicKeyFilename)
        {
            var rsaProvider = new RSACryptoServiceProvider();
            publicKeyFilename = Path.ChangeExtension(publicKeyFilename, "xml");
            rsaProvider.FromXmlString(File.ReadAllText(publicKeyFilename));

            var exportedFilename = Path.ChangeExtension(publicKeyFilename, "pub");
            StreamWriter exportedFile = File.CreateText(exportedFilename);
            ExportPublicKey(rsaProvider, exportedFile);
        }

        private static void EncryptFile(string keyFilename, string inputFilename)
        {
            var rsaProvider = new RSACryptoServiceProvider();
            keyFilename = Path.ChangeExtension(keyFilename, "xml");
            rsaProvider.FromXmlString(File.ReadAllText(keyFilename));

            var inputText = File.ReadAllText(inputFilename);
            var inputBytes = Encoding.ASCII.GetBytes(inputText);

            var encryptedBytesWithPadding = rsaProvider.Encrypt(inputBytes, true);
            var encryptedFilenameWithPadding = Path.ChangeExtension(inputFilename, "enc1");
            File.WriteAllBytes(encryptedFilenameWithPadding, encryptedBytesWithPadding);

            var encryptedBytes = rsaProvider.Encrypt(inputBytes, false);
            var encryptedFilename = Path.ChangeExtension(inputFilename, "enc");
            File.WriteAllBytes(encryptedFilename, encryptedBytesWithPadding);
        }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
				var command = args[0];
                if (command == "-g" || command == "--generate")
                {
                    var privateKeyFilename = args[1];
                    var publicKeyFilename = args[2];
                    GenerateKeyPair(2048, privateKeyFilename, publicKeyFilename);
                }
                else if (command == "-x" || command == "--export")
                {
                    var publicKeyFilename = args[1];
                    ExportPublicKey(publicKeyFilename);
                }
                else if (command == "-e" || command == "--encrypt")
                {
                    var keyFilename = args[1];
                    var inputFilename = args[2];
                    EncryptFile(keyFilename, inputFilename);
                }
			}
            Console.ReadKey();
        }
    }
}
