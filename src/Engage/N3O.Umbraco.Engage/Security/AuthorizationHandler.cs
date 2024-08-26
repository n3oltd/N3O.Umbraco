﻿using StackExchange.Profiling.Internal;
using System.Net.Http.Headers;

namespace N3O.Umbraco.Engage.Security;

public class AuthorizationHandler : DelegatingHandler {
    private readonly string _bearerToken;
    private readonly string _onBehalfOf;

    public AuthorizationHandler(string bearerToken, string onBehalfOf, HttpMessageHandler innerHandler) {
        _bearerToken = bearerToken;
        _onBehalfOf = onBehalfOf;

        InnerHandler = innerHandler;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken) {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

        if (_onBehalfOf.HasValue()) {
            request.Headers.Add("N3O-OnBehalfOf", _onBehalfOf);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}