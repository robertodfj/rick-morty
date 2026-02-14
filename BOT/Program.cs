using System;
using System.Threading;
using Telegram.Bot;
using Bot.handler;
using Bot.service;
using Bot.commands;
using Bot.config;

class Program
{
    static async Task Main()
    {
        var botSettings = new BotSettings();
        var apiSettings = new ApiSettings();

        var httpClient = new HttpClient();
        var authService = new AuthService(httpClient, apiSettings.URL);
        var registerCommand = new RegisterCommand(authService);
        var handler = new BotUpdateHandler(new TelegramBotClient(botSettings.Token), registerCommand);

        var botClient = new TelegramBotClient(botSettings.Token);
        using var cts = new CancellationTokenSource();

        botClient.StartReceiving(
            updateHandler: handler.HandleUpdateAsync,
            errorHandler: async (bot, exception, source, ct) =>
            {
                Console.WriteLine($"Error: {exception.Message}");
            },
            cancellationToken: cts.Token
        );

        Console.WriteLine("Bot started. Press any key to exit");
        Console.ReadKey();
        cts.Cancel();
    }
}