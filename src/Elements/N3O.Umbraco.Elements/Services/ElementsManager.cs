using N3O.Umbraco.Accounts.Content;
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
        
        var req = GetDonationFormSaveAndPublishReq();
        
        await client.InvokeAsync(x => x.SaveAndPublishElementAsync, req);
    }

    public async Task SaveAndPublishCheckoutProfileAsync() {
        var subscription = _subscriptionAccessor.GetSubscription();
        var client = await _clientFactory.CreateAsync(subscription, ClientTypes.BackOffice);
        
        var req = GetCheckoutProfileSaveAndPublishReq();
        
        await client.InvokeAsync(x => x.SaveAndPublishElementAsync, req);
    }

    private SaveAndPublishReq GetCheckoutProfileSaveAndPublishReq() {
        var checkoutProfile = GetCheckoutProfile();
        
        var req = new SaveAndPublishReq();
        req.Element = new SaveAndPublishElementReq();
        req.Element.Id = checkoutProfile.Id;
        req.Element.Type = ElementType.CheckoutProfile;
        req.Element.Content = checkoutProfile;
        req.Element.PublishedContent = checkoutProfile;
        
        return req;
    }

    private CheckoutProfile GetCheckoutProfile() {
        var dataEntrySettingsContent = _contentLocator.Single<DataEntrySettingsContent>();
        var organisationSettings = _contentLocator.Single<OrganisationDataEntrySettingsContent>();
        var paymentSettings = _contentLocator.Single<PaymentMethodDataEntrySettingsContent>();
        var termsOfServiceSettings = _contentLocator.Single<TermsDataEntrySettingsContent>();
        
        var checkoutProfile = new CheckoutProfile();
        checkoutProfile.Id = dataEntrySettingsContent.Content().Key.ToString();
        checkoutProfile.Accounts = _mapper.Map<DataEntrySettingsContent, AccountEntrySettings>(dataEntrySettingsContent);
        checkoutProfile.Branding = _mapper.Map<OrganisationDataEntrySettingsContent, BrandingSettings>(organisationSettings);
        checkoutProfile.Payments = _mapper.Map<PaymentMethodDataEntrySettingsContent, PaymentsSettings>(paymentSettings);
        checkoutProfile.TermsOfService = _mapper.Map<TermsDataEntrySettingsContent, TermsOfServiceSettings>(termsOfServiceSettings);
        
        return checkoutProfile;
    }

    private SaveAndPublishReq GetDonationFormSaveAndPublishReq() {
        var giving = _contentLocator.Single<GivingContent>();

        var categoryPartialReqs = giving.AllCategories.Select(x => GetDonationFormSaveAndPublishPartialReq<DonationCategoryContent, DonationCategoryPartial>(x, PartialType.DonationFormCategory));
        var optionPartialReqs = giving.AllOptions.Select(x => GetDonationFormSaveAndPublishPartialReq<DonationOptionContent, DonationOptionPartial>(x, PartialType.DonationFormOption));

        var req = new SaveAndPublishReq();
        req.Element = new SaveAndPublishElementReq();
        req.Element.Id = giving.Content().Key.ToString();
        req.Element.Type = ElementType.DonationForm;
        req.Element.Content = JObject.Parse(_jsonProvider.SerializeObject(giving.Content()));
        req.Element.PublishedContent = _mapper.Map<GivingContent, DonationFormElement>(giving);
        req.Partials = categoryPartialReqs.Concat(optionPartialReqs).ToList();

        return req;
    }

    private SaveAndPublishPartialReq GetDonationFormSaveAndPublishPartialReq<TContent, TData>(TContent content, PartialType type)
        where TContent : UmbracoContent<TContent> {
        var req = new SaveAndPublishPartialReq();
        req.Id = content.Content().Key.ToString();
        req.Type = type;
        req.Content = JObject.Parse(_jsonProvider.SerializeObject(content.Content()));
        req.PublishedContent = _mapper.Map<TContent, TData>(content);

        return req;
    }
}