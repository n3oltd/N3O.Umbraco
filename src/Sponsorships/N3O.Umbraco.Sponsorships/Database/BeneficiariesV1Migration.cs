using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.Sponsorships.Database;

public class BeneficiariesV1Migration : MigrationBase {
    public BeneficiariesV1Migration(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!TableExists(SponsorshipsConstants.Tables.Beneficiaries)) {
            Create.Table<BeneficiarySchema>().Do();
        }
    }
}
