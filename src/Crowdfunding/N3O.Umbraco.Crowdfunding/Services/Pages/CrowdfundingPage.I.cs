using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding;

public interface ICrowdfundingPage {
    bool IsMatch(Uri requestUri);
    Task<object> GetViewModelAsync(Uri requestUri);
    string ViewName { get; }
}