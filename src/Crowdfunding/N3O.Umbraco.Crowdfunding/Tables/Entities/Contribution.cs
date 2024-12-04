using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using NPoco;
using System;
using System.Linq;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Entities;

[TableName(Tables.Contributions.Name)]
[PrimaryKey("Id")]
public class Contribution {
    [PrimaryKeyColumn(Name = Tables.Contributions.PrimaryKey)]
    public int Id { get; set; }

    [Column(nameof(Timestamp))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(Timestamp), ForColumns = nameof(Timestamp))]
    public DateTime Timestamp { get; set; }
    
    [Column(nameof(Date))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(Date), ForColumns = nameof(Date))]
    public DateTime Date { get; set; }

    [Column(nameof(CrowdfunderId))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(CrowdfunderId), ForColumns = nameof(CrowdfunderId))]
    public Guid CrowdfunderId { get; set; }
    
    [Column(nameof(CampaignId))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(CampaignId), ForColumns = nameof(CampaignId))]
    public Guid CampaignId { get; set; }

    [Column(nameof(CampaignName))]
    [Length(100)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(CampaignName), ForColumns = nameof(CampaignName))]
    public string CampaignName { get; set; }
    
    [Column(nameof(TeamId))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(TeamId), ForColumns = nameof(TeamId))]
    public Guid? TeamId { get; set; }
    
    [Column(nameof(TeamName))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(100)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(TeamName), ForColumns = nameof(TeamName))]
    public string TeamName { get; set; }
    
    [Column(nameof(FundraiserId))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(FundraiserId), ForColumns = nameof(FundraiserId))]
    public Guid? FundraiserId { get; set; }
    
    [Column(nameof(FundraiserName))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(100)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(FundraiserName), ForColumns = nameof(FundraiserName))]
    public string FundraiserName { get; set; }
    
    [Column(nameof(CrowdfunderUrl))]
    [Length(400)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(CrowdfunderUrl), ForColumns = nameof(CrowdfunderUrl))]
    public string CrowdfunderUrl { get; set; }
    
    [Column(nameof(TransactionReference))]
    [Length(50)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(TransactionReference), ForColumns = nameof(TransactionReference))]
    public string TransactionReference { get; set; }
    
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
    
    [Column(nameof(CrowdfunderAmount))]
    public decimal CrowdfunderAmount { get; set; }
    
    [Column(nameof(TaxReliefQuoteAmount))]
    public decimal TaxReliefQuoteAmount { get; set; }
    
    [Column(nameof(TaxReliefBaseAmount))]
    public decimal TaxReliefBaseAmount { get; set; }
    
    [Column(nameof(TaxReliefCrowdfunderAmount))]
    public decimal TaxReliefCrowdfunderAmount { get; set; }
    
    [Column(nameof(Anonymous))]
    public bool Anonymous { get; set; }
    
    [Column(nameof(Name))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(200)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(Name), ForColumns = nameof(Name))]
    public string Name { get; set; }
    
    [Column(nameof(Email))]
    [Length(100)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(Email), ForColumns = nameof(Email))]
    public string Email { get; set; }
    
    [Column(nameof(Comment))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(2000)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(Comment), ForColumns = nameof(Comment))]
    public string Comment { get; set; }
    
    [Column(nameof(Status))]
    [Length(50)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(Status), ForColumns = nameof(Status))]
    public string Status { get; set; }

    [Column(nameof(ContributionType))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(ContributionType), ForColumns = nameof(ContributionType))]
    public int ContributionType { get; set; }
    
    [Column(nameof(AllocationSummary))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(200)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(AllocationSummary), ForColumns = nameof(AllocationSummary))]
    public string AllocationSummary { get; set; }
    
    [Column(nameof(FundDimension1))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(100)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(FundDimension1), ForColumns = nameof(FundDimension1))]
    public string FundDimension1 { get; set; }
    
    [Column(nameof(FundDimension2))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(100)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(FundDimension2), ForColumns = nameof(FundDimension2))]
    public string FundDimension2 { get; set; }
    
    [Column(nameof(FundDimension3))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(100)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(FundDimension3), ForColumns = nameof(FundDimension3))]
    public string FundDimension3 { get; set; }
    
    [Column(nameof(FundDimension4))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Length(100)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(FundDimension4), ForColumns = nameof(FundDimension4))]
    public string FundDimension4 { get; set; }
    
    [Column(nameof(AllocationJson))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
    public string AllocationJson { get; set; }

    [Ignore]
    public GivingType GivingType => StaticLookups.FindById<GivingTypes, GivingType>(GivingTypeId);

    public Currency GetCurrency(ILookups lookups) {
        return lookups.GetAll<Currency>().Single(x => x.Code == CurrencyCode);
    }
}
