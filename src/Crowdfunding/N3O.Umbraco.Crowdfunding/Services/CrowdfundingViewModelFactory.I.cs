using N3O.Umbraco.Crowdfunding.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfundingViewModelFactory {
    Task<T> CreateViewModelAsync<T>(ICrowdfundingPage page) where T : CrowdfundingViewModel, new();
}