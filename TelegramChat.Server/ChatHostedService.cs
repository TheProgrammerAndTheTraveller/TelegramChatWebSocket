using Encryption;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChat.TelegramInteraction;
using TelegramInteraction;
using static System.Net.Mime.MediaTypeNames;

namespace TelegramChat.Server;

public class ChatHostedService : IHostedService
{
    private readonly IBotService _botService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly Encryptor _encryptor;
    private readonly Decryptor _decryptor;


    public ChatHostedService(IBotService botService, IHubContext<ChatHub> hubContext, Decryptor decryptor)
    {
        _botService = botService;
        _botService.OnMessageReceived += OnMessageReceived;
        _hubContext = hubContext;
        _decryptor = decryptor;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {

    }

    private async Task OnMessageReceived(object? sender, ChatMessage message)
    {
        var encrypted = Encoding.UTF8.GetString(_encryptor.Encrypt(message.Text));

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.ChatId, encrypted);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }

}