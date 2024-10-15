using Umbraco.Cms.Infrastructure.Migrations;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Migrations;

public class CrowdfunderRevisionMigration : MigrationBase {
    public CrowdfunderRevisionMigration(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!TableExists(Tables.CrowdfunderRevisions.Name)) {
            Create.Table<Entities.CrowdfunderRevision>().Do();
        }
    }
}
