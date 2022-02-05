using eu.mauerkids.pokedexbot.bot;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace telegramPokedexBotWebhook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            var telegramBotConfigurationSection = this.Configuration.GetSection(Constants.CONFIGURATION_SECTION_KEY_TELEGRAM_BOT);
            this._telegramBotApiToken = telegramBotConfigurationSection.GetValue<string>(Constants.CONFIGURATION_KEY_API_TOKEN);
        }

        public IConfiguration Configuration { get; }
        private readonly string _telegramBotApiToken;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddPokeApiClient();
            services.AddTelegramBot(this._telegramBotApiToken);
            services.AddHostedService<TelegramBotHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
