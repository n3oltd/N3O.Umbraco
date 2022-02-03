using N3O.Umbraco.Constants;
using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.Entities {
    public class EntitiesMigration : MigrationBase {
        public EntitiesMigration(IMigrationContext context) : base(context) { }

        protected override void Migrate() {
            if (!TableExists(Tables.Entities.Name)) {
                Create.Table<EntityRow>().Do();
            }
        }
    }
}
