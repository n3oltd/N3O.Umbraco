using Microsoft.Extensions.Caching.Memory;
using N3O.Umbraco.Authentication.Auth0.Clients;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Authentication.Auth0;

public class Auth0M2MTokenAccessor {
    private static readonly MemoryCache Tokens = new(new MemoryCacheOptions());
    
    private readonly IAuth0Client _apiClient;

    public Auth0M2MTokenAccessor(IAuth0Client apiClient) {
        _apiClient = apiClient;
    }

    public async Task<string> GetTokenAsync(string audience, string clientId, string clientSecret) {
        var cacheKey = $"{nameof(Auth0M2MTokenAccessor)}{nameof(GetTokenAsync)}{audience}{clientId}{clientSecret}";

        var token = await Tokens.GetOrCreateAsync(cacheKey, async cacheEntry => {
            var req = new OAuthTokenReq();

            req.Audience = audience;
            req.GrantType = "client_credentials";
            req.ClientId = clientId;
            req.ClientSecret = clientSecret;

            var res = await _apiClient.GetOAuthTokenAsync(req);

            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(res.ExpiresIn - 30);

            return res.AccessToken;
        });

        return token;
    }
}
