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

public class PublishPlatformsSubscriptionSettingsHandler 
    : IRequestHandler<PublishPlatformsSubscriptionSettingsCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly IUmbracoMapper _mapper;
    private readonly ClientFactory<ConnectClient> _clientFactory;

    public PublishPlatformsSubscriptionSettingsHandler(IContentLocator contentLocator,
                                                       IUmbracoMapper mapper,
                                                       ClientFactory<ConnectClient> clientFactory) {
        _contentLocator = contentLocator;
        _mapper = mapper;
        _clientFactory = clientFactory;
    }

    public async Task<None> Handle(PublishPlatformsSubscriptionSettingsCommand req, CancellationToken cancellationToken) {
        var client = await _clientFactory.CreateAsync(UmbracoAuthTypes.User, CloudApiTypes.Engage);

        var platformsContent = _contentLocator.Single<SettingsContent>();
        
        var subscriptionSettingsReq = _mapper.Map<SettingsContent, UmbracoSubscriptionSettingsReq>(platformsContent);

        await client.InvokeAsync(x => x.PublishSubscriptionSettingsAsync(subscriptionSettingsReq, cancellationToken));

        return None.Empty;
    }
}