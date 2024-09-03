using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace N3O.Umbraco.Crowdfunding.Entities;

[TableName(CrowdfundingConstants.Tables.CrowdfundingOfflineContributions.Name)]
[PrimaryKey("Id")]
public partial class CrowdfundingOfflineContributions {
    [PrimaryKeyColumn(Name = CrowdfundingConstants.Tables.CrowdfundingOfflineContributions.PrimaryKey)]
    public int Id { get; set; }
    
    [Column(nameof(PledgeId))]
    public Guid PledgeId { get; set; }
    
    [Column(nameof(PledgeRevision))]
    public int PledgeRevision { get; set; }
    
    [Column(nameof(CampaignId))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + CrowdfundingConstants.Tables.CrowdfundingOfflineContributions.Name + "_" + nameof(CampaignId), ForColumns = nameof(CampaignId))]
    public Guid CampaignId { get; set; }
    
    [Column(nameof(FundraiserId))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + CrowdfundingConstants.Tables.CrowdfundingOfflineContributions.Name + "_" + nameof(FundraiserId), ForColumns = nameof(FundraiserId))]
    public Guid? FundraiserId { get; set; }
    
    [Column(nameof(TeamId))]
    public Guid? TeamId { get; set; }
    
    [Column(nameof(CurrencyCode))]
    [Length(3)]
    public string CurrencyCode { get; set; }
    
    [Column(nameof(Amount))]
    public decimal Amount { get; set; }
    
    [Column(nameof(DonationsCount))]
    public int DonationsCount { get; set; }
}
