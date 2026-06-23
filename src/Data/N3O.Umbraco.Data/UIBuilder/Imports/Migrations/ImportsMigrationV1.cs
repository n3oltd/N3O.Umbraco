using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.Data.UIBuilder;

public class ImportsMigrationV1 : AsyncMigrationBase {
    public ImportsMigrationV1(IMigrationContext context) : base(context) { }

    protected override Task MigrateAsync() {
        if (!TableExists(DataConstants.Tables.Imports.Name)) {
            Create.Table<Import>().Do();
        }

        return Task.CompletedTask;
    }
}
