using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;

namespace N3O.Umbraco.Context {
    public abstract class Cookie : ICookie {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _value;

        protected Cookie(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }
    
        public string GetValue() {
            if (_value == null) {
                var cookies = _httpContextAccessor.HttpContext?.Request.Cookies;
                var key = cookies?.Keys.SingleOrDefault(x => x.EqualsInvariant(Name));

                if (key != null) {
                    _value = cookies[key];
                } else {
                    _value = GetDefaultValue();
                }
            }

            return _value;
        }
        
        public void SetValue(string value) {
            _value = value;
        }

        public void Write(IResponseCookies responseCookies) {
            var value = GetValue();
            
            if (value.HasValue()) {
                var cookieOptions = new CookieOptions();

                SetOptions(cookieOptions);

                responseCookies.Append(Name, _value, cookieOptions);
            }
        }
        
        protected abstract string Name { get; }
        protected abstract TimeSpan Lifetime { get; }

        protected virtual string GetDefaultValue() => null;

        protected virtual void SetOptions(CookieOptions cookieOptions) {
            cookieOptions.Path = "/";
            cookieOptions.Expires = DateTimeOffset.UtcNow.Add(Lifetime);
            cookieOptions.IsEssential = true;
            cookieOptions.HttpOnly = true;
            cookieOptions.Secure = true;
        }
    }
}