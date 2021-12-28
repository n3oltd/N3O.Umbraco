using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace N3O.Umbraco.Sponsorships.Database;

[TableName(SponsorshipsConstants.Tables.Beneficiaries)]
[PrimaryKey(nameof(Id), AutoIncrement = true)]
[ExplicitColumns]
public class BeneficiarySchema {
    [Column(nameof(Id))]
    [PrimaryKeyColumn(AutoIncrement = true)]
    public int Id { get; set; }

    [Column(nameof(Reference))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + SponsorshipsConstants.Tables.Beneficiaries + "_" + nameof(Reference), ForColumns = nameof(Reference))]
    public string Reference { get; set; }

    [Column(nameof(FullName))]
    public string FullName { get; set; }

    [Column(nameof(Priority))]
    public int Priority { get; set; }
    
    [Column(nameof(Json))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [SpecialDbType(SpecialDbTypes.NTEXT)]
    public string Json { get; set; }
    
    [Column(nameof(Sponsored))]
    public bool Sponsored { get; set; }
}
