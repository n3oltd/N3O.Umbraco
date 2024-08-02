using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding.Services;

public interface IFundraisingPage {
    bool IsMatch(string path);
    Task<object> GetViewModelAsync(string path);
    string ViewName { get; }
}