using Encryption;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChat.Domain;
using TelegramChat.TelegramInteraction;
using TelegramInteraction;

namespace TelegramChat.Client
{
    public class ChatHostedService : IHostedService
    {
        HubConnection connection;

        private readonly IBotService _botService;
        private readonly Encryptor _encryptor;
        private readonly Decryptor _decryptor;
        private readonly IMessageHistoryRepository _repository;


        public ChatHostedService(IBotService botService, Encryptor encryptor, Decryptor decryptor, IMessageHistoryRepository messageHistoryRepository)
        {
            _botService = botService;
            _botService.OnMessageReceived += OnMessageReceived;
            _encryptor = encryptor;
            _decryptor = decryptor;
            _repository = messageHistoryRepository;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7287/chat")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync(cancellationToken);
            };

            await connection.StartAsync(cancellationToken);

            // Получение сообщения из SignalR.  ReceiveMessage - название метода в SignalR.
            // Должно совпадать с названием при отправке из хаба.
            connection.On<long, byte[]>("ReceiveMessage", async (chatId, message) =>
            {
                var decrypted = Encoding.UTF8.GetString(_decryptor.Decrypt(message));
                await _botService.SendMessage(new ChatMessage() { To = chatId, Text = decrypted }, cancellationToken);
            });

            connection.On<byte[]>("ReceivePublicKey", (key) => {
                _encryptor.SetPublicKey(key); 
            });

            await connection.SendAsync("ReceivePublicKeyInHub", _decryptor.GetPublicKey(), cancellationToken);
        }

        private async Task OnMessageReceived(object? sender, ChatMessage message)
        {
            // Если удалить эту строчку то сообщение в bot1 не отправится
            // отвечает за получение сообщения
            // ReceiveInHub - название метода в ChatHub.
            var encrypted = _encryptor.Encrypt(message.Text);
            await connection.SendAsync("ReceiveInHub", message.To, encrypted);

            await _repository.Add(message.From, message.To,encrypted);

        }


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await connection.StopAsync();
        }

    }
}   