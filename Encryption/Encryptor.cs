using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    public class Encryptor
    {
        private readonly string _containerName;
        private byte[]? publicKey;
        private readonly CspParameters _cspParameters;
        public Encryptor(IConfiguration configuration)
        {
            _containerName = configuration["CryptoContainerName"]!;
            _cspParameters = new CspParameters
            {
                KeyContainerName = _containerName
            };
        }

        public void SetPublicKey(byte[] publicKey)
        {
            Console.WriteLine($"Recieved public key in {AppDomain.CurrentDomain.FriendlyName}");

            if (publicKey == null || publicKey.Length == 0)
            {
                throw new ArgumentException("Invalid public key");
            }

            this.publicKey = publicKey;
        }

        public byte[] Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (publicKey == null)
            {
                throw new InvalidOperationException("Public key is not set.");
            }

            var rsa = new RSACryptoServiceProvider(_cspParameters);
            rsa.ImportRSAPublicKey(publicKey, out var _);

            var dataBytes = Encoding.UTF8.GetBytes(text);
            var encryptedData = rsa.Encrypt(dataBytes, true);
            return encryptedData;
        }
    }
}
