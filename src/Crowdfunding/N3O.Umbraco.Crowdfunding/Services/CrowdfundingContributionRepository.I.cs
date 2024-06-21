using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;
using NodaTime;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfundingContributionRepository {
    // TODO Either replace with or at least add an overload that takes an ICrowdfundingData which will reduce the
    // number of properties we have to pass in
    void Add(string checkoutReference,
             Instant timestamp,
             Guid? campaignId,
             Guid? teamId,
             Guid? pageId,
             bool isAnonymous,
             string pageUrl,
             string comment,
             string email,
             Allocation allocation);

    Task CommitAsync();
}