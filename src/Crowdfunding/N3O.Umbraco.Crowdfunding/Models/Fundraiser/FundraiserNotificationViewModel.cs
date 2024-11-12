using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Financial;
using System;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserNotificationViewModel {
    public FundraiserNotificationViewModel(string fundraiserEmail,
                                           string fundraiserName,
                                           string title,
                                           string campaignName,
                                           DateTime createdOn,
                                           decimal goalsTotal,
                                           CrowdfunderStatus status,
                                           Currency currency) {
        FundraiserEmail = fundraiserEmail;
        FundraiserName = fundraiserName;
        Title = title;
        CampaignName = campaignName;
        CreatedOn = createdOn;
        GoalsTotal = goalsTotal;
        Status = status;
        Currency = currency;
    }

    public FundraiserNotificationViewModel(FundraiserContent fundraiser)
        : this(fundraiser.Owner.Email,
               fundraiser.Owner.Name,
               fundraiser.Name,
               fundraiser.CampaignName,
               fundraiser.CreatedDate,
               fundraiser.Goals.Sum(x => x.Amount),
               fundraiser.Status,
               fundraiser.Currency) { }

    public string FundraiserEmail { get; set; }
    public string FundraiserName { get; set; }
    public string Title { get; set; }
    public string CampaignName { get; set; }
    public DateTime CreatedOn { get; set; }
    public decimal GoalsTotal { get; set; }
    public Currency Currency { get; set; }
    public CrowdfunderStatus Status { get; set; }
}