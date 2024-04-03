using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChat.TelegramInteraction;
using TelegramInteraction;

namespace TelegramChat.Client
{
    public class ChatHostedService : IHostedService
    {
        HubConnection connection;

        private readonly IBotService _botService;


        public ChatHostedService(IBotService botService)
        {
            _botService = botService;
            _botService.OnMessageReceived += OnMessageReceived;
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

            connection.On<long, string>("ReceiveMessage", async (chatId, message) =>
            {
                await _botService.SendMessage(new ChatMessage() { ChatId = chatId, Text = message }, cancellationToken);
            });
        }

        private async Task OnMessageReceived(object? sender, ChatMessage message)
        {
            //Если удалить эту строчку то сообщение в bot1 не отправится
            //отвечает за получение сообщения
            await connection.SendAsync("ReceiveInHub", message.ChatId, message.Text);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await connection.StopAsync();
        }

    }
}   