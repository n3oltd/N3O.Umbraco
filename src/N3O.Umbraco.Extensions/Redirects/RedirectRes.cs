using System;

namespace N3O.Umbraco.Redirects;

public class RedirectRes {
    public string Url { get; set; }

    public static RedirectRes For(Uri url) {
        return new RedirectRes {
            Url = url.AbsoluteUri
        };
    }
    
    public static RedirectRes For(string url) {
        return new RedirectRes {
            Url = url
        };
    }
}
