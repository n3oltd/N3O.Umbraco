using Microsoft.AspNetCore.Http;
using System;

namespace N3O.Umbraco.Extensions {
    public static class HttpRequestExtensions {
        public static Uri Uri(this HttpRequest request) {
            var builder = new UriBuilder();
            builder.Scheme = request.Scheme;
            builder.Host = request.Host.Host;
            builder.Path = request.Path;
            builder.Query = request.QueryString.ToUriComponent();

            if (request.Host.Port.HasValue()) {
                builder.Port = request.Host.Port.GetValueOrThrow();
            }

            return builder.Uri;
        }
    }
}