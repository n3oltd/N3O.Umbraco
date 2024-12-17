using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Entities;

[TableName(Tables.CrowdfunderRevisions.Name)]
[PrimaryKey("Id")]
public class CrowdfunderRevision {
    [PrimaryKeyColumn(Name = Tables.CrowdfunderRevisions.PrimaryKey)]
    public int Id { get; set; }
    
    [Column(nameof(Type))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfunderRevisions.Name + "_" + nameof(Type), ForColumns = nameof(Type))]
    public int Type { get; set; }
    
    [Column(nameof(ContentKey))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfunderRevisions.Name + "_" + nameof(ContentKey), ForColumns = nameof(ContentKey))]
    public Guid ContentKey { get; set; }
    
    [Column(nameof(ContentRevision))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfunderRevisions.Name + "_" + nameof(ContentRevision), ForColumns = nameof(ContentRevision))]
    public int ContentRevision { get; set; }
    
    [Column(nameof(CampaignId))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfunderRevisions.Name + "_" + nameof(CampaignId), ForColumns = nameof(CampaignId))]
    public Guid CampaignId { get; set; }
    
    [Column(nameof(FundraiserId))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.Contributions.Name + "_" + nameof(FundraiserId), ForColumns = nameof(FundraiserId))]
    public Guid? FundraiserId { get; set; }
    
    [Column(nameof(Name))]
    [Length(CrowdfundingConstants.Crowdfunder.NameMaxLength)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfunderRevisions.Name + "_" + nameof(Name), ForColumns = nameof(Name))]
    public string Name { get; set; }
    
    [Column(nameof(Url))]
    [Length(CrowdfundingConstants.Crowdfunder.NameMaxLength + 200)]
    [Index(IndexTypes.NonClustered, Name = "IX_" + Tables.CrowdfunderRevisions.Name + "_" + nameof(Url), ForColumns = nameof(Url))]
    public string Url { get; set; }
    
    [Column(nameof(CurrencyCode))]
    [Length(3)]
    public string CurrencyCode { get; set; }
    
    [Column(nameof(GoalsTotalQuote))]
    public decimal GoalsTotalQuote { get; set; }
    
    [Column(nameof(GoalsTotalBase))]
    public decimal GoalsTotalBase { get; set; }
    
    [Column(nameof(ActiveFrom))]
    public DateTime ActiveFrom { get; set; }
    
    [Column(nameof(ActiveTo))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public DateTime? ActiveTo { get; set; }
    
    [Column(nameof(GoalCompletedOn))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public DateTime? GoalCompletedOn { get; set; }
}