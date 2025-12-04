/*using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Markup;
using N3O.Umbraco.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;
using ElementType = N3O.Umbraco.Cloud.Platforms.Clients.ElementType;
using GiftType = N3O.Umbraco.Cloud.Platforms.Lookups.GiftType;
using OfferingType = N3O.Umbraco.Cloud.Platforms.Lookups.OfferingType;
using PublishedOfferingType = N3O.Umbraco.Cloud.Platforms.Clients.OfferingType;
using PublishedGiftType = N3O.Umbraco.Cloud.Platforms.Clients.GiftType;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class OfferingPreviewTagGenerator : PreviewTagGenerator {
    private readonly IJsonProvider _jsonProvider;
    private readonly IMediaUrl _mediaUrl;
    private readonly ILookups _lookups;
    protected readonly IUmbracoMapper Mapper;
    private readonly IMarkupEngine _markupEngine;
    private readonly IMediaLocator _mediaLocator;
    private readonly IPublishedValueFallback _publishedValueFallback;

    protected OfferingPreviewTagGenerator(ICdnClient cdnClient,
                                             IJsonProvider jsonProvider,
                                             IMediaUrl mediaUrl,
                                             ILookups lookups,
                                             IUmbracoMapper mapper,
                                             IMarkupEngine markupEngine,
                                             IMediaLocator mediaLocator,
                                             IPublishedValueFallback publishedValueFallback)
        : base(cdnClient, jsonProvider, lookups) {
        _jsonProvider = jsonProvider;
        _mediaUrl = mediaUrl;
        _lookups = lookups;
        Mapper = mapper;
        _markupEngine = markupEngine;
        _mediaLocator = mediaLocator;
        _publishedValueFallback = publishedValueFallback;
    }
    
    protected abstract OfferingType OfferingType { get; }

    protected override string ContentTypeAlias => OfferingType.ContentTypeAlias;

    protected override void PopulatePreviewData(IReadOnlyDictionary<string, object> content,
                                                Dictionary<string, object> previewData) {
        var image = GetMediaWithCrops(content, AliasHelper<OfferingContent>.PropertyAlias(x => x.Image));
        var icon = GetMediaWithCrops(content, AliasHelper<OfferingContent>.PropertyAlias(x => x.Icon));
        var shortDescription = content[AliasHelper<OfferingContent>.PropertyAlias(x => x.Description)]?.ToString();
        var longDescription = content[AliasHelper<OfferingContent>.PropertyAlias(x => x.LongDescription)]?.ToString();
        var suggestedGiftType = GetDataListValue<GiftType>(content, AliasHelper<OfferingContent>.PropertyAlias(x => x.SuggestedGiftType));
        
        var publishedOffering = new CreateOfferingReq();
        publishedOffering.Id = content[AliasHelper<OfferingContent>.PropertyAlias(x => x.Key)].ToString();
        publishedOffering.Name = content[AliasHelper<IPublishedContent>.PropertyAlias(x => x.Name)]?.ToString();
        publishedOffering.Type = GetPublishedOfferingType();
        publishedOffering.Image = _mediaUrl.GetMediaUrl(image, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
        publishedOffering.Icon = _mediaUrl.GetMediaUrl(icon, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
        publishedOffering.ShortDescription = _markupEngine.RenderHtml(shortDescription).IfNotNull(x => new HtmlEncodedString(x.ToString())).ToHtmlString();
        publishedOffering.LongDescription = _markupEngine.RenderHtml(longDescription).IfNotNull(x => new HtmlEncodedString(x.ToString())).ToHtmlString();
        publishedOffering.SuggestedGiftType = suggestedGiftType.ToEnum<PublishedGiftType>();
        
        publishedOffering.FundDimensions = GetPublishedOfferingFundDimensions(content);
        publishedOffering.GiftTypes = GetPublishedSuggestedGiftTypes(content).ToList();

        PopulatePublishedOffering(content, publishedOffering);
        
        var publishedDonationForm = new PublishedDonationForm();
        publishedDonationForm.Id = content[AliasHelper<OfferingContent>.PropertyAlias(x => x.Key)].ToString();
        publishedDonationForm.Type = ElementType.DonationForm;
        publishedDonationForm.Offering = publishedOffering;

        previewData["publishedForm"] = publishedDonationForm;
        
        PopulateAdditionalData(previewData, publishedDonationForm);
    }

    private PublishedOfferingType? GetPublishedOfferingType() {
        var offeringType = StaticLookups.GetAll<OfferingType>().Single(x => x.ContentTypeAlias.EqualsInvariant(ContentTypeAlias));
        
        return offeringType.ToEnum<PublishedOfferingType>();
    }

    private PublishedOfferingFundDimensions GetPublishedOfferingFundDimensions(IReadOnlyDictionary<string, object> content) {
        var dimension1 = GetDataListValue<FundDimension1Value>(content, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension1));
        var dimension2 = GetDataListValue<FundDimension2Value>(content, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension2));
        var dimension3 = GetDataListValue<FundDimension3Value>(content, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension3));
        var dimension4 = GetDataListValue<FundDimension4Value>(content, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension4));
        
        var fundDimensionOptions = GetFundDimensionOptions(content);
        
        var publishedFundDimensions = new PublishedOfferingFundDimensions();
        publishedFundDimensions.Dimension1 = dimension1.ToPublishedOfferingFundDimension(fundDimensionOptions?.Dimension1);
        
        publishedFundDimensions.Dimension2 = dimension2.ToPublishedOfferingFundDimension(fundDimensionOptions?.Dimension2);
        
        publishedFundDimensions.Dimension3 = dimension3.ToPublishedOfferingFundDimension(fundDimensionOptions?.Dimension3);
        
        publishedFundDimensions.Dimension4 = dimension4.ToPublishedOfferingFundDimension(fundDimensionOptions?.Dimension4);

        return publishedFundDimensions;
    }

    private MediaWithCrops GetMediaWithCrops(IReadOnlyDictionary<string, object> content, string alias) {
        var mediaDto = _jsonProvider.DeserializeObject<IEnumerable<MediaWithCropsDto>>(content[alias]?.ToString()).FirstOrDefault();

        if (!mediaDto.HasValue()) {
            return null;
        }

        return mediaDto?.ToMediaWithCrops(_publishedValueFallback, _mediaLocator);
    }
    
    protected abstract void PopulatePublishedOffering(IReadOnlyDictionary<string, object> content,
                                                         PublishedOffering publishedOffering);
    
    protected abstract IFundDimensionOptions GetFundDimensionOptions(IReadOnlyDictionary<string, object> content);
    protected abstract IEnumerable<PublishedGiftType> GetPublishedSuggestedGiftTypes(IReadOnlyDictionary<string, object> content);

    protected virtual void PopulateAdditionalData(Dictionary<string, object> previewData,
                                                  PublishedDonationForm publishedDonationForm) { }
}*/