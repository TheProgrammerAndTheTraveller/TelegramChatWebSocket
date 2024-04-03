using Microsoft.AspNetCore.SignalR;
using TelegramChat.TelegramInteraction;
using TelegramInteraction;

namespace TelegramChat.Server
{
    public class ChatHub : Hub
    {
        private readonly IBotService _botService;

        public ChatHub(IBotService botService)
        {
            _botService = botService;
        }

        public async Task ReceiveInHub(long channelId, string message)
        {
            //отправить полученное сообщение на указанный канал
            await _botService.SendMessage(new ChatMessage { ChatId = channelId, Text = message }, CancellationToken.None);
            Console.WriteLine($"Received message {message} from chat {channelId}");
        }
    }
}
