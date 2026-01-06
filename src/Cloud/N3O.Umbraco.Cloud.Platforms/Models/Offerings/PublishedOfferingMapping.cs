using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Markup;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Strings;
using AllocationType = N3O.Umbraco.Cloud.Platforms.Clients.AllocationType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedOfferingMapping : IMapDefinition {
    private readonly IMarkupEngine _markupEngine;
    private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;

    public PublishedOfferingMapping(IMarkupEngine markupEngine,
                                    IBaseCurrencyAccessor baseCurrencyAccessor) {
        _markupEngine = markupEngine;
        _baseCurrencyAccessor = baseCurrencyAccessor;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<OfferingContent, PublishedOffering>((_, _) => new PublishedOffering(), Map);
    }

    private void Map(OfferingContent src, PublishedOffering dest, MapperContext ctx) {
        var updateOfferingReq = ctx.Map<OfferingContent, UpdateOfferingReq>(src);
        
        var currency = _baseCurrencyAccessor.GetBaseCurrency().Code.ToEnum<Currency>();
        
        dest.Id = src.Content().Key.ToString();
        dest.Name = updateOfferingReq.Name;
        dest.Image = new PublishedImageContent();
        dest.Image.Format = PropertyFormat.Image;
        dest.Image.Main = new PublishedProcessedImage();
        dest.Image.Main.Url = new Uri(updateOfferingReq.Image.SourceFile);
        dest.Image.Main.Size = new PublishedSize();
        dest.Image.Main.Size.Width = updateOfferingReq.Image.Main.Crop.TopRight.X;
        dest.Image.Main.Size.Height = updateOfferingReq.Image.Main.Crop.BottomLeft.Y;
        
        dest.Icon = new PublishedSvgContent();
        dest.Icon.Url = new Uri(updateOfferingReq.Icon.SourceFile);
        dest.Icon.Format = PropertyFormat.Svg;
        
        dest.Description = new PublishedHtmlContent();
        dest.Description.Markup = _markupEngine.RenderHtml(updateOfferingReq.Description.Html).IfNotNull(x => new HtmlEncodedString(x.ToString())).ToHtmlString();
        dest.Description.Format = PropertyFormat.Html;
        
        dest.FormState = new PublishedDonationFormState();
        dest.FormState.CartItem = new PublishedCartItem();
        dest.FormState.CartItem.Type = CartItemType.NewDonation;
        dest.FormState.CartItem.Currency = currency;
        dest.FormState.CartItem.Value = new MoneyRes();
        dest.FormState.CartItem.Value.Currency = currency;
        dest.FormState.CartItem.Value.Amount = 0d;
        
        dest.FormState.CartItem.Type = CartItemType.NewDonation;

        var allocation = updateOfferingReq.FormState.CartItem.NewRegularGiving?.Allocation ?? updateOfferingReq.FormState.CartItem.NewDonation.Allocation;
        
        dest.FormState.CartItem.NewDonation = new PublishedNewDonation();
        dest.FormState.CartItem.NewDonation.Allocation = new PublishedAllocationIntent();
        dest.FormState.CartItem.NewDonation.Allocation.FundDimensions = GetPublishedFundDimensionValues(allocation);
        dest.FormState.CartItem.NewDonation.Allocation.Value = new Money();
        dest.FormState.CartItem.NewDonation.Allocation.Value.Currency = currency;
        dest.FormState.CartItem.NewDonation.Allocation.Value.Amount = 0d;

        dest.FormState.CartItem.NewDonation.Allocation.Type = allocation.Type;

        if (allocation.Type == AllocationType.Fund) {
            dest.FormState.CartItem.NewDonation.Allocation.Fund = new PublishedFundIntent();
            dest.FormState.CartItem.NewDonation.Allocation.Fund.DonationItem = allocation.Fund.DonationItem;
        } else if (allocation.Type == AllocationType.Feedback) {
            dest.FormState.CartItem.NewDonation.Allocation.Feedback = new PublishedFeedbackIntent();
            dest.FormState.CartItem.NewDonation.Allocation.Feedback.Scheme = allocation.Feedback.Scheme;
        } else if (allocation.Type == AllocationType.Sponsorship) {
            dest.FormState.CartItem.NewDonation.Allocation.Sponsorship = new PublishedSponsorshipIntent();
            dest.FormState.CartItem.NewDonation.Allocation.Sponsorship.Scheme = allocation.Sponsorship.Scheme;
        }

        if (allocation.Type == AllocationType.Fund) {
            dest.FormState.Options = new PublishedDonationFormOptions();
            dest.FormState.Options.SuggestedAmounts = updateOfferingReq.FormState.Options.SuggestedAmounts;
        }
    }
    
    private PublishedFundDimensionValues GetPublishedFundDimensionValues(AllocationIntentReq allocation) {
        var publishedFundDimensionValues = new PublishedFundDimensionValues();
        publishedFundDimensionValues.Dimension1 = allocation.FundDimensions.Dimension1;
        publishedFundDimensionValues.Dimension2 = allocation.FundDimensions.Dimension2;
        publishedFundDimensionValues.Dimension3 = allocation.FundDimensions.Dimension3;
        publishedFundDimensionValues.Dimension4 = allocation.FundDimensions.Dimension4;

        return publishedFundDimensionValues;
    }
}