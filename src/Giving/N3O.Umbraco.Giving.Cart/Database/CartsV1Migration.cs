using Umbraco.Cms.Infrastructure.Migrations;

namespace N3O.Umbraco.Giving.Cart.Database;

public class CartsV1Migration : MigrationBase {
    public CartsV1Migration(IMigrationContext context) : base(context) { }

    protected override void Migrate() {
        if (!TableExists(CartConstants.Tables.Carts)) {
            Create.Table<CartTable>().Do();
        }
    }
}
