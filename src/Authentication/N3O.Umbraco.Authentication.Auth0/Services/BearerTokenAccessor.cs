using Microsoft.Extensions.Options;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Authentication.Auth0.Options;
using N3O.Umbraco.Exceptions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Authentication.Auth0;

public class BearerTokenAccessor {
    private readonly Auth0M2MTokenAccessor _auth0M2MTokenAccessor;
    private readonly AuthenticationOptions _auth0Options;

    public BearerTokenAccessor(Auth0M2MTokenAccessor auth0M2MTokenAccessor,
                               IOptions<AuthenticationOptions> auth0Options) {
        _auth0M2MTokenAccessor = auth0M2MTokenAccessor;
        _auth0Options = auth0Options.Value;
    }

    public async Task<string> GetAsync(ClientType clientType) {
        var m2MOptions = GetM2MOptions(clientType);
        
        var token = await _auth0M2MTokenAccessor.GetTokenAsync(m2MOptions, m2MOptions.ApiIdentifier);
        
        return token;
    }

    private M2MOptions GetM2MOptions(ClientType clientType) {
        if (clientType == ClientTypes.BackOffice) {
            return _auth0Options.BackOffice.Auth0.M2M;
        } else if (clientType == ClientTypes.Members) {
            return _auth0Options.Members.Auth0.M2M;
        } else {
            throw UnrecognisedValueException.For(clientType);
        }
    }
}
