using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Engage;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Elements.Content;
using N3O.Umbraco.Elements.Models;
using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements;

public class ElementsManager : IElementsManager {
    private readonly ClientFactory<ElementsClient> _clientFactory;
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private readonly IContentLocator _contentLocator;
    private readonly IJsonProvider _jsonProvider;
    private readonly IUmbracoMapper _mapper;

    public ElementsManager(ClientFactory<ElementsClient> clientFactory,
                           ISubscriptionAccessor subscriptionAccessor,
                           IContentLocator contentLocator,
                           IJsonProvider jsonProvider,
                           IUmbracoMapper mapper) {
        _clientFactory = clientFactory;
        _subscriptionAccessor = subscriptionAccessor;
        _contentLocator = contentLocator;
        _jsonProvider = jsonProvider;
        _mapper = mapper;
    }

    public async Task SaveAndPublishDonationFormAsync() {
        var subscription = _subscriptionAccessor.GetSubscription();
        var client = await _clientFactory.CreateAsync(subscription, ClientTypes.BackOffice);
        
        var req = GetSaveAndPublishReq();
        
        await client.InvokeAsync(x => x.SaveAndPublishElementAsync, req);
    }

    private SaveAndPublishReq GetSaveAndPublishReq() {
        var giving = _contentLocator.Single<GivingContent>();

        var categoryPartialReqs = giving.AllCategories.Select(x => GetSaveAndPublishPartialReq<DonationCategoryContent, DonationCategoryPartial>(x, PartialType.DonationFormCategory));
        var optionPartialReqs = giving.AllOptions.Select(x => GetSaveAndPublishPartialReq<DonationOptionContent, DonationOptionPartial>(x, PartialType.DonationFormOption));

        var req = new SaveAndPublishReq();
        req.Element = new SaveAndPublishElementReq();
        req.Element.Id = giving.Content().Key.ToString();
        req.Element.Type = ElementType.DonationForm;
        req.Element.Content = JObject.Parse(_jsonProvider.SerializeObject(giving.Content()));
        req.Element.PublishedContent = _mapper.Map<GivingContent, DonationFormElement>(giving);
        req.Partials = categoryPartialReqs.Concat(optionPartialReqs).ToList();

        return req;
    }

    private SaveAndPublishPartialReq GetSaveAndPublishPartialReq<TContent, TData>(TContent content, PartialType type)
        where TContent : UmbracoContent<TContent> {
        var req = new SaveAndPublishPartialReq();
        req.Id = content.Content().Key.ToString();
        req.Type = type;
        req.Content = JObject.Parse(_jsonProvider.SerializeObject(content.Content()));
        req.PublishedContent = _mapper.Map<TContent, TData>(content);

        return req;
    }
}