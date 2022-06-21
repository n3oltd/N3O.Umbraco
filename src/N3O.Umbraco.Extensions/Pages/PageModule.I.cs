using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages;

public interface IPageModule {
    string Key { get; }

    bool ShouldExecute(IPublishedContent page);
    Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken);
}
