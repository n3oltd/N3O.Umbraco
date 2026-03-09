using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public abstract class DonationElementValidator<T> : ContentValidator where T : DonationElementContent<T> {
    private static readonly string CampaignAlias = AliasHelper<ElementContent>.PropertyAlias(x => x.Campaign);
    private static readonly string OfferingAlias = AliasHelper<DonationElementContent<T>>.PropertyAlias(x => x.Offering);
    protected static readonly string Dimension1Alias = AliasHelper<DonationElementContent<T>>.PropertyAlias(x => x.Dimension1);
    protected static readonly string Dimension2Alias = AliasHelper<DonationElementContent<T>>.PropertyAlias(x => x.Dimension2);
    protected static readonly string Dimension3Alias = AliasHelper<DonationElementContent<T>>.PropertyAlias(x => x.Dimension3);
    protected static readonly string Dimension4Alias = AliasHelper<DonationElementContent<T>>.PropertyAlias(x => x.Dimension4);

    private readonly IContentHelper _contentHelper;
    private readonly IContentLocator _contentLocator;
    private readonly ILookups _lookups;

    protected DonationElementValidator(IContentHelper contentHelper,
                                       IContentLocator contentLocator,
                                       ILookups lookups)
        : base(contentHelper) {
        _contentHelper = contentHelper;
        _contentLocator = contentLocator;
        _lookups = lookups;
    }
    
    public override bool IsValidator(ContentProperties content) {
        return content.ContentTypeAlias == AliasHelper<T>.ContentTypeAlias();
    }

    public override void Validate(ContentProperties content) {
        var offering = GetOfferingContent(content);
        var fundDimensionOptions = offering.GetFundDimensionOptions();

        if (fundDimensionOptions != null) {
            DimensionAllowed(content, fundDimensionOptions.Dimension1, Dimension1Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension2, Dimension2Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension3, Dimension3Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension4, Dimension4Alias);
        }
    }

    protected abstract void ValidateProperties(ContentProperties content);

    protected OfferingContent GetOfferingContent(ContentProperties content) {
        var campaign = _contentHelper.GetMultiNodeTreePickerValue<IPublishedContent>(content, CampaignAlias);
        var offering = _contentHelper.GetMultiNodeTreePickerValue<IPublishedContent>(content, OfferingAlias);

        if (campaign.HasValue()) {
            var publishedCampaign = _contentLocator.ById<CampaignContent>(campaign.Key);
            
            return publishedCampaign.DefaultOffering;
        } else if (offering.HasValue()) {
            var publishedOffering = _contentLocator.ById<OfferingContent>(offering.Key);
            
            return publishedOffering;
        } else {
            var defaultCampaign = _contentLocator.Single<PlatformsContent>().Campaigns.First();

            return defaultCampaign.DefaultOffering;
        }
    }
    
    private void DimensionAllowed<U>(ContentProperties content,
                                     IEnumerable<U> allowedValues,
                                     string propertyAlias)
        where U: FundDimensionValue<U> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetLookupValue<U>(_lookups, x));

        if (value != null && allowedValues != null && !allowedValues.Contains(value)) {
            ErrorResult(property, $"{value.Name} is not a permitted fund dimension value");
        }
    }
}