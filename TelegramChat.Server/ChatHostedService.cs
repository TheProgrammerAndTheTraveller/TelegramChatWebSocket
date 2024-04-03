using Microsoft.AspNetCore.SignalR;
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

    public ChatHostedService(IBotService botService, IHubContext<ChatHub> hubContext)
    {
        _botService = botService;
        _botService.OnMessageReceived += OnMessageReceived;
        _hubContext = hubContext;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
    }

    private async Task OnMessageReceived(object? sender, ChatMessage message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.ChatId, message.Text);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }

}