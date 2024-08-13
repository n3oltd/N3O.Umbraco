using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Lookups;
using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.UIBuilder;

[TableName(Tables.CrowdfundingContributions.Name)]
[PrimaryKey("Id")]
public class CrowdfundingContribution {
    [PrimaryKeyColumn(Name = Tables.CrowdfundingContributions.PrimaryKey)]
    public int Id { get; set; }

    [Column(nameof(Timestamp))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfundingContributions.Name + "_" + nameof(Timestamp), ForColumns = nameof(Timestamp))]
    public DateTime Timestamp { get; set; }
    
    [Column(nameof(CampaignId))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfundingContributions.Name + "_" + nameof(CampaignId), ForColumns = nameof(CampaignId))]
    public Guid CampaignId { get; set; }

    [Column(nameof(CampaignName))]
    [Length(100)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfundingContributions.Name + "_" + nameof(CampaignName), ForColumns = nameof(CampaignName))]
    public string CampaignName { get; set; }
    
    [Column(nameof(TeamId))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfundingContributions.Name + "_" + nameof(TeamId), ForColumns = nameof(TeamId))]
    public Guid? TeamId { get; set; }
    
    [Column(nameof(TeamName))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(100)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfundingContributions.Name + "_" + nameof(TeamName), ForColumns = nameof(TeamName))]
    public string TeamName { get; set; }
    
    [Column(nameof(FundraiserId))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfundingContributions.Name + "_" + nameof(FundraiserId), ForColumns = nameof(FundraiserId))]
    public Guid FundraiserId { get; set; }
    
    [Column(nameof(FundraiserUrl))]
    [Length(400)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfundingContributions.Name + "_" + nameof(FundraiserUrl), ForColumns = nameof(FundraiserUrl))]
    public string FundraiserUrl { get; set; }
    
    [Column(nameof(CheckoutReference))]
    [Length(50)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfundingContributions.Name + "_" + nameof(CheckoutReference), ForColumns = nameof(CheckoutReference))]
    public string CheckoutReference { get; set; }
    
    [Column(nameof(GivingTypeId))]
    [Length(50)]
    public string GivingTypeId { get; set; }
    
    [Column(nameof(CurrencyCode))]
    [Length(3)]
    public string CurrencyCode { get; set; }
    
    [Column(nameof(QuoteAmount))]
    public decimal QuoteAmount { get; set; }
    
    [Column(nameof(BaseAmount))]
    public decimal BaseAmount { get; set; }
    
    [Column(nameof(TaxRelief))]
    public bool TaxRelief { get; set; }
    
    [Column(nameof(Anonymous))]
    public bool Anonymous { get; set; }
    
    [Column(nameof(Name))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(200)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfundingContributions.Name + "_" + nameof(Name), ForColumns = nameof(Name))]
    public string Name { get; set; }
    
    [Column(nameof(Email))]
    [Length(100)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfundingContributions.Name + "_" + nameof(Email), ForColumns = nameof(Email))]
    public string Email { get; set; }
    
    [Column(nameof(Comment))]
    [Length(2000)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfundingContributions.Name + "_" + nameof(Comment), ForColumns = nameof(Comment))]
    public string Comment { get; set; }
    
    [Column(nameof(Status))]
    [Length(50)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfundingContributions.Name + "_" + nameof(Status), ForColumns = nameof(Status))]
    public string Status { get; set; }
    
    [Column(nameof(AllocationJson))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
    public string AllocationJson { get; set; }

    [Ignore]
    public GivingType GivingType => StaticLookups.FindById<GivingTypes, GivingType>(GivingTypeId);
}
