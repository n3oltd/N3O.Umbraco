﻿using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Authentication.Auth0.Clients;

public interface IAuth0TokenClient {
    [Post("/oauth/token")]
    Task<OAuthTokenRes> GetOAuthTokenAsync(OAuthTokenReq req);
}
