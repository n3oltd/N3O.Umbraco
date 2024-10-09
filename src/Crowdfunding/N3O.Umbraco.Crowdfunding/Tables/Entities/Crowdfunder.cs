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
    
    // lookup type key, e.g. CrowdfunderTypes.Fundraiser.Key
    public int Type { get; set; }
    
    // Umbraco ID
    public Guid ContentKey { get; set; }
    public decimal Name { get; set; }
    public decimal Url { get; set; }
    public decimal Currency { get; set; }
    public decimal GoalsTotalQuote { get; set; }
    public decimal GoalsTotalBase { get; set; }
    
    // When we receive a donation that we insert into the contributions table, we call
    // CrowdfunderRepository.RefreshContributions() which does the debounce (see GitHub for
    // what this means). UPDATE Crowdfunder 
    //SET ContributionsTotalQuote = (SELECT SUM(CrowdfunderAmount) FROM Contributions WHERE CRowdfunderId =Crowdfunder.Id),
    //ContributionsTotalBase = (SELECT SUM(CrowdfunderAmount) FROM Contributions WHERE CRowdfunderId =Crowdfunder.Id)
    // etc.
    public decimal ContributionsTotalQuote { get; set; }
    public decimal ContributionsTotalBase { get; set; }
    
    // When the pledge webhook is received we'll call CrowdfunderRepository.UpdateNonDonationsTotal(crowdfunderId, forexMoney)
    // which will update these with the brought forward balance = transfers in
    public decimal NonDonationsTotalQuote { get; set; }
    public decimal NonDonationsTotalBase { get; set; }
    
    
    // These are from content saving handlers on crowdfunder and are generated from the 1st hero image
    public string TallImage { get; set; }
    public decimal WideImage { get; set; }
    public decimal JumboImage { get; set; }
    public decimal Owner { get; set; }
    public string Tags  { get; set; } // store as þtag1þtag2þ so when searching for a tag we can do like '%þ{tag}þ'
    public decimal FullText { get; set; } // CrowdfunderContent.GetFullText() -> Name, slug, + campaign name for fundraiser + name of member who owns it
    public decimal StatusKey { get; set; } // CrowdfunderStatuses.Key
    
    // add a CrowdfunderRepository.Search(type, string query) ->
    // optionally filter by type, but then do fulltext like
    
    // add a CrowdfunderRepository.FilterByTag(string tag) ->
    
    // CrowdfunderRepository.GetActiveTags()
    // SELECT Tags FROM Crowdfunders WHERE Status = 'active' -> in memory split on þ then distinct
}
