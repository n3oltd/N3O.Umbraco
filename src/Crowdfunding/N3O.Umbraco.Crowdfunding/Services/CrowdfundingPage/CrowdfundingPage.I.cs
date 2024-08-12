using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding.Services;

public interface ICrowdfundingPage {
    bool IsMatch(string path);
    Task<object> GetViewModelAsync(string path);
    string ViewName { get; }
}