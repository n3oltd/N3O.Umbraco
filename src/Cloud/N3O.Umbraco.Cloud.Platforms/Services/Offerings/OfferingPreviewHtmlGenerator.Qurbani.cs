using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Markup;
using N3O.Umbraco.Media;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using AllocationType = N3O.Umbraco.Cloud.Platforms.Clients.AllocationType;
using OurAllocationType = N3O.Umbraco.Giving.Allocations.Lookups.AllocationType;
using PublishedFundDimensionValues = N3O.Umbraco.Cloud.Platforms.Clients.PublishedFundDimensionValues;
using PublishedPrice = N3O.Umbraco.Cloud.Platforms.Clients.PublishedPrice;
using PublishedQurbaniItem = N3O.Umbraco.Cloud.Platforms.Clients.PublishedQurbaniItem;

namespace N3O.Umbraco.Cloud.Platforms;

public class QurbaniOfferingPreviewHtmlGenerator : OfferingPreviewHtmlGenerator {
    private readonly ILookups _lookups;

    public QurbaniOfferingPreviewHtmlGenerator(ICdnClient cdnClient,
                                               IJsonProvider jsonProvider,
                                               IMediaUrl mediaUrl,
                                               ILookups lookups,
                                               IMarkupEngine markupEngine,
                                               IMediaLocator mediaLocator,
                                               IPublishedValueFallback publishedValueFallback,
                                               IBaseCurrencyAccessor baseCurrencyAccessor)
        : base(cdnClient,
               jsonProvider,
               mediaUrl,
               lookups,
               markupEngine,
               mediaLocator,
               publishedValueFallback,
               baseCurrencyAccessor) {
        _lookups = lookups;
    }

    protected override OurAllocationType OfferingAllocationType => AllocationTypes.Qurbani;

    protected override void PopulateAllocationIntent(IReadOnlyDictionary<string, object> content,
                                                     PublishedAllocationIntent allocationIntent) {
        var qurbaniItem = GetQurbaniItem(content);

        allocationIntent.Type = AllocationType.Qurbani;
        allocationIntent.Qurbani = new PublishedQurbaniIntent();
        allocationIntent.Qurbani.New = new PublishedNewQurbaniIntent();
        allocationIntent.Qurbani.New.Item = qurbaniItem.Id;
    }

    protected override void PopulateAdditionalData(Dictionary<string, object> previewData,
                                                   PublishedDonationForm publishedDonationForm) {
        var qurbaniItem = _lookups.FindById<QurbaniItem>(publishedDonationForm.FormState.CartItem.NewDonation.Allocation.Qurbani.New.Item);

        var publishedFundDimensionValues = new PublishedFundDimensionValues();
        publishedFundDimensionValues.Dimension1 = qurbaniItem.FundDimensionValues?.Dimension1?.Name;
        publishedFundDimensionValues.Dimension2 = qurbaniItem.FundDimensionValues?.Dimension2?.Name;
        publishedFundDimensionValues.Dimension3 = qurbaniItem.FundDimensionValues?.Dimension3?.Name;
        publishedFundDimensionValues.Dimension4 = qurbaniItem.FundDimensionValues?.Dimension4?.Name;

        var publishedQurbaniItem = new PublishedQurbaniItem();
        publishedQurbaniItem.Id = qurbaniItem.Id;
        publishedQurbaniItem.Name = qurbaniItem.Name;
        publishedQurbaniItem.FundDimensionValues = publishedFundDimensionValues;

        if (qurbaniItem.Price.HasValue()) {
            publishedQurbaniItem.Price = new PublishedPrice();
            publishedQurbaniItem.Price.Amount = (double) qurbaniItem.Price.Amount;
            publishedQurbaniItem.Price.Locked = true;
        }

        previewData["qurbaniItem"] = publishedQurbaniItem;
    }

    protected override IFundDimensionOptions GetFundDimensionOptions(IReadOnlyDictionary<string, object> content) {
        return null;
    }

    private QurbaniItem GetQurbaniItem(IReadOnlyDictionary<string, object> content) {
        return GetDataListValue<QurbaniItem>(content, AliasHelper<QurbaniDonationFormStateContent>.PropertyAlias(x => x.QurbaniItem));
    }
}
