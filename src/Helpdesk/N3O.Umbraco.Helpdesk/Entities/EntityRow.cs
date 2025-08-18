using N3O.Umbraco.Constants;
using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace N3O.Umbraco.Entities;

[TableName(Tables.Entities.Name)]
[ExplicitColumns]
public class EntityRow {
    [Column(nameof(Id))]
    [PrimaryKeyColumn(AutoIncrement = false, Clustered = true, Name = Tables.Entities.PrimaryKey)]
    public Guid Id { get; set; }

    [Column(nameof(Revision))]
    public int Revision { get; set; }
    
    [Column(nameof(Timestamp))]
    public DateTime Timestamp { get; set; }
    
    [Column(nameof(Type))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Entities.Name + "_" + nameof(Type), ForColumns = nameof(Type))]
    [Length(400)]
    public string Type { get; set; }

    [Column(nameof(Json))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
    public string Json { get; set; }
}
