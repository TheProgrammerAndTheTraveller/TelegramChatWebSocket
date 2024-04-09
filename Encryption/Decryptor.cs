using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
namespace Encryption
{
    public class Decryptor 
    {
        private readonly string _containerName;
        private readonly CspParameters _cspParameters;

        public Decryptor(IConfiguration configuration)
        {
            _containerName = configuration["CryptoContainerName"]!;
            _cspParameters = new CspParameters
            {
                KeyContainerName = _containerName
            };
        }

        public byte[] Decrypt(byte[] data)
        {
            var rsa = new RSACryptoServiceProvider(_cspParameters);

            return rsa.Decrypt(data, true);
        }

        public byte[] GetPublicKey()
        {
            using (var rsa = new RSACryptoServiceProvider(_cspParameters))
            {
                rsa.PersistKeyInCsp = true;
                return rsa.ExportRSAPublicKey();
            }
        }


    }
}
//builder.Services.AddSingleton<IBotService, BotService>();
