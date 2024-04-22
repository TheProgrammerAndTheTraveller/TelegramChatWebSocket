using Encryption;
using Microsoft.EntityFrameworkCore;
using System;
using TelegramChat.Data;
using TelegramChat.Domain;
using TelegramChat.Server;
using TelegramChat.TelegramInteraction;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
builder.Services.AddHostedService<ChatHostedService>();
builder.Services.AddSingleton<IMessageHistoryRepository, MessageHistoryRepository>();
builder.Services.AddSingleton<IBotService, BotService>();
builder.Services.AddSingleton<Encryptor>();
builder.Services.AddSingleton<Decryptor>();

var connectionString = builder.Configuration.GetConnectionString("TelegramChatHistory");
builder.Services.AddDbContextFactory<TelegramChatDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHub<ChatHub>("/chat");

app.Run();


