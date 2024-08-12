using N3O.Umbraco.Financial;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public class CrowdfundingPageProgress {
    public int SupportersCount { get; set; }
    public int DaysLeft { get; set; }
    public Money RaisedAmount { get; set; }
    public Money TargetAmount { get; set; }
    public decimal PercentageCompleted { get; set; }
}