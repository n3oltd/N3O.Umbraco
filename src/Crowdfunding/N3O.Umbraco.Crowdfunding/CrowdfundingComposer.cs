using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.CrowdFunding;
using N3O.Umbraco.Crowdfunding.Migrations;
using N3O.Umbraco.CrowdFunding.Services;
using N3O.Umbraco.Extensions;
using Slugify;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(CrowdfundingConstants.ApiName);
        
        builder.Components().Append<OnlineContributionMigrationsComponent>();
        builder.Components().Append<OfflineContributionMigrationsComponent>();
        
        builder.Services.AddSingleton<IOnlineContributionRepository, OnlineContributionRepository>();
        builder.Services.AddSingleton<IOfflineContributionRepository, OfflineContributionRepository>();
        builder.Services.AddScoped<ICrowdfundingHelper, CrowdfundingHelper>();
        builder.Services.AddSingleton<ISlugHelper>(_ => {
            var config = new SlugHelperConfiguration();
            config.DeniedCharactersRegex = CrowdfundingUrl.Routes.Slugs.DeniedCharacters;
            config.CollapseDashes = true;
            config.ForceLowerCase = true;

            return new SlugHelper(config);
        });
        
        RegisterAll(t => t.ImplementsInterface<IContentPropertyValidator>(),
                    t => builder.Services.AddTransient(typeof(IContentPropertyValidator), t));
        
        RegisterAll(t => t.ImplementsInterface<ICrowdfundingPage>(),
                    t => builder.Services.AddTransient(typeof(ICrowdfundingPage), t));
    }
}