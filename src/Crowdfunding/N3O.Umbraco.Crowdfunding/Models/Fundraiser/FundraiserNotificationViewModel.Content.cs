using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Financial;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserContentViewModel {
    [JsonConstructor]
    public FundraiserContentViewModel(string fundraiserEmail,
                                      string fundraiserFirstName,
                                      string fundraiserLastName,
                                      string fundraiserName,
                                      string fundraiserLink,
                                      string title,
                                      string campaignName,
                                      DateTime createdOn,
                                      decimal goalsTotal,
                                      CrowdfunderStatus status,
                                      Currency currency) {
        FundraiserEmail = fundraiserEmail;
        FundraiserFirstName = fundraiserFirstName;
        FundraiserLastName = fundraiserLastName;
        FundraiserName = fundraiserName;
        FundraiserLink = fundraiserLink;
        Title = title;
        CampaignName = campaignName;
        CreatedOn = createdOn;
        GoalsTotal = goalsTotal;
        Status = status;
        Currency = currency;
    }

    public FundraiserContentViewModel(ICrowdfundingUrlBuilder crowdfundingUrlBuilder, FundraiserContent fundraiser)
        : this(fundraiser.Owner.Email,
               fundraiser.Owner.FirstName,
               fundraiser.Owner.LastName,
               fundraiser.Owner.Name,
               fundraiser.Url(crowdfundingUrlBuilder),
               fundraiser.Name,
               fundraiser.CampaignName,
               fundraiser.CreatedDate,
               fundraiser.Goals.Sum(x => x.Amount),
               fundraiser.Status,
               fundraiser.Currency) { }

    public string FundraiserEmail { get; }
    public string FundraiserFirstName { get; }
    public string FundraiserLastName { get; }
    public string FundraiserName { get; }
    public string FundraiserLink { get; }
    public string Title { get; }
    public string CampaignName { get; }
    public DateTime CreatedOn { get; }
    public decimal GoalsTotal { get; }
    public Currency Currency { get; }
    public CrowdfunderStatus Status { get; }
}