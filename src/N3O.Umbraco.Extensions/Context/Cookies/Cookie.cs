using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using System.Web;

namespace N3O.Umbraco.Context;

public abstract class Cookie : ICookie {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string _value;
    private bool _deferredReset;

    protected Cookie(IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
    }

    public void DeferredReset() {
        _deferredReset = true;
    }
    
    public string GetValue() {
        if (_value == null) {
            var cookies = _httpContextAccessor.HttpContext?.Request.Cookies;
            var key = cookies?.Keys.SingleOrDefault(x => x.EqualsInvariant(Name));

            if (key != null) {
                _value = HttpUtility.UrlDecode(cookies[key]);
            } else {
                _value = GetDefaultValue();
            }
        }

        return _value;
    }
    
    public void Reset() {
        _value = GetDefaultValue();
    }

    public void SetValue(string value) {
        _value = value;
    }
    
    public void Write(IResponseCookies responseCookies) {
        var value = _deferredReset ? GetDefaultValue() : _value;
        
        if (value.HasValue()) {
            var cookieOptions = new CookieOptions();

            SetOptions(cookieOptions);

            responseCookies.Append(Name, value, cookieOptions);
        }
    }
    
    protected abstract string Name { get; }

    protected virtual TimeSpan Lifetime => TimeSpan.Zero;
    protected virtual bool Session => false;
    protected virtual string GetDefaultValue() => null;

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
