using N3O.Umbraco.Crowdfunding.Content;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfunderRevisionRepository {
    Task AddOrUpdateAsync(ICrowdfunderContent crowdfunderContent, int revision);
}