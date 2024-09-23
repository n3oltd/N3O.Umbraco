using N3O.Umbraco.Crowdfunding.Content;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderTagViewModel {
    public string Name { get; private set; }
    public string IconUrl { get; private set; }

    public static CrowdfunderTagViewModel For(TagContent tag) {
        var viewModel = new CrowdfunderTagViewModel();
        
        viewModel.Name = tag.Name;
        viewModel.IconUrl = tag.Category.Icon.Src;

        return viewModel;
    }
}