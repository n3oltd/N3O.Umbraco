using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Cloud;
using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class AddToConnectCartHandler : IRequestHandler<AddToConnectCartCommand, CrowdfundingCartReq, EntityId> {
    private readonly ClientFactory<CartClient> _clientFactory;
    private readonly IContentLocator _contentLocator;
    private readonly IJsonProvider _jsonProvider;
    private readonly IConnectCartIdAccessor _connectCartIdAccessor;

    public AddToConnectCartHandler(ClientFactory<CartClient> clientFactory,
                                   IContentLocator contentLocator,
                                   IJsonProvider jsonProvider,
                                   IConnectCartIdAccessor connectCartIdAccessor) {
        _clientFactory = clientFactory;
        _contentLocator = contentLocator;
        _jsonProvider = jsonProvider;
        _connectCartIdAccessor = connectCartIdAccessor;
    }

    public async Task<EntityId> Handle(AddToConnectCartCommand req, CancellationToken cancellationToken) {
        var client = await _clientFactory.CreateAsync(UmbracoAuthTypes.User, CloudApiTypes.Connect);

        var cartId = _connectCartIdAccessor.GetId();

        var connectBulkAddToCartReq = req.Model.ToConnectBulkAddToCartReq(_contentLocator, _jsonProvider);

        await client.InvokeAsync(x => x.BulkAddAsync(cartId.ToString(), connectBulkAddToCartReq));

        return cartId;
    }
}