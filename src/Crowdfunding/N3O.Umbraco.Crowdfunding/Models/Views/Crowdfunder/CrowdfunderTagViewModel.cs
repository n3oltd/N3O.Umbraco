using N3O.Umbraco.Crowdfunding.Content;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderTagViewModel {
    public string Name { get; private set; }
    public string IconUrl { get; private set; }
    public string Url { get; private set; }

    public static CrowdfunderTagViewModel For(ICrowdfundingUrlBuilder urlBuilder, TagContent tag) {
        var viewModel = new CrowdfunderTagViewModel();
        
        viewModel.Name = tag.Name;
        viewModel.IconUrl = tag.Category.Icon.Src;
        viewModel.Url = tag.GetUrl(urlBuilder);

        return viewModel;
    }
}