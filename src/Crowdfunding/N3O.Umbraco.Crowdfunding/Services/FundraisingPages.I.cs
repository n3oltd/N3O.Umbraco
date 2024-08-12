using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Crowdfunding;

public interface IFundraisingPages {
    Task<CreatePageResult> CreatePageAsync(CreatePageCommand req);
    Task<IContentPublisher> GetEditorAsync(Guid id);
    IReadOnlyList<CrowdfundingPageContent> GetAllFundraisingPages();
    public bool IsFundraisingPage(IContent content);
    bool IsPageNameAvailable(string name);
}