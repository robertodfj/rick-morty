using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Bot.commands;
using Bot.service;
using Bot.model.request;

namespace Bot.handler
{
    public class BotUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly RegisterCommand _registerCommand;
        private readonly LoginCommand _loginCommand;
        private readonly AuthService _authService;

        public BotUpdateHandler(ITelegramBotClient botClient, RegisterCommand registerCommand, LoginCommand loginCommand, AuthService authService)
        {
            _botClient = botClient;
            _registerCommand = registerCommand;
            _loginCommand = loginCommand;
            _authService = authService;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update == null || update.Message == null || string.IsNullOrEmpty(update.Message.Text))
                return;

            var messageText = update.Message.Text.Trim();
            var chatId = update.Message.Chat.Id;

            if (messageText == "/start")
            {
                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Welcome to the Rick and Morty Mini Game 🧪!" + Environment.NewLine +
                          "Use /help to see all available commands." + Environment.NewLine +
                          "REMINDER: This bot is for demonstration purposes only. Do not share sensitive information.",
                    cancellationToken: cancellationToken
                );
                return;
            }
            if (messageText == "/help")
            {
                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Available commands:" + Environment.NewLine +
                          "/start - Start the bot" + Environment.NewLine +
                          "/help - Show this help message" + Environment.NewLine +
                          "/register - Register a new user" + Environment.NewLine +
                          "/login - Login to your account" + Environment.NewLine +
                          "/my-info - Show your user information" + Environment.NewLine +
                          "/user-info - Show the user information" + Environment.NewLine +
                          "/edit-user - Edit your user information" + Environment.NewLine +
                          Environment.NewLine +
                            "REMINDER: This bot is for demonstration purposes only. Do not share sensitive information.",
                    cancellationToken: cancellationToken
                );
                return;
            }
            if (messageText == "/register")
            {
                string email = (update.Message.From?.FirstName ?? "") + (update.Message.From?.LastName ?? "") + "@example.com";
                string username = update.Message.From?.Id.ToString() ?? string.Empty;
                if (username.Length > 20) username = username.Substring(0, 20);
                string password = $"TgBotPwd{update.Message.From?.Id}";

                try
                {
                    var registerRequest = new RegisterRequest
                    {
                        Email = email,
                        Username = username,
                        Password = password,
                        ConfirmPassword = password
                    };

                    var registrationResult = await _registerCommand.ExecuteAsync(registerRequest);

                    if (registrationResult.Success)
                    {
                        await botClient.SendMessage(
                            chatId: chatId,
                            text: $"Registration successful! Welcome, {username}!",
                            cancellationToken: cancellationToken
                        );
                    }
                    else
                    {
                        await botClient.SendMessage(
                            chatId: chatId,
                            text: $"Registration failed: {registrationResult.Message}",
                            cancellationToken: cancellationToken
                        );
                    }
                }
                catch (Exception ex)
                {
                    // Aquí puedes personalizar según el tipo de excepción si quieres
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"Error: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
            if (messageText == "/login")
            {
                string username = update.Message.From?.Id.ToString() ?? string.Empty;
                if (username.Length > 20) username = username.Substring(0, 20);
                string password = $"TgBotPwd{update.Message.From?.Id}";
                try
                {
                    var loginRequest = new LoginRequest
                    {
                        Username = username,
                        Password = password,
                    };

                    var loginResult = await _loginCommand.ExecuteAsync(loginRequest);
                    
                    if (loginResult.Success)
                    {
                        await botClient.SendMessage(
                            chatId: chatId,
                            text: $"Login successful! Welcome back, {username}!",
                            cancellationToken: cancellationToken
                        );
                    }
                    else
                    {
                        await botClient.SendMessage(
                            chatId: chatId,
                            text: $"Login failed: {loginResult.Message}",
                            cancellationToken: cancellationToken
                        );
                    }
                }
                catch (System.Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"Error during login: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }

        }
    }
}