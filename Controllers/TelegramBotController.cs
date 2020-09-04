using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eu.mauerkids.pokedexbot.bot;
using eu.mauerkids.pokedexbot.bot.command;
using Microsoft.AspNetCore.Mvc;
using PokeApiNet;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace telegramPokedexBotWebhook.Controllers
{
    [ApiController]
    [Route("webhook")]
    public class TelegramBotController : Controller
    {
        private static readonly Dictionary<String, PokedexBotCommand> COMMAND_NAMES = new Dictionary<string, PokedexBotCommand>()
        {
            {"/battle", PokedexBotCommand.BattleStatistics}
        };

        private static string ExtractCommand(string message)
        {
            int commandSeparatorIndex = message.IndexOf(' ');
            if (commandSeparatorIndex == (-1))
            {
                if (message[0] == '/')
                {
                    return message;
                }
                throw new UnknownCommandException("Blank command");
            }
            else
            {
                string commandString = message.Substring(0, commandSeparatorIndex);
                return commandString;
            }
        }
        
        private ITelegramBotClient _botClient;
        private readonly PokeApiClient _pokeApiClient;

        public TelegramBotController(ITelegramBotClient botClient, PokeApiClient pokeApiClient)
        {
            _botClient = botClient;
            _pokeApiClient = pokeApiClient;
        }

        [HttpPost("")]
        public async Task HandleWebHookMessage(Update update)
        {
            var message = update.Message;
            if (message.Text != null)
            {
                try
                {
                    string command = ExtractCommand(message.Text);
                    IBotCommand handler = CommandSelector.MapCommandToHandler(command);
                    await handler.Handle(message, this._botClient, this._pokeApiClient);
                }
                catch (Exception exception)
                {
                    // Workaround since C# does not support multi catch as in Java
                    if (exception is UnknownCommandException || exception is NoCommandHandlerException)
                    {
                        await this._botClient.SendTextMessageAsync(
                            chatId: message.Chat,
                            text: "Unfortunately the bot does not support this command. Enter /help or /start for more options."
                        );
                    }
                    else
                    {
                        Console.WriteLine(exception.Message);
                        Console.WriteLine(exception.StackTrace);
                        await this._botClient.SendTextMessageAsync(
                            chatId: message.Chat,
                            text: "500 - Internal Server Error"
                        );
                    }
                }
            }
        }
    }
}