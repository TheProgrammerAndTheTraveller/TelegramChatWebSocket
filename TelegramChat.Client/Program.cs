using TelegramChat.Client;
using TelegramChat.TelegramInteraction;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
builder.Services.AddSingleton<IBotService, BotService>();
builder.Services.AddHostedService<ChatHostedService>();

var app = builder.Build();



app.MapGet("/", () => "Hello World!");

app.Run();
