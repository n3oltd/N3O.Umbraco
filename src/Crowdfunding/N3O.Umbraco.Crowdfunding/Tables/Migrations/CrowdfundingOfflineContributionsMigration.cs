using N3O.Umbraco.Crowdfunding.Tables.Entities;
using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.Crowdfunding.Tables.Migrations;

public class CrowdfundingOfflineContributionsMigration : MigrationBase {
    public CrowdfundingOfflineContributionsMigration(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!TableExists(CrowdfundingConstants.Tables.CrowdfundingOfflineContributions.Name)) {
            Create.Table<CrowdfundingOfflineContributions>().Do();
        }
    }
}