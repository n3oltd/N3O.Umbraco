using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Engage;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Elements.Content;
using N3O.Umbraco.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Elements;

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

    public async Task SaveAndPublishDonationFormAsync() {
        var subscription = _subscriptionAccessor.GetSubscription();
        var client = await _clientFactory.CreateAsync(subscription, ClientTypes.BackOffice);
        
        var req = GetSaveAndPublishReq();
        
        await client.InvokeAsync(x => x.SaveAndPublishElementAsync, req);
    }

    private SaveAndPublishReq GetSaveAndPublishReq() {
        var giving = _contentLocator.Single<GivingContent>();

        var partialReqs = new List<SaveAndPublishPartialReq>();

        //PopulateTopLevelCategories(partials);
        PopulateDonationCategories(partialReqs, giving);
        PopulateDonationOptions(partialReqs, giving);

        var req = new SaveAndPublishReq();
        req.Element = new SaveAndPublishElementReq();
        req.Element.Id = giving.Content().Key.ToString();
        req.Element.Type = ElementType.DonationForm;
        req.Element.Content = giving.GetFormJson(_jsonProvider);
        req.Partials = partialReqs;

        return req;
    }
    
    private void PopulateDonationCategories(List<SaveAndPublishPartialReq> partials, GivingContent giving) {
        var categories = giving.GetCategories();
        var options = giving.GetOptions();

        foreach (var category in categories) {
            var categoryOptions = options.Where(x => x.AllCategories.Contains(category)).ToList();

            var req = new SaveAndPublishPartialReq();
            req.Id = category.Content().Key.ToString();
            req.Content = _jsonProvider.SerializeObject(category.Content());
            req.PublishedContent = category.ToFormJson(_jsonProvider, categoryOptions);

            partials.Add(req);
        }
    }
    
    private void PopulateDonationOptions(List<SaveAndPublishPartialReq> partials, GivingContent giving) {
        var options = giving.GetOptions();

        foreach (var option in options) {
            var req = new SaveAndPublishPartialReq();
            req.Id = option.Content().Key.ToString();
            req.Content = option.ToFormJson(_jsonProvider);

            partials.Add(req);
        }
    }
}