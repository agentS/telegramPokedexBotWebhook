using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PokeApiNet;
using Telegram.Bot;

namespace eu.mauerkids.pokedexbot.bot
{
    public static class StartupExtensions
    {
        public static void AddPokeApiClient(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<PokeApiClient>();
        }

        public static void AddTelegramBot(this IServiceCollection serviceCollection, string apiToken)
        {
            serviceCollection.AddSingleton<ITelegramBotClient, TelegramBotClient>(
                options => new TelegramBotClient(apiToken)
            );
        }
    }
}
