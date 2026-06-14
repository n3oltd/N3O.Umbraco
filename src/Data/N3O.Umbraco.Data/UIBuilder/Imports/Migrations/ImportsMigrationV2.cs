using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.Data.UIBuilder;

public class ImportsMigrationV2 : AsyncMigrationBase {
    public ImportsMigrationV2(IMigrationContext context) : base(context) { }

    protected override Task MigrateAsync() {
        if (!ColumnExists(DataConstants.Tables.Imports.Name, nameof(Import.MoveUpdatedContentToContainer))) {
            Alter.Table(DataConstants.Tables.Imports.Name)
                 .AddColumn(nameof(Import.MoveUpdatedContentToContainer))
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(false)
                 .Do();
        }

        return Task.CompletedTask;
    }
}
