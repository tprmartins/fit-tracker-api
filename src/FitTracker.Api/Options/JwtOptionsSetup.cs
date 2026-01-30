using Microsoft.Extensions.Options;
using FitTracker.Infra.Authentication;

namespace FitTracker.Api.Options
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private readonly string SectionName = "Jwt";
        private readonly IConfiguration _configuration;

        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options)
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
