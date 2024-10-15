using Umbraco.Cms.Infrastructure.Migrations;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Migrations;

public class CrowdfunderMigration : MigrationBase {
    public CrowdfunderMigration(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!TableExists(Tables.Crowdfunders.Name)) {
            Create.Table<Entities.Crowdfunder>().Do();
        }
    }
}
