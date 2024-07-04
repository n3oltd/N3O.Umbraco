using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Profanity;

public class ProfanityComposer : IComposer {
    public void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IProfanityService, ProfanityService>();
    }
}