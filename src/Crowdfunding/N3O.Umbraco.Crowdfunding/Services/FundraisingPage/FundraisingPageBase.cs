using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding.Services;

public abstract class FundraisingPageBase : IFundraisingPage {
    public abstract bool IsMatch(string path);
    public abstract Task<object> GetViewModelAsync(string path);

    public string ViewName => GetType().Name;
}