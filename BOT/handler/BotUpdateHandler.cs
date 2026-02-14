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
// Loguear automaticamente cada hora para renovar el token (si es que expira a la hora, sino ajustar el tiempo)
// Hacer que se pueda editar el username
// Hacer que se pueda quitar el personaje de en venta


namespace Bot.handler
{
    public class BotUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly RegisterCommand _registerCommand;
        private readonly LoginCommand _loginCommand;
        private readonly CaptureCharacterCommand _captureCharacterCommand;
        private readonly CaptureEpisodeCommand _captureEpisodeCommand;
        private readonly PutCharacterForSaleCommand _sellCharacterCommand;
        private readonly PutEpisodeForSaleCommand _sellEpisodeCommand;
        private readonly BuyCharacterCommand _buyCharacterCommand;
        private readonly BuyEpisodeCommand _buyEpisodeCommand;
        private readonly ExtractToken _extractToken;
        private readonly Dictionary<long, string> _userTokens = new();

        public BotUpdateHandler(ITelegramBotClient botClient, RegisterCommand registerCommand, LoginCommand loginCommand, CaptureCharacterCommand captureCharacterCommand,
                                CaptureEpisodeCommand captureEpisodeCommand, PutCharacterForSaleCommand sellCharacterCommand, PutEpisodeForSaleCommand sellEpisodeCommand,
                                BuyCharacterCommand buyCharacterCommand, BuyEpisodeCommand buyEpisodeCommand,
                                ExtractToken extractToken, Dictionary<long, string> userTokens)
        {
            _botClient = botClient;
            _registerCommand = registerCommand;
            _loginCommand = loginCommand;
            _captureCharacterCommand = captureCharacterCommand;
            _captureEpisodeCommand = captureEpisodeCommand;
            _sellCharacterCommand = sellCharacterCommand;
            _sellEpisodeCommand = sellEpisodeCommand;
            _buyCharacterCommand = buyCharacterCommand;
            _buyEpisodeCommand = buyEpisodeCommand;
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

        // Verificar el token
        private async Task<string?> VerifyUserToken(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            var userToken = GetUserToken(chatId);

            if (string.IsNullOrEmpty(userToken))
            {
                await botClient.SendMessage(
                    chatId: chatId,
                    text: "🚨⚠️ You must be logged in to perform this action.",
                    cancellationToken: cancellationToken
                );

                return null;
            }

            return userToken;
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
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

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
                }
            }
            if (messageText == "/captureEpisode")
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

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
                        text: $"🚨 Error capturing episode: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
            if (messageText.StartsWith("/sellCharacter"))
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

                var parts = messageText.Split(' ');
                if (parts.Length != 3)
                {
                    await botClient.SendMessage(
                    chatId: chatId,
                    text: "🚨 ERROR: Usage: /sellCharacter <itemID> <price> Example: /sellCharacter 1 100",
                    cancellationToken: cancellationToken
                );
                }

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Putting the character for sale... 🤑...",
                    cancellationToken: cancellationToken
                );
                try
                {
                    var itemForSaleRequest = new ItemForSaleRequest
                    {
                        Price = int.Parse(parts[2]),
                        ItemID = int.Parse(parts[1])
                    };
                    var sellCharacter = await _sellCharacterCommand.ExecuteAsync(itemForSaleRequest, userToken);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: sellCharacter.Message,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚨 Error putting the character for sale: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
            if (messageText.StartsWith("/sellEpisode"))
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

                var parts = messageText.Split(' ');
                if (parts.Length != 3)
                {
                    await botClient.SendMessage(
                    chatId: chatId,
                    text: "🚨 ERROR: Usage: /sellEpisode <itemID> <price> Example: /sellEpisode 1 100",
                    cancellationToken: cancellationToken
                );
                }

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Putting the episode for sale... 🤑",
                    cancellationToken: cancellationToken
                );
                try
                {
                    var itemForSaleRequest = new ItemForSaleRequest
                    {
                        Price = int.Parse(parts[2]),
                        ItemID = int.Parse(parts[1])
                    };
                    var sellEpisode = await _sellEpisodeCommand.ExecuteAsync(itemForSaleRequest, userToken);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: sellEpisode.Message,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚨 Error putting the episode for sale: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
        }
    }
}
