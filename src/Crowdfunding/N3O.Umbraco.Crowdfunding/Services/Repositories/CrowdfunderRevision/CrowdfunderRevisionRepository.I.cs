using N3O.Umbraco.Crowdfunding.Content;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfunderRevisionRepository {
    Task UpdateCrowdfunderRevisionAsync(ICrowdfunderContent crowdfunder, int revision);
    Task AddCrowdfunderRevisionAsync(ICrowdfunderContent crowdfunderContent, int revision);
    Task CloseCrowdfunderRevisionAsync(Guid id);
}