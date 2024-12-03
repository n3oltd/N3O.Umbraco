using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Elements.Content;
using N3O.Umbraco.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crm.Engage;

public class ElementsManager : IElementsManager {
    private readonly ClientFactory<ElementsClient> _clientFactory;
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private readonly IContentLocator _contentLocator;
    private readonly IJsonProvider _jsonProvider;

    public ElementsManager(ClientFactory<ElementsClient> clientFactory,
                           ISubscriptionAccessor subscriptionAccessor,
                           IContentLocator contentLocator,
                           IJsonProvider jsonProvider) {
        _clientFactory = clientFactory;
        _subscriptionAccessor = subscriptionAccessor;
        _contentLocator = contentLocator;
        _jsonProvider = jsonProvider;
    }

    public async Task CreateOrUpdateDonationOptionAsync() {
        var subscription = _subscriptionAccessor.GetSubscription();
        var client = await _clientFactory.CreateAsync(subscription, ClientTypes.BackOffice);

        var req = GetCreateElementReq();
        
        //await client.InvokeAsync(x => x.CreateElementAsync, req);
    }

    private CreateElementReq GetCreateElementReq() {
        var giving = _contentLocator.Single<GivingContent>();

        var partials = new List<PartialReq>();

        //PopulateTopLevelCategories(partials);
        PopulateDonationCategories(partials, giving);
        PopulateDonationOptions(partials, giving);

        var req = new CreateElementReq();
        req.Id = giving.Content().Key.ToString();
        req.Type = ElementType.DonationForm;
        req.Content = (DynamicDataReq) giving.GetFormJson(_jsonProvider);
        req.Partials = partials;

        return req;
    }
    
    private void PopulateDonationCategories(List<PartialReq> partials, GivingContent giving) {
        var categories = giving.GetDonationCategories();
        var options = giving.GetDonationOptions();

        foreach (var category in categories) {
            var linkedOptions = options.Where(x => x.Categories.Contains(category)).ToList();

            var req = new PartialReq();
            req.Id = category.Content().Key.ToString();
            req.Content = (DynamicDataReq) category.ToFormJson(_jsonProvider, linkedOptions);

            partials.Add(req);
        }
    }
    
    private void PopulateDonationOptions(List<PartialReq> partials, GivingContent giving) {
        var options = giving.GetDonationOptions();

        foreach (var option in options) {
            var req = new PartialReq();
            req.Id = option.Content().Key.ToString();
            req.Content = (DynamicDataReq) option.ToFormJson(_jsonProvider);

            partials.Add(req);
        }
    }
}