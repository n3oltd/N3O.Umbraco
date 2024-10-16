using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Crowdfunding.Models;

//Right hand dashboard
public class CrowdfunderStatisticsItemRes {
    public string Name { get; set; }
    public MoneyRes GoalsTotal { get; set; }
    public MoneyRes ContributionsTotal { get; set; }
    public string Url { get; set; }
}