using N3O.Umbraco.Crowdfunding;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding;

public abstract class CrowdfundingPage : ICrowdfundingPage {
    protected CrowdfundingPage(ICrowdfundingHelper crowdfundingHelper) {
        CrowdfundingHelper = crowdfundingHelper;
    }

    public bool IsMatch(Uri requestUri) {
        var crowdfundingPath = CrowdfundingHelper.GetCrowdfundingPath(requestUri);

        return IsMatch(crowdfundingPath);
    }

    public async Task<object> GetViewModelAsync(Uri requestUri) {
        var crowdfundingPath = CrowdfundingHelper.GetCrowdfundingPath(requestUri);

        return await GetViewModelAsync(crowdfundingPath);
    }

    protected bool IsMatch(string crowdfundingPath, string pattern) {
        return Regex.IsMatch(crowdfundingPath, pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    }
    
    protected Match Match(string crowdfundingPath, string pattern) {
        return Regex.Match(crowdfundingPath, pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    }

    protected abstract bool IsMatch(string crowdfundingPath);

    protected abstract Task<object> GetViewModelAsync(string crowdfundingPath);

    public string ViewName => GetType().Name;
    
    protected ICrowdfundingHelper CrowdfundingHelper { get; }
}