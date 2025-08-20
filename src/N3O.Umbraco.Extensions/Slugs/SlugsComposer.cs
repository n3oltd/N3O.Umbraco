using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Slugify;
using System.Text.RegularExpressions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Slug;

public class SlugsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<ISlugHelper>(_ => {
            var config = new SlugHelperConfiguration();
            config.DeniedCharactersRegex = new Regex(SlugsConstants.Slugs.DeniedCharacters);
            config.CollapseDashes = true;
            config.ForceLowerCase = true;

            return new SlugHelper(config);
        });
    }
}