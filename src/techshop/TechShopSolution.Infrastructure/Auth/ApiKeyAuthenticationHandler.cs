using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using TechShopSolution.Domain.Services;

namespace TechShopSolution.Infrastructure.Auth
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string ApiKeyHeaderName = "X-API-KEY";
        private readonly IVaultService _vaultService;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            IVaultService vaultService,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
            _vaultService = vaultService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
            {
                return AuthenticateResult.NoResult();
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(providedApiKey))
            {
                return AuthenticateResult.NoResult();
            }

            try
            {
                // Lấy API Key từ Vault qua VaultService
                var vaultApiKey = await _vaultService.GetSecretAsync("upgrade-dev/domain1/database/staging/service1", "kv", "api_key");

                if (providedApiKey.Equals(vaultApiKey))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "API Key User"),
                    };

                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
            }
            catch (KeyNotFoundException ex)
            {
                Logger.LogError($"Vault error: {ex.Message}");
                return AuthenticateResult.Fail("API Key not found in Vault.");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Unexpected error: {ex.Message}");
                return AuthenticateResult.Fail("An unexpected error occurred.");
            }

            return AuthenticateResult.Fail("Invalid API Key");
        }
    }
}