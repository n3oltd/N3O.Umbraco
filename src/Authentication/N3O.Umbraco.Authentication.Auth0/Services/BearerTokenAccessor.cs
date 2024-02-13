using Microsoft.Extensions.Options;
using N3O.Umbraco.Authentication.Auth0.Options;
using System.Threading.Tasks;

namespace N3O.Umbraco.Authentication.Auth0;

public class BearerTokenAccessor {
    private readonly Auth0M2MTokenAccessor _auth0M2MTokenAccessor;
    private readonly Auth0AuthenticationOptions _auth0Options;

    public BearerTokenAccessor(Auth0M2MTokenAccessor auth0M2MTokenAccessor,
                               IOptions<Auth0AuthenticationOptions> auth0Options) {
        _auth0M2MTokenAccessor = auth0M2MTokenAccessor;
        _auth0Options = auth0Options.Value;
    }

    public async Task<string> GetAsync() {
        var token = await _auth0M2MTokenAccessor.GetTokenAsync(_auth0Options.M2M, _auth0Options.M2M.ApiIdentifier);
        
        return token;
    }
}
