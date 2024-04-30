using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Extensions;
using N3O.Umbraco.Webhooks.Models;
using N3O.Umbraco.Webhooks.Receivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using static N3O.Umbraco.Giving.GivingConstants.Webhooks;

namespace N3O.Umbraco.Giving.Webhooks;

[WebhookReceiver(HookIds.DonationItem)]
public class DonationItemReceiver : WebhookReceiver {
    private readonly IJsonProvider _jsonProvider;
    private readonly IContentCache _contentCache;
    private readonly IContentEditor _contentEditor;
    private readonly IContentService _contentService;
    private readonly IContentHelper _contentHelper;
    private readonly ILookups _lookups;
    private readonly AsyncKeyedLocker<string> _locker;

    public DonationItemReceiver(IJsonProvider jsonProvider,
                                IContentCache contentCache,
                                IContentEditor contentEditor,
                                IContentService contentService,
                                IContentHelper contentHelper,
                                ILookups lookups,
                                AsyncKeyedLocker<string> locker) {
        _jsonProvider = jsonProvider;
        _contentCache = contentCache;
        _contentEditor = contentEditor;
        _contentService = contentService;
        _contentHelper = contentHelper;
        _lookups = lookups;
        _locker = locker;
    }

    protected override async Task ProcessAsync(WebhookPayload payload, CancellationToken cancellationToken) {
        var webhookDonationItem = payload.GetBody<WebhookDonationItem>(_jsonProvider);

        using (await _locker.LockAsync(webhookDonationItem.Name, cancellationToken)) {
            var eventType = payload.GetEventType();

            switch (eventType) {
                case EventTypes.Published:
                    CreateOrUpdate(payload, webhookDonationItem);
                    break;

                case EventTypes.Unpublished:
                    Unpublish(payload, webhookDonationItem);
                    break;
            }
        }
    }

    private void CreateOrUpdate(WebhookPayload payload, WebhookDonationItem webhookDonationItem) {
        var collection = _contentCache.Single<DonationItemsContent>();
        var existingContent = GetExistingContent(webhookDonationItem.Name, payload.GetHeader(Headers.PreviousName));

        var contentPublisher = existingContent != null
                               ? _contentEditor.ForExisting(existingContent.Key)
                               : _contentEditor.New(webhookDonationItem.Name,
                                                    collection.Content().Key,
                                                    GivingConstants.Aliases.DonationItem.ContentType);
        
        var allowedGivingTypes = ToLookups<GivingType>(webhookDonationItem.AllowedGivingTypes);
        var dimension1Options = GetLookupsByName<FundDimension1Value>(webhookDonationItem.FundDimensionOptions.Dimension1);
        var dimension2Options = GetLookupsByName<FundDimension2Value>(webhookDonationItem.FundDimensionOptions.Dimension2);
        var dimension3Options = GetLookupsByName<FundDimension3Value>(webhookDonationItem.FundDimensionOptions.Dimension3);
        var dimension4Options = GetLookupsByName<FundDimension4Value>(webhookDonationItem.FundDimensionOptions.Dimension4);

        contentPublisher.SetName(webhookDonationItem.Name);
        contentPublisher.Content.DataList(GivingConstants.Aliases.DonationItem.Properties.AllowedGivingTypes).SetLookups(allowedGivingTypes);
        contentPublisher.Content.ContentPicker(GivingConstants.Aliases.DonationItem.Properties.Dimension1Options).SetContent(dimension1Options);
        contentPublisher.Content.ContentPicker(GivingConstants.Aliases.DonationItem.Properties.Dimension2Options).SetContent(dimension2Options);
        contentPublisher.Content.ContentPicker(GivingConstants.Aliases.DonationItem.Properties.Dimension3Options).SetContent(dimension3Options);
        contentPublisher.Content.ContentPicker(GivingConstants.Aliases.DonationItem.Properties.Dimension4Options).SetContent(dimension4Options);
        contentPublisher.Content.Numeric(GivingConstants.Aliases.Price.Properties.Amount).SetDecimal(webhookDonationItem.Price?.Amount);
        contentPublisher.Content.Toggle(GivingConstants.Aliases.Price.Properties.Locked).Set(webhookDonationItem.Price?.Locked);

        if (webhookDonationItem.PriceRules.HasAny()) {
            var nestedContent = contentPublisher.Content.Nested(GivingConstants.Aliases.DonationItem.Properties.PriceRules);
            
            foreach (var priceRule in webhookDonationItem.PriceRules) {
                AddPriceRule(nestedContent.Add(GivingConstants.Aliases.PricingRule.ContentType), priceRule);
            }
        } else {
            contentPublisher.Content.Null(GivingConstants.Aliases.DonationItem.Properties.PriceRules);
        }

        contentPublisher.SaveAndPublish();
    }

    private void Unpublish(WebhookPayload payload, WebhookDonationItem webhookDonationItem) {
        var existingContent = GetExistingContent(webhookDonationItem.Name, payload.GetHeader(Headers.PreviousName));

        if (existingContent != null) {
            _contentEditor.ForExisting(existingContent.Key).Unpublish();
        }
    }

    private IContent GetExistingContent(string name, string previousName) {
        var collectionPublished = _contentCache.Single<DonationItemsContent>();
        var collection = _contentService.GetById(collectionPublished.Content().Key);
        var donationItems = _contentHelper.GetChildren(collection);
        var matches = donationItems.Where(x => x.Name.EqualsInvariant(name) || x.Name.EqualsInvariant(previousName))
                                   .ToList();

        if (matches.IsSingle()) {
            return matches.Single();
        } else if (matches.None()) {
            return null;
        } else {
            throw new Exception($"More than one donation item found: {matches.Select(x => x.Name).ToCsv(true)}");
        }
    }

    private IReadOnlyList<T> ToLookups<T>(IEnumerable<WebhookLookup> webhookLookups) where T : ILookup {
        return webhookLookups.OrEmpty().Select(x => _lookups.FindById<T>(x.Id)).ExceptNull().ToList();
    }
    
    private IReadOnlyList<T> GetLookupsByName<T>(IEnumerable<string> names) where T : ILookup {
        return names.OrEmpty().SelectMany(x => _lookups.FindByName<T>(x)).ExceptNull().ToList();
    }

    private void AddPriceRule(IContentBuilder contentBuilder, WebhookPricingRule webhookPricingRule) {
        contentBuilder.Numeric(GivingConstants.Aliases.Price.Properties.Amount).SetDecimal(webhookPricingRule.Price?.Amount);
        contentBuilder.Toggle(GivingConstants.Aliases.Price.Properties.Locked).Set(webhookPricingRule.Price?.Locked);
        contentBuilder.ContentPicker(GivingConstants.Aliases.PricingRule.Properties.Dimension1).SetContent(webhookPricingRule.FundDimensions.Dimension1.IfNotNull(_lookups.FindByName<FundDimension1Value>));
        contentBuilder.ContentPicker(GivingConstants.Aliases.PricingRule.Properties.Dimension2).SetContent(webhookPricingRule.FundDimensions.Dimension2.IfNotNull(_lookups.FindByName<FundDimension2Value>));
        contentBuilder.ContentPicker(GivingConstants.Aliases.PricingRule.Properties.Dimension3).SetContent(webhookPricingRule.FundDimensions.Dimension3.IfNotNull(_lookups.FindByName<FundDimension3Value>));
        contentBuilder.ContentPicker(GivingConstants.Aliases.PricingRule.Properties.Dimension4).SetContent(webhookPricingRule.FundDimensions.Dimension4.IfNotNull(_lookups.FindByName<FundDimension4Value>));
    }
    
    public class WebhookDonationItem : WebhookEntity {
        public WebhookDonationItem(WebhookRevision revision,
                                   WebhookReference reference,
                                   string name,
                                   IEnumerable<WebhookLookup> allowedGivingTypes,
                                   WebhookFundDimensionOptions fundDimensionOptions,
                                   WebhookPrice price,
                                   IEnumerable<WebhookPricingRule> priceRules,
                                   string status,
                                   bool isActive)
            : base(revision, reference) {
            Name = name;
            AllowedGivingTypes = allowedGivingTypes;
            FundDimensionOptions = fundDimensionOptions;
            Price = price;
            PriceRules = priceRules;
            Status = status;
            IsActive = isActive;
        }

        public string Name { get; }
        public IEnumerable<WebhookLookup> AllowedGivingTypes { get; }
        public WebhookFundDimensionOptions FundDimensionOptions { get; }
        public WebhookPrice Price { get; }
        public IEnumerable<WebhookPricingRule> PriceRules { get; }
        public string Status { get; }
        public bool IsActive { get; }

        protected override IEnumerable<object> GetValues() {
            yield return Name;
            yield return AllowedGivingTypes;
            yield return FundDimensionOptions;
            yield return Price;
            yield return PriceRules;
            yield return Status;
            yield return IsActive;
        }
    }
}
