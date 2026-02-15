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
// Pruebas completas de la api consumida desde telegram no postman
// Hacer el codigo mas escalable y limpio sin repeticion
/*
COMANDOS A IMPLEMENTAR
user/edit
*/

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
        private readonly ViewMarketCommand _viewMarketCommand;
        private readonly MyCharactersCommand _myCharactersCommand;
        private readonly MyEpisodesCommand _myEpisodesCommand;
        private readonly UserCharactersCommand _userCharactersCommand;
        private readonly UserEpisodesCommand _userEpisodesCommand;
        private readonly UserService _userService;
        private readonly ExtractToken _extractToken;
        private readonly Dictionary<long, string> _userTokens = new();

        public BotUpdateHandler(ITelegramBotClient botClient, RegisterCommand registerCommand, LoginCommand loginCommand, CaptureCharacterCommand captureCharacterCommand,
                                CaptureEpisodeCommand captureEpisodeCommand, PutCharacterForSaleCommand sellCharacterCommand, PutEpisodeForSaleCommand sellEpisodeCommand,
                                BuyCharacterCommand buyCharacterCommand, BuyEpisodeCommand buyEpisodeCommand, ViewMarketCommand viewMarketCommand, MyCharactersCommand myCharactersCommand, 
                                MyEpisodesCommand myEpisodesCommand, UserCharactersCommand userCharactersCommand, UserEpisodesCommand userEpisodesCommand, UserService userService,
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
            _viewMarketCommand = viewMarketCommand;
            _myCharactersCommand = myCharactersCommand;
            _myEpisodesCommand = myEpisodesCommand;
            _userCharactersCommand = userCharactersCommand;
            _userEpisodesCommand = userEpisodesCommand;
            _userService = userService;
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
                        text: "🎮 Available commands:" + Environment.NewLine +
                            "/start - Start the bot" + Environment.NewLine +
                            "/help - Show this help message" + Environment.NewLine +
                            "/register - Register a new user" + Environment.NewLine +
                            "/login - Login to your account" + Environment.NewLine +
                            "/captureCharacter - Capture a random character" + Environment.NewLine +
                            "/captureEpisode - Capture a random episode" + Environment.NewLine +
                            "/sellCharacter - Sell a character you own" + Environment.NewLine +
                            "/sellEpisode - Sell an episode you own" + Environment.NewLine +
                            "/buyCharacter - Buy a character from the market" + Environment.NewLine +
                            "/buyEpisode - Buy an episode from the market" + Environment.NewLine +
                            "/viewMarket - View all available items in the market" + Environment.NewLine +
                            "/myCharacters - View your characters" + Environment.NewLine +
                            "/myEpisodes - View your episodes" + Environment.NewLine +
                            "/charactersUser <username> - View another user's characters" + Environment.NewLine +
                            "/episodesUser <username> - View another user's episodes" + Environment.NewLine +
                            Environment.NewLine +
                            "⚠️ REMINDER: This bot is for demonstration purposes only. Do not share sensitive information. ⚠️",
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
            if (messageText.StartsWith("/buyCharacter"))
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

                var parts = messageText.Split(' ');
                if (parts.Length != 2)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: "🚨 ERROR: Usage: /buyCharacter <itemId> Example: /buyCharacter 1",
                        cancellationToken: cancellationToken
                    );
                    return;
                }

                if (!int.TryParse(parts[1], out int itemId))
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: "🚨 ERROR: The item ID must be a valid integer.",
                        cancellationToken: cancellationToken
                    );
                    return;
                }

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Buying the character... 🛒",
                    cancellationToken: cancellationToken
                );

                try
                {
                    // Pasamos directamente el int
                    var buyCharacter = await _buyCharacterCommand.ExecuteAsync(itemId, userToken);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: buyCharacter.Message,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚨 Error buying the character: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
            if (messageText.StartsWith("/buyEpisode"))
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

                var parts = messageText.Split(' ');
                if (parts.Length != 2)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: "🚨 ERROR: Usage: /buyEpisode <itemId> Example: /buyEpisode 1",
                        cancellationToken: cancellationToken
                    );
                    return;
                }

                if (!int.TryParse(parts[1], out int itemId))
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: "🚨 ERROR: The item ID must be a valid integer.",
                        cancellationToken: cancellationToken
                    );
                    return;
                }

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Buying the episode... 🛒",
                    cancellationToken: cancellationToken
                );

                try
                {
                    // Pasamos directamente el int
                    var buyEpisode = await _buyEpisodeCommand.ExecuteAsync(itemId, userToken);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: buyEpisode.Message,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚨 Error buying the episode: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
            if (messageText == "/viewMarket")
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Fetching market data... 🛍️",
                    cancellationToken: cancellationToken
                );

                try
                {
                    var marketData = await _viewMarketCommand.ExecuteAsync(userToken);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: marketData.Message,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚨 Error fetching market data: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
            if (messageText == "/myCharacters")
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Fetching your characters... 🧠",
                    cancellationToken: cancellationToken
                );

                try
                {
                    var myCharacters = await _myCharactersCommand.ExecuteAsync(userToken);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: myCharacters.Message,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚨 Error fetching your characters: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
            if (messageText == "/myEpisodes")
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Fetching your episodes... 🧠",
                    cancellationToken: cancellationToken
                );

                try
                {
                    var myEpisodes = await _myEpisodesCommand.ExecuteAsync(userToken);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: myEpisodes.Message,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚨 Error fetching your episodes: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
            if (messageText.StartsWith("/charactersUser"))
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Fetching user's characters... 🧠",
                    cancellationToken: cancellationToken
                );

                try
                {
                    var parts = messageText.Split(' ');
                    if (parts.Length != 2)
                    {
                        await botClient.SendMessage(
                        chatId: chatId,
                        text: "🚨 ERROR: Usage: /charactersUser <username> Example: /charactersUser user123",
                        cancellationToken: cancellationToken
                    );
                    return;
                    }
                    var charactersUser = await _userCharactersCommand.ExecuteAsync(userToken, parts[1]);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: charactersUser.Message,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚨 Error fetching user's characters: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
            if (messageText.StartsWith("/episodesUser"))
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Fetching user's episodes... 🧠",
                    cancellationToken: cancellationToken
                );

                try
                {
                    var parts = messageText.Split(' ');
                    if (parts.Length != 2)
                    {
                        await botClient.SendMessage(
                        chatId: chatId,
                        text: "🚨 ERROR: Usage: /episodesUser <username> Example: /episodesUser user123",
                        cancellationToken: cancellationToken
                    );
                    return;
                    }
                    var episodesUser = await _userEpisodesCommand.ExecuteAsync(userToken, parts[1]);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: episodesUser.Message,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚨 Error fetching user's episodes: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
            if (messageText == "/myInfo")
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Fetching your user info... 🧠",
                    cancellationToken: cancellationToken
                );

                try
                {
                    var userInfo = await _userService.GetMyUserInfo(userToken);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: userInfo,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚨 Error fetching your user info: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
            if (messageText == "/work")
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Working... 🧠",
                    cancellationToken: cancellationToken
                );

                try
                {
                    var userInfo = await _userService.Work(userToken);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: userInfo,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚨 Error working: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
            if (messageText.StartsWith("/userInfo"))
            {
                var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
                if (string.IsNullOrEmpty(userToken)) return;

                var parts = messageText.Split(' ');
                if (parts.Length != 2)
                {
                    await botClient.SendMessage(
                    chatId: chatId,
                    text: "🚨 ERROR: Usage: userInfo <username> Example: userInfo user123",
                    cancellationToken: cancellationToken
                );
                return;
                }

                await botClient.SendMessage(
                    chatId: chatId,
                    text: $"Fetching {parts[1]} user info... 🧠",
                    cancellationToken: cancellationToken
                );

                try
                {
                    var userInfo = await _userService.GetUserInfo(userToken, parts[1]);
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: userInfo,
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚨 Error fetching user info: {ex.Message}",
                        cancellationToken: cancellationToken
                    );
                }
            }
        }
    }
}
