using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramInteraction;

namespace TelegramChat.TelegramInteraction;

public class BotService : IBotService
{
    private readonly TelegramBotClient _botClient;
    private readonly Dictionary<long, long> _conversations = new ();

    public BotService(IConfiguration configuration)
    {
        _botClient = new TelegramBotClient(configuration["TelegramToken"]!);

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };

        // Начинаем принимать сообщения из бота
        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions
        );
    }

    public async Task SendMessage(ChatMessage message, CancellationToken token)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: message.Text,
            cancellationToken: token);
    }

    public delegate Task OnMessageReceivedEvent(object sender, ChatMessage e);

    public OnMessageReceivedEvent OnMessageReceived { get; set; }

    /// <summary>
    /// Выполняется  при получении ошибки из бота
    /// </summary>
    /// <param name="client">Клиент бота</param>
    /// <param name="exception">Ошибка</param>
    /// <param name="token">Токен отмены</param>
    /// <returns></returns>
    private static Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        throw exception;
    }

    /// <summary>
    /// Принимает сообщения из бота
    /// </summary>
    /// <param name="client">Клиент бота</param>
    /// <param name="update">Сообщение</param>
    /// <param name="token">Токен отмены</param>
    /// <returns></returns>
    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
            return;
        // Only process text messages
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        Console.WriteLine($"Received a '{messageText}' message in chat {chatId} from {message.From}.");

        if (_conversations.ContainsKey(chatId))
        {
            if (_conversations[chatId] == 0 && long.TryParse(messageText, out var otherChatId))
            {
                _conversations[chatId] = otherChatId;
            }

            await OnMessageReceived(this, new ChatMessage { ChatId = _conversations[chatId], Text = messageText });
            return;
        }

        _conversations.Add(chatId, 0);
        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"Ваш id чата: {chatId}",
            cancellationToken: token);
    }
}
