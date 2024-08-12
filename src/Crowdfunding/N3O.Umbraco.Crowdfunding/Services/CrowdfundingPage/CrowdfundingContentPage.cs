using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.CrowdFunding.Extensions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrowdfundingPageContent = N3O.Umbraco.Crowdfunding.Content.CrowdfundingPageContent;

namespace N3O.Umbraco.CrowdFunding.Services;

public class CrowdfundingPage : CrowdfundingPageBase {
    private readonly IContentLocator _contentLocator;
    private readonly FundraisingPageHelper _pageHelper;
    
    public CrowdfundingPage(IContentLocator contentLocator, FundraisingPageHelper pageHelper) {
        _contentLocator = contentLocator;
        _pageHelper = pageHelper;
    }

    public override bool IsMatch(string path) {
        return Regex.IsMatch(path, CrowdfundingPageUrlConstants.CrowdfundingContentPagePathPattern, RegexOptions.IgnoreCase);
    }

    public override async Task<object> GetViewModelAsync(string path) {
        var match = Regex.Match(path, CrowdfundingPageUrlConstants.CrowdfundingContentPagePathPattern);
        var pageId = int.Parse(match.Groups[1].Value);
        var pageContent = _contentLocator.ById<CrowdfundingPageContent>(pageId);

        var viewModel = await pageContent.GetCrowdfundingContentPageViewModelAsync(_pageHelper);

        return viewModel;
    }
}