using N3O.Umbraco.Constants;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.Entities;

public class EntitiesMigration : AsyncMigrationBase {
    public EntitiesMigration(IMigrationContext context) : base(context) { }

    protected override Task MigrateAsync() {
        if (!TableExists(Tables.Entities.Name)) {
            Create.Table<EntityRow>().Do();
        }

        return Task.CompletedTask;
    }
}
