using N3O.Umbraco.Composing;
using N3O.Umbraco.CrowdFunding.Konstrukt;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.CrowdFunding;

public class CrowdfundingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Components().Append<CrowdfundingDonationsMigrationsComponent>();
    }
}
