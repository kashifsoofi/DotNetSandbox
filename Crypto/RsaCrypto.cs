using System.Text;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace Crypto
{
    public class RsaCrypto
    {
        private readonly string _algorithm = "RSA/ECB/OAEPWITHSHA1ANDMGF1PADDING";

        public AsymmetricKeyParameter LoadPrivate(string serialized)
        {
            return PrivateKeyFactory.CreateKey(Decode(serialized));
        }

        public AsymmetricKeyParameter LoadPublic(string serialized)
        {
            return PublicKeyFactory.CreateKey(Decode(serialized));
        }

        public string Encrypt(string plainText, AsymmetricKeyParameter encryptionKey)
        {
            var cipher = CipherUtilities.GetCipher(_algorithm);
            cipher.Init(true, encryptionKey);

            var data = Encoding.UTF8.GetBytes(plainText);
            var cipherText = cipher.DoFinal(data);
            return Encode(cipherText);
        }

        public string Decrypt(string encrypted, AsymmetricKeyParameter decryptionKey)
        {
            var cipher = CipherUtilities.GetCipher(_algorithm);
            cipher.Init(false, decryptionKey);

            var cipherText = Decode(encrypted);
            var data = cipher.DoFinal(cipherText);
            return Encoding.UTF8.GetString(data);
        }

        private string Encode(byte[] data)
        {
            return Base64UrlEncoder.Encode(data);
        }

        private byte[] Decode(string serialized)
        {
            return Base64UrlEncoder.DecodeBytes(serialized);
        }
    }
}
