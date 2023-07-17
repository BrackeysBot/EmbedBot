using DSharpPlus;
using EmbedBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using X10D.Hosting.DependencyInjection;

Directory.CreateDirectory("data");

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("data/config.json", true, true);

builder.Logging.ClearProviders();
builder.Logging.AddNLog();

builder.Services.AddSingleton(new DiscordClient(new DiscordConfiguration
{
    Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
    LoggerFactory = new NLogLoggerFactory(),
    Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
}));

builder.Services.AddHostedSingleton<LoggingService>();
builder.Services.AddHostedSingleton<BotService>();

IHost app = builder.Build();
await app.RunAsync();
