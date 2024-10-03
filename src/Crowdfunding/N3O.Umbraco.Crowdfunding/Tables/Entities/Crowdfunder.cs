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
    
 public decimal Type { get; set; }
 public Guid ContentKey { get; set; }
 public decimal Name { get; set; }
 public decimal Url { get; set; }
 public decimal Currency { get; set; }
 public decimal GoalsTotalQuote { get; set; }
 public decimal GoalsTotalBase { get; set; }
 public decimal ContributionsTotalQuote { get; set; }
 public decimal ContributionsTotalBase { get; set; }
 public decimal NonDonationsTotalQuote { get; set; }
 public decimal NonDonationsTotalBase { get; set; }
 public decimal LargeImagePath { get; set; }
 public decimal SmallImagePath { get; set; }
 public decimal CreatedBy { get; set; }
 public decimal Tags  { get; set; }
 public decimal FullText { get; set; }
 public decimal StatusKey { get; set; }

}
