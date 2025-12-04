using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public class DonateButtonElementValidator : ContentValidator {
    private static readonly string CampaignAlias = AliasHelper<ElementContent>.PropertyAlias(x => x.Campaign);
    private static readonly string OfferingAlias = AliasHelper<DesignatableElementContent<DonateButtonElementContent>>.PropertyAlias(x => x.Offering);
    private static readonly string Dimension1Alias = AliasHelper<DesignatableElementContent<DonateButtonElementContent>>.PropertyAlias(x => x.Dimension1);
    private static readonly string Dimension2Alias = AliasHelper<DesignatableElementContent<DonateButtonElementContent>>.PropertyAlias(x => x.Dimension2);
    private static readonly string Dimension3Alias = AliasHelper<DesignatableElementContent<DonateButtonElementContent>>.PropertyAlias(x => x.Dimension3);
    private static readonly string Dimension4Alias = AliasHelper<DesignatableElementContent<DonateButtonElementContent>>.PropertyAlias(x => x.Dimension4);
    private static readonly string AmountAlias = AliasHelper<DonateButtonElementContent>.PropertyAlias(x => x.Amount);
    private static readonly string ActionAlias = AliasHelper<DonateButtonElementContent>.PropertyAlias(x => x.Action);
    
    
    private readonly IContentHelper _contentHelper;
    private readonly IContentLocator _contentLocator;
    private readonly ILookups _lookups;

    public DonateButtonElementValidator(IContentHelper contentHelper,
                                        IContentLocator contentLocator,
                                        ILookups lookups) : base(contentHelper) {
        _contentHelper = contentHelper;
        _contentLocator = contentLocator;
        _lookups = lookups;
    }
    
    public override bool IsValidator(ContentProperties content) {
        return content.ContentTypeAlias == AliasHelper<DonateButtonElementContent>.ContentTypeAlias();
    }

    public override void Validate(ContentProperties content) {
        var offering = GetOfferingContent(content);
        var fundDimensionOptions = offering.GetFundDimensionOptions();
        
        var action = _contentHelper.GetDataListValue<DonateButtonAction>(content, ActionAlias);

        if (fundDimensionOptions != null) {
            DimensionAllowed(content, fundDimensionOptions.Dimension1, Dimension1Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension2, Dimension2Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension3, Dimension3Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension4, Dimension4Alias);
        }

        if (action.IsAnyOf(DonateButtonActions.AddToCart, DonateButtonActions.BeginCheckout)) {
            ValidateHasPricing(content, offering);
            ValidateHasFixedDimensions(content, offering);
        }
    }
    
    private OfferingContent GetOfferingContent(ContentProperties content) {
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
    
    private void DimensionAllowed<T>(ContentProperties content,
                                     IEnumerable<T> allowedValues,
                                     string propertyAlias)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetLookupValue<T>(_lookups, x));

        if (value != null && allowedValues != null && !allowedValues.Contains(value)) {
            ErrorResult(property, $"{value.Name} is not a permitted fund dimension value");
        }
    }

    private void ValidateHasPricing(ContentProperties content, OfferingContent offering) {
        var amount = content.GetPropertyValueByAlias<decimal?>(AmountAlias);

        if (!offering.HasPricing && !amount.HasValue()) {
            ErrorResult("Amount must be specified");
        }
        
        if (offering.HasPricing && amount.HasValue()) {
            ErrorResult("Offering has a pricing, amount cannot be specified");
        }
    }
    
    private void ValidateHasFixedDimensions(ContentProperties content, OfferingContent offering) {
        var publishedFundDimensionOptions = offering.ToOfferingFundDimensionReq();
        
        ValidateHasFixedDimension<FundDimension1Value>(content, publishedFundDimensionOptions.Dimension1?.Fixed, Dimension1Alias);
        ValidateHasFixedDimension<FundDimension2Value>(content, publishedFundDimensionOptions.Dimension2?.Fixed, Dimension2Alias);
        ValidateHasFixedDimension<FundDimension3Value>(content, publishedFundDimensionOptions.Dimension3?.Fixed, Dimension3Alias);
        ValidateHasFixedDimension<FundDimension4Value>(content, publishedFundDimensionOptions.Dimension4?.Fixed, Dimension4Alias);
    }
    
    private void ValidateHasFixedDimension<T>(ContentProperties content, string fixedDimensionValue, string propertyAlias)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetLookupValue<T>(_lookups, x));

        if (property.HasValue() && !fixedDimensionValue.HasValue() && !value.HasValue()) {
            ErrorResult(property, "Fund dimension value must be specified");
        }
    }
}