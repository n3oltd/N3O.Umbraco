using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Engage;
using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Cart;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class AddToCrmCartHandler : IRequestHandler<AddToCrmCartCommand, CrowdfundingCartReq, RevisionId> {
    private readonly ClientFactory<CartClient> _clientFactory;
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private readonly IContentLocator _contentLocator;
    private readonly IJsonProvider _jsonProvider;
    private readonly ICartAccessor _cartAccessor;

    public AddToCrmCartHandler(ClientFactory<CartClient> clientFactory,
                               ISubscriptionAccessor subscriptionAccessor,
                               IContentLocator contentLocator,
                               IJsonProvider jsonProvider,
                               ICartAccessor cartAccessor) {
        _clientFactory = clientFactory;
        _contentLocator = contentLocator;
        _jsonProvider = jsonProvider;
        _cartAccessor = cartAccessor;
        _subscriptionAccessor = subscriptionAccessor;
    }

    public async Task<RevisionId> Handle(AddToCrmCartCommand req, CancellationToken cancellationToken) {
        var subscription = _subscriptionAccessor.GetSubscription();
        var client = await _clientFactory.CreateAsync(subscription, ClientTypes.BackOffice);

        var cart = await _cartAccessor.GetAsync();

        var bulkAddToCartReq = req.Model.ToBulkAddToCrmCartReq(_contentLocator, _jsonProvider);

        await client.InvokeAsync<BulkAddToCartReq, string>(x => x.BulkAddAsync,
                                                           cart.Id.ToString(),
                                                           bulkAddToCartReq,
                                                           cancellationToken);

        return cart.RevisionId;
    }
}