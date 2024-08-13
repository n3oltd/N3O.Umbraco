using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.CrowdFunding;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using N3O.Umbraco.Extensions;
using Slugify;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(CrowdfundingConstants.ApiName);
        
        builder.Components().Append<CrowdfundingContributionsMigrationsComponent>();
        
        builder.Services.AddSingleton<IContributionRepository, ContributionRepository>();
        builder.Services.AddScoped<ICrowdfundingHelper, CrowdfundingHelper>();
        builder.Services.AddSingleton<ISlugHelper, SlugHelper>();
        
        RegisterAll(t => t.ImplementsInterface<IContentPropertyValidator>(),
                    t => builder.Services.AddTransient(typeof(IContentPropertyValidator), t));
        
        RegisterAll(t => t.ImplementsInterface<ICrowdfundingPage>(),
                    t => builder.Services.AddTransient(typeof(ICrowdfundingPage), t));
    }
}