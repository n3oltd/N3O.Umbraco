using N3O.Umbraco.Crowdfunding.Content;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderTagsViewModel {
    public string Name { get; private set; }
    public string IconUrl { get; private set; }

    public static CrowdfunderTagsViewModel For(TagContent tag) {
        var viewModel = new CrowdfunderTagsViewModel();
        
        viewModel.Name = tag.Name;
        viewModel.IconUrl = tag.Category.Icon.Src;

        return viewModel;
    }
}