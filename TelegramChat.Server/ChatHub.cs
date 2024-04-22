using Encryption;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography;
using System.Text;
using TelegramChat.TelegramInteraction;
using TelegramInteraction;

namespace TelegramChat.Server
{
    public class ChatHub : Hub
    {
        private readonly IBotService _botService;
        private readonly Encryptor _encryptor;
        private readonly Decryptor _decryptor;

        public ChatHub(IBotService botService, Encryptor encryptor, Decryptor decryptor)
        {
            _botService = botService;
            _encryptor = encryptor;
            _decryptor = decryptor;
        }

        public async Task ReceivePublicKeyInHub(byte[] publicKey)
        {
            _encryptor.SetPublicKey(publicKey);

            // Отправка открытого ключа в SignalR
            var currentPublicKey = _decryptor.GetPublicKey();
            
            await Clients.Caller.SendAsync("ReceivePublicKey", currentPublicKey);
        }

        public async Task ReceiveInHub(long channelId, byte[] message)
        {
            var decrypted = Encoding.UTF8.GetString(_decryptor.Decrypt(message));

            //отправить полученное сообщение на указанный канал
            await _botService.SendMessage(new ChatMessage { To = channelId, Text = decrypted }, CancellationToken.None);
            Console.WriteLine($"Received message {message} from chat {channelId}");
        }
        
    }
}
