using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;
using NodaTime;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfundingContributionRepository {
    Task AppendAsync(string checkoutReference,
                     Instant timestamp,
                     Guid? campaignId,
                     Guid? teamId,
                     Guid? pageId,
                     bool isAnonymous,
                     string pageUrl,
                     string comment,
                     Money value,
                     string email,
                     Allocation allocation);

    Task CommitAsync();
}