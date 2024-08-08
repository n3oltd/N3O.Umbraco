using N3O.Umbraco.Content;
using N3O.Umbraco.CrowdFunding.Extensions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrowdfundingPageContent = N3O.Umbraco.Crowdfunding.Content.CrowdfundingPageContent;

namespace N3O.Umbraco.CrowdFunding.Services;

public class FundraisingPage : FundraisingPageBase {
    private readonly IContentLocator _contentLocator;
    private readonly FundraisingPageHelper _pageHelper;
    
    public FundraisingPage(IContentLocator contentLocator, FundraisingPageHelper pageHelper) {
        _contentLocator = contentLocator;
        _pageHelper = pageHelper;
    }

    public override bool IsMatch(string path) {
        return Regex.IsMatch(path, FundraisingPageUrls.FundraisingContentPagePathPattern, RegexOptions.IgnoreCase);
    }

    public override async Task<object> GetViewModelAsync(string path) {
        var match = Regex.Match(path, FundraisingPageUrls.FundraisingContentPagePathPattern);
        var pageId = int.Parse(match.Groups[1].Value);
        var pageContent = _contentLocator.ById<CrowdfundingPageContent>(pageId);

        var viewModel = await pageContent.GetFundraisingContentPageViewModelAsync(_pageHelper);

        return viewModel;
    }
}