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
    
    [Column(nameof(Name))]
    [Length(200)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Crowdfunders.Name + "_" + nameof(Name), ForColumns = nameof(Name))]
    public string Name { get; set; }
    
    [Column(nameof(Url))]
    [Length(400)]
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
    
    [Column(nameof(TallImage))]
    public string TallImage { get; set; }
    
    [Column(nameof(WideImage))]
    public string WideImage { get; set; }
    
    [Column(nameof(JumboImage))]
    public string JumboImage { get; set; }
    
    [Column(nameof(Owner))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public string Owner { get; set; }
    
    [Column(nameof(Tags))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public string Tags  { get; set; }
    
    [Column(nameof(FullText))]
    public string FullText { get; set; }
    
    [Column(nameof(StatusKey))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public int? StatusKey { get; set; }
}
