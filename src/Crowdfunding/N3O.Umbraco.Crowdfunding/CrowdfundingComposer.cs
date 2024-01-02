using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Crowdfunding.Konstrukt;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Components().Append<CrowdfundingContributionsMigrationsComponent>();
        
        builder.Services.AddSingleton<ICrowdfundingWriter, CrowdfundingWriter>();
    }
}