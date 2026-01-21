using N3O.Umbraco.Authentication.Auth0;
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
    private readonly Auth0TokenAccessor _auth0TokenAccessor;
    private readonly IUserDirectoryIdAccessor _userDirectoryIdAccessor;
    private readonly ClientFactory<CartClient> _clientFactory;
    private readonly IContentLocator _contentLocator;
    private readonly IJsonProvider _jsonProvider;
    private readonly IConnectCartIdAccessor _connectCartIdAccessor;

    public AddToConnectCartHandler(Auth0TokenAccessor auth0TokenAccessor,
                                   IUserDirectoryIdAccessor userDirectoryIdAccessor,
                                   ClientFactory<CartClient> clientFactory,
                                   IContentLocator contentLocator,
                                   IJsonProvider jsonProvider,
                                   IConnectCartIdAccessor connectCartIdAccessor) {
        _auth0TokenAccessor = auth0TokenAccessor;
        _userDirectoryIdAccessor = userDirectoryIdAccessor;
        _clientFactory = clientFactory;
        _contentLocator = contentLocator;
        _jsonProvider = jsonProvider;
        _connectCartIdAccessor = connectCartIdAccessor;
    }

    public async Task<EntityId> Handle(AddToConnectCartCommand req, CancellationToken cancellationToken) {
        var bearerToken = await _auth0TokenAccessor.GetAsync(UserDirectoryTypes.Members);
        var onBehalfOf = await _userDirectoryIdAccessor.GetIdAsync(UserDirectoryTypes.Members);
            
        var client = await _clientFactory.CreateAsync(CloudApiTypes.Connect, bearerToken, onBehalfOf);

        var cartId = _connectCartIdAccessor.GetId();

        var connectBulkAddToCartReq = req.Model.ToConnectBulkAddToCartReq(_contentLocator, _jsonProvider);

        await client.InvokeAsync(x => x.BulkAddAsync(cartId.ToString(), connectBulkAddToCartReq));

        return cartId;
    }
}