using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.CrowdFunding;

public class CrowdfundingDonationsMigration : MigrationBase {
    public CrowdfundingDonationsMigration(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!TableExists(CrowdfundingConstants.Tables.CrowdfundingDonations.Name)) {
            Create.Table<CrowdfundingDonations>().Do();
        }
    }
}
