using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace N3O.Umbraco.Crowdfunding.Entities;

[TableName(CrowdfundingConstants.Tables.OfflineContributions.Name)]
[PrimaryKey("Id")]
public class OfflineContribution {
    [PrimaryKeyColumn(Name = CrowdfundingConstants.Tables.OfflineContributions.PrimaryKey)]
    public int Id { get; set; }

    [Column(nameof(CrowdfunderId))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + CrowdfundingConstants.Tables.OfflineContributions.Name + "_" + nameof(CrowdfunderId), ForColumns = nameof(CrowdfunderId))]
    public Guid CrowdfunderId { get; set; }
    
    [Column(nameof(CampaignId))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + CrowdfundingConstants.Tables.OfflineContributions.Name + "_" + nameof(CampaignId), ForColumns = nameof(CampaignId))]
    public Guid CampaignId { get; set; }
    
    [Column(nameof(FundraiserId))]
    [Index(IndexTypes.NonClustered, Name = "IX_" + CrowdfundingConstants.Tables.OfflineContributions.Name + "_" + nameof(FundraiserId), ForColumns = nameof(FundraiserId))]
    public Guid? FundraiserId { get; set; }
    
    [Column(nameof(TeamId))]
    public Guid? TeamId { get; set; }
    
    [Column(nameof(Count))]
    public int Count { get; set; }
    
    [Column(nameof(Total))]
    public decimal Total { get; set; }
}
