using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Entities;

[TableName(Tables.Crowdfunders.Name)]
[PrimaryKey("Id")]
public class Crowdfunder {
    [PrimaryKeyColumn(Name = Tables.Crowdfunders.PrimaryKey)]
    public int Id { get; set; }
    
    [Column(nameof(Type))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Crowdfunders.Name + "_" + nameof(Type), ForColumns = nameof(Type))]
    public int Type { get; set; }
    
    [Column(nameof(ContentKey))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Crowdfunders.Name + "_" + nameof(ContentKey), ForColumns = nameof(ContentKey))]
    public Guid ContentKey { get; set; }
    
    [Column(nameof(CreatedAt))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Crowdfunders.Name + "_" + nameof(CreatedAt), ForColumns = nameof(CreatedAt))]
    public DateTime CreatedAt { get; set; }
    
    [Column(nameof(Name))]
    [Length(CrowdfundingConstants.Crowdfunder.NameMaxLength)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Crowdfunders.Name + "_" + nameof(Name), ForColumns = nameof(Name))]
    public string Name { get; set; }
    
    [Column(nameof(Url))]
    [Length(CrowdfundingConstants.Crowdfunder.NameMaxLength + 200)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Crowdfunders.Name + "_" + nameof(Url), ForColumns = nameof(Url))]
    public string Url { get; set; }
    
    [Column(nameof(CurrencyCode))]
    [Length(3)]
    public string CurrencyCode { get; set; }
    
    [Column(nameof(GoalsTotalQuote))]
    public decimal GoalsTotalQuote { get; set; }
    
    [Column(nameof(GoalsTotalBase))]
    public decimal GoalsTotalBase { get; set; }
    
    [Column(nameof(ContributionsTotalQuote))]
    public decimal ContributionsTotalQuote { get; set; }
    
    [Column(nameof(ContributionsTotalBase))]
    public decimal ContributionsTotalBase { get; set; }
    
    [Column(nameof(NonDonationsTotalQuote))]
    public decimal NonDonationsTotalQuote { get; set; }
    
    [Column(nameof(NonDonationsTotalBase))]
    public decimal NonDonationsTotalBase { get; set; }
    
    [Column(nameof(LeftToRaiseBase))]
    public decimal LeftToRaiseBase { get; set; }
    
    [Column(nameof(LeftToRaiseQuote))]
    public decimal LeftToRaiseQuote { get; set; }
    
    [Column(nameof(LastContributionOn))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public DateTime LastContributionOn { get; set; }
    
    [Column(nameof(TallImage))]
    public string TallImage { get; set; }
    
    [Column(nameof(WideImage))]
    public string WideImage { get; set; }
    
    [Column(nameof(JumboImage))]
    public string JumboImage { get; set; }
    
    [Column(nameof(OwnerEmail))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public string OwnerEmail { get; set; }
    
    [Column(nameof(OwnerName))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public string OwnerName { get; set; }
    
    [Column(nameof(OwnerProfilePicture))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public string OwnerProfilePicture { get; set; }
    
    [Column(nameof(Tags))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public string Tags  { get; set; }
    
    [Column(nameof(FullText))]
    public string FullText { get; set; }
    
    [Column(nameof(StatusKey))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public int? StatusKey { get; set; }
}
