using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding.Services;

public abstract class CrowdfundingPageBase : ICrowdfundingPage {
    public abstract bool IsMatch(string path);
    public abstract Task<object> GetViewModelAsync(string path);

    public string ViewName => GetType().Name;
}