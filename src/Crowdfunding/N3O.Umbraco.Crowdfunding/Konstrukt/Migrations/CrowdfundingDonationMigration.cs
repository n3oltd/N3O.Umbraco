using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.CrowdFunding.Konstrukt;

public class CrowdfundingDonationMigration : MigrationBase {
    public CrowdfundingDonationMigration(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!TableExists(CrowdfundingConstants.Tables.CrowdfundingDonations.Name)) {
            Create.Table<CrowdfundingDonation>().Do();
        }
    }
}
