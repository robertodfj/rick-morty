using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Bot.commands;
using Bot.service;
using Bot.model.request;
using Bot.token;

/*
TAREAS RESTANTES
user/edit
    Hacer bonito la respuesta para que no quede asi: Login failed: {"message":"Invalid username or password"}
    Pruebas completas de la api consumida desde telegram no postman
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

        // Enviar comandos (Realizado para evitar repeticion de codigo y hacerlo mas limpio)
        public async Task SendCommandAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken, Func<string, Task<string>> action, string processingMessage = "Processing...")
        {
            var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
            if (string.IsNullOrEmpty(userToken)) return;

            await botClient.SendMessage(
                chatId: chatId,
                text: processingMessage,
                cancellationToken: cancellationToken
            );

            try
            {
                var data = await action(userToken);
                await botClient.SendMessage(
                    chatId: chatId,
                    text: data,
                    cancellationToken: cancellationToken
                );
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(
                    chatId: chatId,
                    text: $"🚨 Error fetching data: {ex.Message}",
                    cancellationToken: cancellationToken
                );
            }
        }
        public async Task SendCommandWithParmsAsync<T>(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken, Func<string, T, Task<string>> action, T parameter, string processingMessage = "Processing...")
        {
            var userToken = await VerifyUserToken(chatId, botClient, cancellationToken);
            if (string.IsNullOrEmpty(userToken)) return;

            await botClient.SendMessage(
                chatId: chatId,
                text: processingMessage,
                cancellationToken: cancellationToken
            );

            try
            {
                var data = await action(userToken, parameter);
                await botClient.SendMessage(
                    chatId: chatId,
                    text: data,
                    cancellationToken: cancellationToken
                );
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(
                    chatId: chatId,
                    text: $"🚨 Error fetching data: {ex.Message}",
                    cancellationToken: cancellationToken
                );
            }
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
                            "cancelCharacterSell - Cancel the sale of a character you have for sale" + Environment.NewLine +
                            "cancelEpisodeSell - Cancel the sale of an episode you have for sale" + Environment.NewLine +
                            "/buyCharacter - Buy a character from the market" + Environment.NewLine +
                            "/buyEpisode - Buy an episode from the market" + Environment.NewLine +
                            "/viewMarket - View all available items in the market" + Environment.NewLine +
                            "/myCharacters - View your characters" + Environment.NewLine +
                            "/myEpisodes - View your episodes" + Environment.NewLine +
                            "/charactersUser <username> - View another user's characters" + Environment.NewLine +
                            "/episodesUser <username> - View another user's episodes" + Environment.NewLine +
                            "/myInfo - View your user info" + Environment.NewLine +
                            "/userInfo <username> - View another user's info" + Environment.NewLine +
                            "/work - Work to earn money & more oportunities to capture items" + Environment.NewLine +
                            "/editUsername <newUsername> - Edit your username" +
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
                await SendCommandAsync(chatId, botClient, cancellationToken, async (userToken) =>
                {
                    var captureCharacter = await _captureCharacterCommand.ExecuteAsync(userToken);
                    return captureCharacter.Message;
                }, "Capturing character...");
            }
            if (messageText == "/captureEpisode")
            {
                await SendCommandAsync(chatId, botClient, cancellationToken, async (userToken) =>
                {
                    var captureEpisode = await _captureEpisodeCommand.ExecuteAsync(userToken);
                    return captureEpisode.Message;
                }, "Capturing episode...");
            }
            if (messageText.StartsWith("/sellCharacter"))
            {

                var parts = messageText.Split(' ');
                if (parts.Length != 3)
                {
                    await botClient.SendMessage(
                    chatId: chatId,
                    text: "🚨 ERROR: Usage: /sellCharacter <itemID> <price> Example: /sellCharacter 1 100",
                    cancellationToken: cancellationToken
                );
                    return;
                }

                var itemForSaleRequest = new ItemForSaleRequest
                {
                    Price = int.Parse(parts[2]),
                    ItemID = int.Parse(parts[1])
                };

                await SendCommandWithParmsAsync(chatId, botClient, cancellationToken, async (userToken, request) =>
                {
                    var sellCharacter = await _sellCharacterCommand.ExecuteAsync(request, userToken);
                    return sellCharacter.Message;
                }, itemForSaleRequest, "Putting the character for sale... 🤑...");


            }
            if (messageText.StartsWith("/sellEpisode"))
            {

                var parts = messageText.Split(' ');
                if (parts.Length != 3)
                {
                    await botClient.SendMessage(
                    chatId: chatId,
                    text: "🚨 ERROR: Usage: /sellEpisode <itemID> <price> Example: /sellEpisode 1 100",
                    cancellationToken: cancellationToken
                );
                }

                var itemForSaleRequest = new ItemForSaleRequest
                {
                    Price = int.Parse(parts[2]),
                    ItemID = int.Parse(parts[1])
                };

                await SendCommandWithParmsAsync(chatId, botClient, cancellationToken, async (userToken, request) =>
                {
                    var sellEpisode = await _sellEpisodeCommand.ExecuteAsync(request, userToken);
                    return sellEpisode.Message;
                }, itemForSaleRequest, "Putting the episode for sale... 🤑...");
            }
            if (messageText.StartsWith("/buyCharacter"))
            {

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

                await SendCommandWithParmsAsync(chatId, botClient, cancellationToken, async (userToken, id) =>
                {
                    var buyCharacter = await _buyCharacterCommand.ExecuteAsync(id, userToken);
                    return buyCharacter.Message;
                }, itemId, "Buying the character... 🛒...");
            }
            if (messageText.StartsWith("/buyEpisode"))
            {

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

                await SendCommandWithParmsAsync(chatId, botClient, cancellationToken, async (userToken, id) =>
                {
                    var buyEpisode = await _buyEpisodeCommand.ExecuteAsync(id, userToken);
                    return buyEpisode.Message;
                }, itemId, "Buying the episode... 🛒...");
            }
            if (messageText.StartsWith("/cancelCharacterSell"))
            {
                var parts = messageText.Split(' ');
                if (parts.Length != 2)
                {
                    await botClient.SendMessage(
                    chatId: chatId,
                    text: "🚨 ERROR: Usage: /cancelCharacterSell <characterId> Example: /cancelCharacterSell 1",
                    cancellationToken: cancellationToken
                );
                    return;
                }

                if (!int.TryParse(parts[1], out int characterId))
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: "🚨 ERROR: The character ID must be a valid integer.",
                        cancellationToken: cancellationToken
                    );
                    return;
                }

                await SendCommandWithParmsAsync(chatId, botClient, cancellationToken, async (userToken, id) =>
                {
                    var cancelSell = await _sellCharacterCommand.CancelAsync(id, userToken);
                    return cancelSell.Message;
                }, characterId, "Canceling character sale... 🛑...");

            }
            if (messageText.StartsWith("/cancelEpisodeSell"))
            {
                var parts = messageText.Split(' ');
                if (parts.Length != 2)
                {
                    await botClient.SendMessage(
                    chatId: chatId,
                    text: "🚨 ERROR: Usage: /cancelEpisodeSell <episodeId> Example: /cancelEpisodeSell 1",
                    cancellationToken: cancellationToken
                );
                    return;
                }

                if (!int.TryParse(parts[1], out int episodeId))
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: "🚨 ERROR: The episode ID must be a valid integer.",
                        cancellationToken: cancellationToken
                    );
                    return;
                }

                await SendCommandWithParmsAsync(chatId, botClient, cancellationToken, async (userToken, id) =>
                {
                    var cancelSell = await _sellEpisodeCommand.CancelAsync(id, userToken);
                    return cancelSell.Message;
                }, episodeId, "Canceling episode sale... 🛑...");

            }
            if (messageText == "/viewMarket")
            {
                await SendCommandAsync(chatId, botClient, cancellationToken, async (userToken) =>
                {
                    var marketItems = await _viewMarketCommand.ExecuteAsync(userToken);
                    return marketItems.Message;
                }, "Fetching market items... 🛒");
            }
            if (messageText == "/myCharacters")
            {
                await SendCommandAsync(chatId, botClient, cancellationToken, async (userToken) =>
                {
                    var myCharacters = await _myCharactersCommand.ExecuteAsync(userToken);
                    return myCharacters.Message;
                }, "Fetching your characters... 🧠");
            }
            if (messageText == "/myEpisodes")
            {
                await SendCommandAsync(chatId, botClient, cancellationToken, async (userToken) =>
                {
                    var myEpisodes = await _myEpisodesCommand.ExecuteAsync(userToken);
                    return myEpisodes.Message;
                }, "Fetching your episodes... 🧠");
            }
            if (messageText.StartsWith("/charactersUser"))
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

               await SendCommandWithParmsAsync(chatId, botClient, cancellationToken, async (userToken, username) =>
                {
                    var charactersUser = await _userCharactersCommand.ExecuteAsync(userToken, username);
                    return charactersUser.Message;
                }, parts[1], "Fetching user's characters... 🧠...");
            }
            if (messageText.StartsWith("/episodesUser"))
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

                await SendCommandWithParmsAsync(chatId, botClient, cancellationToken, async (userToken, username) =>
                {
                    var episodesUser = await _userEpisodesCommand.ExecuteAsync(userToken, username);
                    return episodesUser.Message;
                }, parts[1], "Fetching user's episodes... 🧠...");
            }
            if (messageText == "/myInfo")
            {
                await SendCommandAsync(chatId, botClient, cancellationToken, async (userToken) =>
                {
                    var userInfo = await _userService.GetMyUserInfo(userToken);
                    return userInfo;
                }, "Fetching your user info... 🧠");
            }
            if (messageText == "/work")
            {
                await SendCommandAsync(chatId, botClient, cancellationToken, async (userToken) =>
                {
                    var workResult = await _userService.Work(userToken);
                    return workResult;
                }, "Working... 💼");
            }
            if (messageText.StartsWith("/userInfo"))
            {

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

                await SendCommandWithParmsAsync(chatId, botClient, cancellationToken, async (userToken, username) =>
                {
                    var userInfo = await _userService.GetUserInfo(userToken, username);
                    return userInfo;
                }, parts[1], "Fetching user info... 🧠...");
            }
            if (messageText.StartsWith("/editUsername"))
            {
                var parts = messageText.Split(' ');
                if (parts.Length != 2)
                {
                    await botClient.SendMessage(
                    chatId: chatId,
                    text: "🚨 ERROR: Usage: /editUsername <newUsername> Example: /editUsername newuser123",
                    cancellationToken: cancellationToken
                );
                    return;
                }

                await SendCommandWithParmsAsync(chatId, botClient, cancellationToken, async (userToken, newUsername) =>
                {
                    var result = await _userService.EditUsername(userToken, newUsername);
                    return result;
                }, parts[1], "Editing username... 🧠...");
            }
        }
    }
}
