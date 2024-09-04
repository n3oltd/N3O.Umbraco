using N3O.Umbraco.Crowdfunding.Entities;
using Umbraco.Cms.Infrastructure.Migrations;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Migrations;

public class OnlineContributionMigration : MigrationBase {
    public OnlineContributionMigration(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!TableExists(Tables.OnlineContributions.Name)) {
            Create.Table<OnlineContribution>().Do();
        }
    }
}
