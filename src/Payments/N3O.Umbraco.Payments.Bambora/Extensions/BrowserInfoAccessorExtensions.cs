using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Bambora.Client;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Bambora.Extensions {
    public static class BrowserInfoAccessorExtensions {
        public static BrowserReq GetBrowserReq(this IBrowserInfoAccessor browserInfoAccessor,
                                               BrowserParametersReq parameters) {
            var req = new BrowserReq();
            req.AcceptHeader = browserInfoAccessor.GetAccept();
            req.JavascriptEnabled = parameters.JavaScriptEnabled.GetValueOrThrow();
            req.JavaEnabled = parameters.JavaEnabled.GetValueOrThrow();
            req.Language = browserInfoAccessor.GetLanguage();
            req.ScreenHeight = parameters.ScreenHeight.GetValueOrThrow();
            req.ScreenWidth = parameters.ScreenWidth.GetValueOrThrow();
            req.UserAgent = browserInfoAccessor.GetUserAgent();
            req.ColorDepth = parameters.GetColourDepth().ToString();
            req.TimeZone = parameters.UtcOffsetMinutes.GetValueOrThrow();

            return req;
        }
    }
}