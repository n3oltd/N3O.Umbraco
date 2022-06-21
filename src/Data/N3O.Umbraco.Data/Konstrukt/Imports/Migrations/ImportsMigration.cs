using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.Data.Konstrukt;

public class ImportsMigration : MigrationBase {
    public ImportsMigration(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!TableExists(DataConstants.Tables.Imports.Name)) {
            Create.Table<Import>().Do();
        }
    }
}
