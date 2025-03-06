using N3O.Headless.Communications.Clients;
using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Engage;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Elements.Content;
using N3O.Umbraco.Elements.Extensions;
using N3O.Umbraco.Elements.Models;
using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using CurrecncyContent = N3O.Umbraco.Financial.Currency;

namespace N3O.Umbraco.Elements;

public class ElementsManager : IElementsManager {
    private readonly ClientFactory<ElementsClient> _elementsClientFactory;
    private readonly ClientFactory<CommunicationsClient> _communicationsClientFactory;
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private readonly IContentLocator _contentLocator;
    private readonly IJsonProvider _jsonProvider;
    private readonly IUmbracoMapper _mapper;

    public ElementsManager(ClientFactory<ElementsClient> elementsClientFactory,
                           ISubscriptionAccessor subscriptionAccessor,
                           IContentLocator contentLocator,
                           IJsonProvider jsonProvider,
                           IUmbracoMapper mapper,
                           ClientFactory<CommunicationsClient> communicationsClientFactory) {
        _elementsClientFactory = elementsClientFactory;
        _subscriptionAccessor = subscriptionAccessor;
        _contentLocator = contentLocator;
        _jsonProvider = jsonProvider;
        _mapper = mapper;
        _communicationsClientFactory = communicationsClientFactory;
    }
    
    public async Task SaveAndPublishCheckoutProfileAsync() {
        var subscription = _subscriptionAccessor.GetSubscription();
        var client = await _elementsClientFactory.CreateAsync(subscription, ClientTypes.BackOffice);
        
        var checkoutProfile = await GetCheckoutProfileAsync();
        
        var req = new SaveAndPublishReq();
        req.Element = new SaveAndPublishElementReq();
        req.Element.Id = checkoutProfile.Id;
        req.Element.Type = ElementType.CheckoutProfile;
        req.Element.Content = checkoutProfile;
        req.Element.PublishedContent = checkoutProfile;
        
        await client.InvokeAsync(x => x.SaveAndPublishElementAsync, req);
    }

    public async Task SaveAndPublishDonationFormAsync() {
        var subscription = _subscriptionAccessor.GetSubscription();
        var client = await _elementsClientFactory.CreateAsync(subscription, ClientTypes.BackOffice);
        
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
        
        await client.InvokeAsync(x => x.SaveAndPublishElementAsync, req);
    }

    public async Task SaveAndPublishElementsSettingsAsync() {
        var settings = _contentLocator.Single<ElementsSettingsContent>();
        var subscription = _subscriptionAccessor.GetSubscription();
        var client = await _elementsClientFactory.CreateAsync(subscription, ClientTypes.BackOffice);
        
        var req = new SaveAndPublishReq();
        req.Element = new SaveAndPublishElementReq();
        req.Element.Id = settings.Content().Key.ToString();
        req.Element.Type = ElementType.Configuration;
        req.Element.Content = JObject.Parse(_jsonProvider.SerializeObject(settings.Content()));
        req.Element.PublishedContent = settings.ToConfigurationElement();
        req.Element.CustomPath = "configuration.json";
        
        await client.InvokeAsync(x => x.SaveAndPublishElementAsync, req);
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
    
    private async Task<CheckoutProfile> GetCheckoutProfileAsync() {
        var dataEntrySettingsContent = _contentLocator.Single<DataEntrySettingsContent>();
        var organisationSettings = _contentLocator.Single<OrganisationDataEntrySettingsContent>();
        var paymentSettings = _contentLocator.Single<PaymentMethodDataEntrySettingsContent>();
        var termsOfServiceSettings = _contentLocator.Single<TermsDataEntrySettingsContent>();
        var currencies = _contentLocator.All<CurrecncyContent>();
        var checkoutCompleteSettings = _contentLocator.Single<CheckoutCompleteSettingsContent>();
        var preferences = await GetPreferencesAsync();
        
        var checkoutProfile = new CheckoutProfile();
        checkoutProfile.Id = dataEntrySettingsContent.Content().Key.ToString();
        checkoutProfile.Accounts = _mapper.Map<DataEntrySettingsContent, AccountEntrySettings>(dataEntrySettingsContent);
        checkoutProfile.Branding = _mapper.Map<OrganisationDataEntrySettingsContent, BrandingSettings>(organisationSettings);
        checkoutProfile.Payments = _mapper.Map<PaymentMethodDataEntrySettingsContent, PaymentsSettings>(paymentSettings);
        checkoutProfile.TermsOfService = _mapper.Map<TermsDataEntrySettingsContent, TermsOfServiceSettings>(termsOfServiceSettings);
        checkoutProfile.AllowedCurrencies = currencies.Select(x => GetCurrencyByCode(x.Code)).ToList();
        checkoutProfile.Adverts = checkoutCompleteSettings.Adverts
                                                          .Select(_mapper.Map<AdvertContentElement, AdvertSettings>)
                                                          .ToList();
        
        // TODO need to go after <DataEntrySettingsContent, AccountEntrySettings> as consent being set to default
        checkoutProfile.Accounts.Preferences = _mapper.Map<PreferencesStructureRes, PreferencesSettings>(preferences);
        
        return checkoutProfile;
    }

    private Currency GetCurrencyByCode(string code) {
        return (Currency) Enum.Parse(typeof(Currency), code, true);
    }

    private async Task<PreferencesStructureRes> GetPreferencesAsync() {
        var subscription = _subscriptionAccessor.GetSubscription();
        var client = await _communicationsClientFactory.CreateAsync(subscription, ClientTypes.BackOffice);

        var preferences = await client.InvokeAsync<PreferencesStructureRes>(x => x.GetPreferencesStructureAsync);
        
        return preferences;
    }
}