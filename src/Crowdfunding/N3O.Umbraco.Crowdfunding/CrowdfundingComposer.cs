using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.CrowdFunding.Services;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(CrowdfundingConstants.ApiName);
        
        builder.Components().Append<CrowdfundingContributionsMigrationsComponent>();
        
        builder.Services.AddSingleton<ICrowdfundingContributionRepository, CrowdfundingContributionRepository>();
        builder.Services.AddScoped<FundraisingPageHelper>();
        builder.Services.AddScoped<IFundraisingPages, FundraisingPages>();
        builder.Services.AddScoped<IFundraisingPages, FundraisingPages>();
        
        RegisterFundraisingPagePropertyValidators(builder);
        
        RegisterAll(t => t.ImplementsInterface<IFundraisingPage>(),
                    t => builder.Services.AddTransient(typeof(IFundraisingPage), t));
    }
    
    private void RegisterFundraisingPagePropertyValidators(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<IFundraisingPagePropertyValidator>(),
                    t => builder.Services.AddTransient(typeof(IFundraisingPagePropertyValidator), t));
    }
}