using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.Data.Konstrukt;

public class ImportsMigrationV2 : MigrationBase {
    public ImportsMigrationV2(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!ColumnExists(DataConstants.Tables.Imports.Name, nameof(Import.MoveUpdatedContentToContainer))) {
            Alter.Table(DataConstants.Tables.Imports.Name)
                 .AddColumn(nameof(Import.MoveUpdatedContentToContainer))
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(false)
                 .Do();
        }
    }
}
