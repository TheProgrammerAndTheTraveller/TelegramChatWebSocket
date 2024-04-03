using TelegramChat.Server;
using TelegramChat.TelegramInteraction;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

builder.Services.AddHostedService<ChatHostedService>();
builder.Services.AddSingleton<IBotService, BotService>();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHub<ChatHub>("/chat");

app.Run();


