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

        public BotUpdateHandler(ITelegramBotClient botClient, RegisterCommand registerCommand)
        {
            _botClient = botClient;
            _registerCommand = registerCommand;
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
                    text: "Welcome to the Rick and Morty Bot!" + Environment.NewLine +
                          "Use /register to create an account or /login to access your account." + Environment.NewLine +
                          "Use /help to see all available commands." + Environment.NewLine +
                          "REMINDER: This bot is for demonstration purposes only. Do not share sensitive information.",
                    cancellationToken: cancellationToken
                );
                return;
            }

            if (messageText == "/register")
            {
                var message = "🚨 REMINDER: This bot is for demonstration purposes only. Do not share sensitive information." + Environment.NewLine +
                              "Welcome to the registration process! Please provide your details in the following format:" + Environment.NewLine +
                              "Username: your_username" + Environment.NewLine +
                              "Email: your_email@example.com" + Environment.NewLine +
                              "Password: your_secure_password" + Environment.NewLine +
                              "Confirm Password: your_secure_password" + Environment.NewLine +
                              Environment.NewLine +
                              "First step: enter your username.";

                await botClient.SendMessage(
                    chatId: chatId,
                    text: message,
                    cancellationToken: cancellationToken
                );
                return;
            }
        }
    }
}