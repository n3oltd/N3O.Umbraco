using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface IFundraisingPages {
    CreatePageResult CreatePage();
    Task<IContentPublisher> GetEditorAsync(Guid id);
    bool IsPageNameAvailable(string name);
}