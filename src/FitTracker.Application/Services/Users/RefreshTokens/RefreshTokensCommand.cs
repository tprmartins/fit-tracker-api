using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Users.RefreshTokens
{
    public record class RefreshTokensCommand : ICommand<RefreshTokensResponse>
    {
        public string Token { get; init; } = default!;
        public string ProvidedRefreshToken { get; init; } = default!;

        public RefreshTokensCommand(string token, string providedRefreshToken)
        {
            Token = token;
            ProvidedRefreshToken = providedRefreshToken;
        }
    }
}
