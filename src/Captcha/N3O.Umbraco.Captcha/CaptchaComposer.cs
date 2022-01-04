using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Captcha {
    public class CaptchaComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddScoped<ICaptchaValidator, CaptchaValidator>();
        }
    }
}