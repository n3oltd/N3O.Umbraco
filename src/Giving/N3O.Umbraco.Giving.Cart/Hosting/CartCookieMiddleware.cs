using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Hosting;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart.Hosting {
    public class CartCookieMiddleware : CookieMiddleware {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICartIdAccessor _cartIdAccessor;

        public CartCookieMiddleware(IWebHostEnvironment webHostEnvironment, ICartIdAccessor cartIdAccessor) {
            _webHostEnvironment = webHostEnvironment;
            _cartIdAccessor = cartIdAccessor;
        }
    
        protected override Task ModifyCookiesAsync(IResponseCookies cookies) {
            var cartId = _cartIdAccessor.GetCartId();
        
            var cookieOptions = new CookieOptions();
            cookieOptions.Path = "/";
            cookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(90);
            cookieOptions.IsEssential = true;
            cookieOptions.HttpOnly = false;
            cookieOptions.Secure = _webHostEnvironment.IsProduction();

            cookies.Append(CartConstants.Cookie, cartId.ToString(), cookieOptions);
        
            return Task.CompletedTask;
        }
    }
}