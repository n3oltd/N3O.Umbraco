using N3O.Umbraco.Crowdfunding.Content;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserNotificationViewModel {
    public FundraiserNotificationViewModel(FundraiserContent fundraiser) {
        Fundraiser = fundraiser;
    }

    public FundraiserContent Fundraiser { get; }
}