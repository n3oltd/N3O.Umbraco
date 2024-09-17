namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderOwnerViewModel {
    public string Name { get; private set; }
    public string ProfilePicture { get; private set; }
    public string Strapline { get; private set; }

    public static CrowdfunderOwnerViewModel For(string name, string profilePicture, string strapline) {
        var viewModel = new CrowdfunderOwnerViewModel();

        viewModel.Name = name;
        viewModel.ProfilePicture = profilePicture;
        viewModel.Strapline = strapline;

        return viewModel;
    }
}