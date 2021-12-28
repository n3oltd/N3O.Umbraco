using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace N3O.Umbraco.Giving.Cart.Database;

[TableName(CartConstants.Tables.Carts)]
[PrimaryKey(nameof(Id), AutoIncrement = true)]
[ExplicitColumns]
public class CartTable {
    public CartTable() {
        Id = Guid.NewGuid();
    }
    
    [Column(nameof(Id))]
    [PrimaryKeyColumn]
    public Guid Id { get; set; }

    [Column(nameof(Json))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [SpecialDbType(SpecialDbTypes.NTEXT)]
    public string Json { get; set; }
}
