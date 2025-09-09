using N3O.Umbraco.Cloud.Platforms.Clients;
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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;
using DesignationType = N3O.Umbraco.Cloud.Platforms.Lookups.DesignationType;
using ElementType = N3O.Umbraco.Cloud.Platforms.Clients.ElementType;
using GiftType = N3O.Umbraco.Cloud.Platforms.Lookups.GiftType;
using PublishedDesignationType = N3O.Umbraco.Cloud.Platforms.Clients.DesignationType;
using PublishedGiftType = N3O.Umbraco.Cloud.Platforms.Clients.GiftType;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class DesignationPreviewTagGenerator : PreviewTagGenerator {
    private readonly IJsonProvider _jsonProvider;
    private readonly IMediaUrl _mediaUrl;
    private readonly ILookups _lookups;
    protected readonly IUmbracoMapper Mapper;
    private readonly IMarkupEngine _markupEngine;
    private readonly IMediaLocator _mediaLocator;
    private readonly IPublishedValueFallback _publishedValueFallback;

    protected DesignationPreviewTagGenerator(ICdnClient cdnClient,
                                             IJsonProvider jsonProvider,
                                             IMediaUrl mediaUrl,
                                             ILookups lookups,
                                             IUmbracoMapper mapper,
                                             IMarkupEngine markupEngine,
                                             IMediaLocator mediaLocator,
                                             IPublishedValueFallback publishedValueFallback)
        : base(cdnClient, jsonProvider) {
        _jsonProvider = jsonProvider;
        _mediaUrl = mediaUrl;
        _lookups = lookups;
        Mapper = mapper;
        _markupEngine = markupEngine;
        _mediaLocator = mediaLocator;
        _publishedValueFallback = publishedValueFallback;
    }
    
    protected abstract DesignationType DesignationType { get; }

    protected override string ContentTypeAlias => DesignationType.ContentTypeAlias;

    protected override void PopulatePreviewData(IReadOnlyDictionary<string, object> content,
                                                Dictionary<string, object> previewData) {
        var image = GetMediaWithCrops(content, AliasHelper<DesignationContent>.PropertyAlias(x => x.Image));
        var icon = GetMediaWithCrops(content, AliasHelper<DesignationContent>.PropertyAlias(x => x.Icon));
        var shortDescription = content[AliasHelper<DesignationContent>.PropertyAlias(x => x.ShortDescription)]?.ToString();
        var longDescription = content[AliasHelper<DesignationContent>.PropertyAlias(x => x.LongDescription)]?.ToString();
        var suggestedGiftType = GetDataListValue<GiftType>(content, AliasHelper<DesignationContent>.PropertyAlias(x => x.SuggestedGiftType));
        
        var publishedDesignation = new PublishedDesignation();
        publishedDesignation.Id = Guid.NewGuid().ToString();
        publishedDesignation.Name = content[AliasHelper<IPublishedContent>.PropertyAlias(x => x.Name)]?.ToString();
        publishedDesignation.Type = GetPublishedDesignationType();
        publishedDesignation.Image = _mediaUrl.GetMediaUrl(image, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
        publishedDesignation.Icon = _mediaUrl.GetMediaUrl(icon, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
        publishedDesignation.ShortDescription = _markupEngine.RenderHtml(shortDescription).IfNotNull(x => new HtmlEncodedString(x.ToString())).ToHtmlString();
        publishedDesignation.LongDescription = _markupEngine.RenderHtml(longDescription).IfNotNull(x => new HtmlEncodedString(x.ToString())).ToHtmlString();
        publishedDesignation.SuggestedGiftType = suggestedGiftType.ToEnum<PublishedGiftType>();
        
        publishedDesignation.FundDimensions = GetPublishedDesignationFundDimensions(content);
        publishedDesignation.GiftTypes = GetPublishedSuggestedGiftTypes(content).ToList();

        PopulatePublishedDesignation(content, publishedDesignation);
        
        var publishedDonationForm = new PublishedDonationForm();
        publishedDonationForm.Id = content[AliasHelper<DesignationContent>.PropertyAlias(x => x.Key)].ToString();
        publishedDonationForm.Type = ElementType.DonationForm;
        publishedDonationForm.Designation = publishedDesignation;

        previewData["publishedForm"] = publishedDonationForm;
    }

    private PublishedDesignationType? GetPublishedDesignationType() {
        var designationType = StaticLookups.GetAll<DesignationType>().Single(x => x.ContentTypeAlias.EqualsInvariant(ContentTypeAlias));
        
        return designationType.ToEnum<PublishedDesignationType>();
    }

    private PublishedDesignationFundDimensions GetPublishedDesignationFundDimensions(IReadOnlyDictionary<string, object> content) {
        var dimension1 = GetDataListValue<FundDimension1Value>(content, AliasHelper<DesignationContent>.PropertyAlias(x => x.Dimension1));
        var dimension2 = GetDataListValue<FundDimension2Value>(content, AliasHelper<DesignationContent>.PropertyAlias(x => x.Dimension2));
        var dimension3 = GetDataListValue<FundDimension3Value>(content, AliasHelper<DesignationContent>.PropertyAlias(x => x.Dimension3));
        var dimension4 = GetDataListValue<FundDimension4Value>(content, AliasHelper<DesignationContent>.PropertyAlias(x => x.Dimension4));
        
        var fundDimensionOptions = GetFundDimensionOptions(content);
        
        var publishedFundDimensions = new PublishedDesignationFundDimensions();
        publishedFundDimensions.Dimension1 = dimension1.ToPublishedDesignationFundDimension(fundDimensionOptions?.Dimension1);
        
        publishedFundDimensions.Dimension2 = dimension2.ToPublishedDesignationFundDimension(fundDimensionOptions?.Dimension2);
        
        publishedFundDimensions.Dimension3 = dimension3.ToPublishedDesignationFundDimension(fundDimensionOptions?.Dimension3);
        
        publishedFundDimensions.Dimension4 = dimension4.ToPublishedDesignationFundDimension(fundDimensionOptions?.Dimension4);

        return publishedFundDimensions;
    }
    
    protected T GetDataListValue<T>(IReadOnlyDictionary<string, object> content, string alias) where T : ILookup {
        if (content.ContainsKey(alias)) {
            var strValue = content[alias].ToString();

            if (strValue.HasValue() && strValue != "[]") {
                var id = JArray.Parse(strValue)[0].ToString();
                
                return _lookups.FindById<T>(id);
            }
        }

        return default;
    }

    private MediaWithCrops GetMediaWithCrops(IReadOnlyDictionary<string, object> content, string alias) {
        var mediaDto = _jsonProvider.DeserializeObject<IEnumerable<MediaWithCropsDto>>(content[alias]?.ToString()).FirstOrDefault();

        if (!mediaDto.HasValue()) {
            return null;
        }

        return mediaDto?.ToMediaWithCrops(_publishedValueFallback, _mediaLocator);
    }
    
    protected abstract void PopulatePublishedDesignation(IReadOnlyDictionary<string, object> content,
                                                         PublishedDesignation publishedDesignation);
    
    protected abstract IFundDimensionOptions GetFundDimensionOptions(IReadOnlyDictionary<string, object> content);
    protected abstract IEnumerable<PublishedGiftType> GetPublishedSuggestedGiftTypes(IReadOnlyDictionary<string, object> content);
}