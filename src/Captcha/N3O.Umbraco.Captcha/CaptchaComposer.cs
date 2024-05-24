using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Captcha;

public class CaptchaComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<ICaptchaValidatorFactory, CaptchaValidatorFactory>();
        
        RegisterAll(t => t.ImplementsInterface<ICaptchaValidator>(),
                    t => builder.Services.AddTransient(typeof(ICaptchaValidator), t));
    }
}
