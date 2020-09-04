namespace telegramPokedexBotWebhook
{
    public sealed class Constants
    {
        private Constants() {}
        
        public const string CONFIGURATION_SECTION_KEY_TELEGRAM_BOT = "TelegramBot";
        public const string CONFIGURATION_KEY_API_TOKEN = "ApiToken";
        public const string CONFIGURATION_KEY_WEBHOOK_URL = "WebhookUrl";
    }
}