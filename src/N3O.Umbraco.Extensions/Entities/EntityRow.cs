using N3O.Umbraco.Constants;
using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace N3O.Umbraco.Entities {
    [TableName(Tables.Entities)]
    [PrimaryKey(nameof(Id), AutoIncrement = false)]
    [ExplicitColumns]
    public class EntityRow {
        [Column(nameof(Id))]
        [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Entities + "_" + nameof(Id), ForColumns = nameof(Id))]
        public Guid Id { get; set; }

        [Column(nameof(Revision))]
        public int Revision { get; set; }
        
        [Column(nameof(Timestamp))]
        public DateTime Timestamp { get; set; }
        
        [Column(nameof(Type))]
        [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Entities + "_" + nameof(Type), ForColumns = nameof(Type))]
        [Length(400)]
        public string Type { get; set; }

        [Column(nameof(Json))]
        [NullSetting(NullSetting = NullSettings.Null)]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string Json { get; set; }
    }
}
