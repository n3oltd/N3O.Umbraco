using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Context;

public abstract class Cookie : ReadOnlyCookie, ICookie {
    private bool _deferredReset;

    protected Cookie(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

    public void DeferredReset() {
        _deferredReset = true;
    }
    
    public void Reset() {
        Value = GetDefaultValue();
    }

    public void SetValue(string value) {
        Value = value;
    }
    
    public void Write(IResponseCookies responseCookies) {
        var value = _deferredReset ? GetDefaultValue() : Value;
        
        if (value.HasValue()) {
            var cookieOptions = new CookieOptions();

            SetOptions(cookieOptions);

            responseCookies.Append(Name, value, cookieOptions);
        }
    }

    protected virtual TimeSpan Lifetime => TimeSpan.Zero;
    protected virtual bool Session => false;

    protected virtual void SetOptions(CookieOptions cookieOptions) {
        cookieOptions.Path = "/";
        cookieOptions.IsEssential = true;
        cookieOptions.HttpOnly = true;
        cookieOptions.Secure = true;

        if (!Session) {
            if (Lifetime == TimeSpan.Zero) {
                throw new Exception("Lifetime must be specified for non-session cookies");
            }
            
            cookieOptions.Expires = DateTimeOffset.UtcNow.Add(Lifetime);
        }
    }
}
