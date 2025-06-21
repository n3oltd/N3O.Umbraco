using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Cloud;
using N3O.Umbraco.Cloud.Engage;
using N3O.Umbraco.Cloud.Engage.Clients;
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

public class AddToCrmCartHandler : IRequestHandler<AddToCrmCartCommand, CrowdfundingCartReq, EntityId> {
    private readonly ClientFactory<CartClient> _clientFactory;
    private readonly IContentLocator _contentLocator;
    private readonly IJsonProvider _jsonProvider;
    private readonly ICrmCartIdAccessor _crmCartIdAccessor;

    public AddToCrmCartHandler(ClientFactory<CartClient> clientFactory,
                               IContentLocator contentLocator,
                               IJsonProvider jsonProvider,
                               ICrmCartIdAccessor crmCartIdAccessor) {
        _clientFactory = clientFactory;
        _contentLocator = contentLocator;
        _jsonProvider = jsonProvider;
        _crmCartIdAccessor = crmCartIdAccessor;
    }

    public async Task<EntityId> Handle(AddToCrmCartCommand req, CancellationToken cancellationToken) {
        var client = await _clientFactory.CreateAsync(ClientTypes.BackOffice);

        var cartId = _crmCartIdAccessor.GetId();

        var connectBulkAddToCartReq = req.Model.ToConnectBulkAddToCartReq(_contentLocator, _jsonProvider);

        await client.InvokeAsync(x => x.BulkAddAsync(cartId.ToString(), connectBulkAddToCartReq));

        return cartId;
    }
}