using Encryption;
using Microsoft.EntityFrameworkCore;
using TelegramChat.Client;
using TelegramChat.Data;
using TelegramChat.Domain;
using TelegramChat.TelegramInteraction;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
builder.Services.AddSingleton<IBotService, BotService>();
builder.Services.AddHostedService<ChatHostedService>();
builder.Services.AddScoped<IMessageHistoryRepository, MessageHistoryRepository>();
builder.Services.AddSingleton<Encryptor>();
builder.Services.AddSingleton<Decryptor>();


var connectionString = builder.Configuration.GetConnectionString("TelegramChatHistory");
builder.Services.AddDbContext<TelegramChatDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();



app.MapGet("/", () => "Hello World!");

app.Run();
