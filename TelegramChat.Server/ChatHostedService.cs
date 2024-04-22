using Encryption;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChat.Domain;
using TelegramChat.TelegramInteraction;
using TelegramInteraction;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;

namespace TelegramChat.Server;

public class ChatHostedService : IHostedService
{
    private readonly IBotService _botService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly Encryptor _encryptor;
    private readonly IMessageHistoryRepository _repository;

    public ChatHostedService(IBotService botService, IHubContext<ChatHub> hubContext, Encryptor encryptor, IMessageHistoryRepository repository)
    {
        _botService = botService;
        _botService.OnMessageReceived += OnMessageReceived;
        _hubContext = hubContext;
        _encryptor = encryptor;
        _repository = repository;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {

    }

    private async Task OnMessageReceived(object? sender, ChatMessage message )
    {
        var encrypted = _encryptor.Encrypt(message.Text);

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.To, encrypted);

        await _repository.Add(message.From, message.To, encrypted);

    }


    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }

}