using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Bot.commands;
using Bot.service;
using Bot.model.request;
using Bot.token;

//TAREAS RESTANTES
// Hacer bonito la respuesta para que no quede asi: Login failed: {"message":"Invalid username or password"}
// Guardar el token en memoria para usarlo en los siguientes comandos (my-info, user-info, edit-user) y asi no tener que loguear cada vez
// Loguear automaticamente cada hora para renovar el token (si es que expira a la hora, sino ajustar el tiempo)


namespace Bot.handler
{
    public class BotUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly RegisterCommand _registerCommand;
        private readonly LoginCommand _loginCommand;
        private readonly CaptureCharacterCommand _captureCharacterCommand;
        private readonly CaptureEpisodeCommand _captureEpisodeCommand;
        private readonly ExtractToken _extractToken;
        private readonly Dictionary<long, string> _userTokens = new();

        public BotUpdateHandler(ITelegramBotClient botClient, RegisterCommand registerCommand, LoginCommand loginCommand, CaptureCharacterCommand captureCharacterCommand, 
                                CaptureEpisodeCommand captureEpisodeCommand, ExtractToken extractToken, Dictionary<long, string> userTokens)
        {
            _botClient = botClient;
            _registerCommand = registerCommand;
            _loginCommand = loginCommand;
            _captureCharacterCommand = captureCharacterCommand;
            _captureEpisodeCommand = captureEpisodeCommand;
            _extractToken = extractToken;
            _userTokens = userTokens;
        }

        // Usar y guardar tokens
        public void SaveUserToken(long userId, string token)
        {
            _userTokens[userId] = token;
        }

        public string GetUserToken(long userId)
        {
            return _userTokens.TryGetValue(userId, out var token) ? token : string.Empty;
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
                          "/myInfo - Show your user information" + Environment.NewLine +
                          "/userInfo - Show the user information" + Environment.NewLine +
                          "/editUser - Edit your user information" + Environment.NewLine +
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
                        // Guardamos el token despues de extraerlo de la respuesta del login
                        SaveUserToken(chatId, _extractToken.GetTokenFromResponse(loginResult.Message));

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
            if (messageText == "/captureCharacter")
            {
                var userToken = GetUserToken(chatId);
                if (string.IsNullOrEmpty(userToken))
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: "You must be logged in to capture a character.",
                        cancellationToken: cancellationToken
                    );
                    return;
                }

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Capturing character...",
                    cancellationToken: cancellationToken
                );
                try
                {
                    var captureCharacter = await _captureCharacterCommand.ExecuteAsync(userToken);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: captureCharacter.Message,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"Error capturing character: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                    throw;
                }
            }
            if (messageText == "/captureEpisode")
            {
                var userToken = GetUserToken(chatId);
                if (string.IsNullOrEmpty(userToken))
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: "You must be logged in to capture an episode.",
                        cancellationToken: cancellationToken
                    );
                    return;
                }

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Capturing episode...",
                    cancellationToken: cancellationToken
                );
                try
                {
                    var captureEpisode = await _captureEpisodeCommand.ExecuteAsync(userToken);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: captureEpisode.Message,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"Error capturing episode: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                    throw;
                }
            }
        }
    }
}