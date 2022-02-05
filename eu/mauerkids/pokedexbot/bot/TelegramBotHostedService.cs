using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using telegramPokedexBotWebhook;

namespace eu.mauerkids.pokedexbot.bot
{
    public class TelegramBotHostedService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IConfiguration _configuration;
        private readonly string _webhookUrl;

        public TelegramBotHostedService(
            ITelegramBotClient botClient,
            IConfiguration configuration
        )
        {
            this._botClient = botClient;
            this._webhookUrl = configuration.GetSection(Constants.CONFIGURATION_SECTION_KEY_TELEGRAM_BOT)
                .GetValue<string>(Constants.CONFIGURATION_KEY_WEBHOOK_URL);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this._botClient.SetWebhookAsync(
                this._webhookUrl,
                cancellationToken: cancellationToken
            );
            Console.WriteLine($"Registered the webhook at {this._webhookUrl}");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await this._botClient.DeleteWebhookAsync(dropPendingUpdates: true, cancellationToken);
            Console.WriteLine("Deleted the webhook.");
        }
    }
}