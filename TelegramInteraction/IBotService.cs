

using TelegramInteraction;
using static TelegramChat.TelegramInteraction.BotService;

namespace TelegramChat.TelegramInteraction
{
    public interface IBotService
    {
        public OnMessageReceivedEvent OnMessageReceived { get; set; }

        Task SendMessage(ChatMessage message, CancellationToken token);
       
    }
}