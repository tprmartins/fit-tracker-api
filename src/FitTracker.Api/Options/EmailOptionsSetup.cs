using Microsoft.Extensions.Options;

namespace FitTracker.Api.Options
{
    public class EmailOptionsSetup : IConfigureOptions<EmailOptions>
    {
        private const string ConfigurationSectionName = "EmailOptions";
        private readonly IConfiguration _configuration;

        public EmailOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(EmailOptions options)
        {
            _configuration.GetSection(ConfigurationSectionName).Bind(options);
        }
    }
}
