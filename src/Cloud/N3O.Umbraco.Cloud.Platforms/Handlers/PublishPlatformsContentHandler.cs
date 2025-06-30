using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Commands;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Handlers;

public class PublishPlatformsContentHandler : IRequestHandler<PublishPlatformsContentCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly IUmbracoMapper _mapper;
    private readonly ClientFactory<PlatformsClient> _clientFactory;

    public PublishPlatformsContentHandler(IContentLocator contentLocator,
                                          IUmbracoMapper mapper,
                                          ClientFactory<PlatformsClient> clientFactory) {
        _contentLocator = contentLocator;
        _mapper = mapper;
        _clientFactory = clientFactory;
    }

    public async Task<None> Handle(PublishPlatformsContentCommand req, CancellationToken cancellationToken) {
        var client = await _clientFactory.CreateAsync(UmbracoAuthTypes.User, CloudApiTypes.Engage);

        var platformsContent = _contentLocator.Single<PlatformsContent>();
        
        var umbracoContentReq = _mapper.Map<PlatformsContent, UmbracoContentReq>(platformsContent);

        await client.InvokeAsync(x => x.PublishContentAsync(umbracoContentReq, cancellationToken));

        return None.Empty;
    }
}