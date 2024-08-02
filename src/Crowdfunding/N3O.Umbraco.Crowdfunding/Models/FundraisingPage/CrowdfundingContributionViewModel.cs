using N3O.Umbraco.Financial;
using System;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public class CrowdfundingContributionViewModel {
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public Money Amount { get; set; }
}