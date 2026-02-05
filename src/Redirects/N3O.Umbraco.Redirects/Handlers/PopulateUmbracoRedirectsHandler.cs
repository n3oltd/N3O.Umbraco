using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Redirects.Commands;
using N3O.Umbraco.Redirects.Content;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Redirects.Handlers;

public class PopulateUmbracoRedirectsHandler : IRequestHandler<PopulateUmbracoRedirectsCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly AsyncKeyedLocker<string> _locker;

    public PopulateUmbracoRedirectsHandler(IContentLocator contentLocator, AsyncKeyedLocker<string> locker) {
        _contentLocator = contentLocator;
        _locker = locker;
    }
    
    public async Task<None> Handle(PopulateUmbracoRedirectsCommand request, CancellationToken cancellationToken) {
        using (await _locker.LockAsync(nameof(PopulateUmbracoRedirectsHandler), cancellationToken)) {
            var redirects = _contentLocator.All<RedirectContent>();
            
            UmbracoRedirects.Clear();

            foreach (var redirect in redirects.OrEmpty()) {
                UmbracoRedirects.Add(redirect.Name, );
            }
        }
    }
}
