using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfundingHelper {
    Task<CreateFundraiserResult> CreateFundraiserAsync(CreateFundraiserCommand req);
    string GetCrowdfundingPath(Uri requestUri);
    Task<IContentPublisher> GetEditorAsync(Guid id);
    IReadOnlyList<FundraiserContent> GetAllFundraisers();
    public bool IsFundraiser(IContent content);
    bool IsFundraiserNameAvailable(string name);
    Money GetQuoteMoney(decimal amount);
    IPublishedContent GetRootPage();
}