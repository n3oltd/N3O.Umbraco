using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Crowdfunding.Migrations;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(CrowdfundingConstants.ApiName);
        builder.Services.AddOpenApiDocument(CrowdfundingConstants.BackOfficeApiName);
        builder.Services.AddOpenApiDocument(CrowdfundingConstants.ProxyApiName);
        
        builder.Components().Append<ContributionMigrationsComponent>();
        builder.Components().Append<CrowdfunderMigrationsComponent>();
        builder.Components().Append<CrowdfunderRevisionMigrationsComponent>();
        
        builder.Services.AddScoped<CrowdfunderJob>();
        builder.Services.AddTransient<IConnectCartIdAccessor, ConnectCartIdAccessor>();
        builder.Services.AddTransient<IContributionRepository, ContributionRepository>();
        builder.Services.AddTransient<ICrowdfunderRepository, CrowdfunderRepository>();
        builder.Services.AddTransient<ICrowdfunderRevisionRepository, CrowdfunderRevisionRepository>();
        builder.Services.AddTransient<ICrowdfundingNotifications, CrowdfundingNotifications>();
        builder.Services.AddScoped<ICrowdfundingRouter, CrowdfundingRouter>();
        builder.Services.AddTransient<ICrowdfundingUrlBuilder, CrowdfundingUrlBuilder>();
        builder.Services.AddScoped<ICrowdfundingViewModelFactory, CrowdfundingViewModelFactory>();
        builder.Services.AddTransient<OurBackofficeUserAccessor>();
        
        RegisterAll(t => t.ImplementsInterface<ICrowdfundingPage>(),
                    t => builder.Services.AddTransient(typeof(ICrowdfundingPage), t));
    }
}