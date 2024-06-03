using Umbraco.Cms.Infrastructure.Migrations;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.UIBuilder;

public class CrowdfundingContributionsMigration : MigrationBase {
    public CrowdfundingContributionsMigration(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!TableExists(Tables.CrowdfundingContributions.Name)) {
            Create.Table<CrowdfundingContribution>().Do();
        }
    }
}
