using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.Data.UIBuilder;

public class ImportsMigrationV1 : MigrationBase {
    public ImportsMigrationV1(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!TableExists(DataConstants.Tables.Imports.Name)) {
            Create.Table<Import>().Do();
        }
    }
}
