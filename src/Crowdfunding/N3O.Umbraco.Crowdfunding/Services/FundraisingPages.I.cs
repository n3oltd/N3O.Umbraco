using N3O.Umbraco.Content;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface IFundraisingPages {
    Task<IContentPublisher> GetEditorAsync(Guid id);
    IContentPublisher New(string name);
}