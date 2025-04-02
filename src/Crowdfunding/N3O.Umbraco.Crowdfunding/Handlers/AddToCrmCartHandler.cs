using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Crm.Engage;
using N3O.Umbraco.Crm.Engage.Clients;
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
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private readonly IContentLocator _contentLocator;
    private readonly IJsonProvider _jsonProvider;
    private readonly ICrmCartIdAccessor _crmCartIdAccessor;

    public AddToCrmCartHandler(ClientFactory<CartClient> clientFactory,
                               ISubscriptionAccessor subscriptionAccessor,
                               IContentLocator contentLocator,
                               IJsonProvider jsonProvider,
                               ICrmCartIdAccessor crmCartIdAccessor) {
        _clientFactory = clientFactory;
        _contentLocator = contentLocator;
        _jsonProvider = jsonProvider;
        _crmCartIdAccessor = crmCartIdAccessor;
        _subscriptionAccessor = subscriptionAccessor;
    }

    public async Task<EntityId> Handle(AddToCrmCartCommand req, CancellationToken cancellationToken) {
        var subscription = _subscriptionAccessor.GetSubscription();
        var client = await _clientFactory.CreateAsync(subscription, ClientTypes.BackOffice);

        var cartId = _crmCartIdAccessor.GetId();

        var bulkAddToCartReq = req.Model.ToBulkAddToCrmCartReq(_contentLocator, _jsonProvider);

        await client.InvokeAsync<BulkAddToCartReq, string>(x => x.BulkAddAsync,
                                                           cartId.ToString(),
                                                           bulkAddToCartReq,
                                                           cancellationToken);

        return cartId;
    }
}