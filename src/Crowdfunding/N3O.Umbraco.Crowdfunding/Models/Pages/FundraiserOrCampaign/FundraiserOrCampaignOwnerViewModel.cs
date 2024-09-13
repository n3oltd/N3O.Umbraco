namespace N3O.Umbraco.CrowdFunding.Models;

public class FundraiserOrCampaignOwnerViewModel {
    public string Name { get; private set; }
    public string AvatarLink { get; private set; }

    public static FundraiserOrCampaignOwnerViewModel For(string name, string avatarLink) {
        var viewModel = new FundraiserOrCampaignOwnerViewModel();

        viewModel.Name = name;
        viewModel.AvatarLink = avatarLink;

        return viewModel;
    }
}