using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Crowdfunding.Konstrukt;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(CrowdfundingConstants.ApiName);
        
        builder.Components().Append<CrowdfundingContributionsMigrationsComponent>();
        
        builder.Services.AddSingleton<ICrowdfundingContributionRepository, CrowdfundingContributionRepository>();
        builder.Services.AddScoped<IFundraisingPageModeAccessor, FundraisingPageModeAccessor>();
        builder.Services.AddScoped<IFundraisingPages, FundraisingPages>();
    }
}