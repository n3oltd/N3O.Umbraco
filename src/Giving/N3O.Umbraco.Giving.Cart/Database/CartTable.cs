using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace N3O.Umbraco.Giving.Cart.Database {
    [TableName(CartConstants.Tables.Carts)]
    [PrimaryKey(nameof(Key), AutoIncrement = true)]
    [ExplicitColumns]
    public class CartTable {
        public CartTable() {
            Id = Guid.NewGuid();
        }
        
        [Column(nameof(Key))]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Key { get; set; }
    
        [Column(nameof(Id))]
        [Index(IndexTypes.NonClustered, Name = "IX_" + CartConstants.Tables.Carts + "_" + nameof(Id), ForColumns = nameof(Id))]
        public Guid Id { get; set; }

        [Column(nameof(Json))]
        [NullSetting(NullSetting = NullSettings.Null)]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string Json { get; set; }
    }
}
