using N3O.Umbraco.Crowdfunding.Entities;
using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.Crowdfunding.Migrations;

public class OfflineContributionMigration : MigrationBase {
    public OfflineContributionMigration(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!TableExists(CrowdfundingConstants.Tables.OfflineContributions.Name)) {
            Create.Table<OfflineContribution>().Do();
        }
    }
}