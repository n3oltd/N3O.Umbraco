using N3O.Umbraco.Financial;
using NodaTime;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfundingWriter {
    Task AppendAsync(string checkoutReference,
                     Instant timestamp,
                     Guid? campaignId,
                     Guid? teamId,
                     string teamName,
                     Guid? pageId,
                     bool isAnonymous,
                     string pageUrl,
                     string comment,
                     Money value,
                     string email);

    Task<int> CommitAsync();
}