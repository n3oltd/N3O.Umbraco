using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages;

public interface IPageExtension {
    string Key { get; }
    
    Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken);
}
