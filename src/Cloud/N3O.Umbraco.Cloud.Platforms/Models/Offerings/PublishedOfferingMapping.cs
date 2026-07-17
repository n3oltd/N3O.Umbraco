using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Strings;
using AllocationType = N3O.Umbraco.Cloud.Platforms.Clients.AllocationType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedOfferingMapping : IMapDefinition {
    private readonly IMarkupEngine _markupEngine;
    private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;

    public PublishedOfferingMapping(IMarkupEngine markupEngine, IBaseCurrencyAccessor baseCurrencyAccessor) {
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
        dest.FormContent = new PublishedDonationFormContent();
        dest.FormContent.Image = new PublishedImageContent();
        dest.FormContent.Image.Format = PropertyFormat.Image;
        dest.FormContent.Image.Main = new PublishedProcessedImage();
        dest.FormContent.Image.Main.Url = new Uri(updateOfferingReq.FormContent.Image.SourceFile);
        dest.FormContent.Image.Main.Size = new PublishedSize();
        dest.FormContent.Image.Main.Size.Width = updateOfferingReq.FormContent.Image.Main.Crop.Width;
        dest.FormContent.Image.Main.Size.Height = updateOfferingReq.FormContent.Image.Main.Crop.Height;
        
        dest.FormContent.Icon = new PublishedSvgContent();
        dest.FormContent.Icon.Url = new Uri(updateOfferingReq.FormContent.Icon.SourceFile);
        dest.FormContent.Icon.Format = PropertyFormat.Svg;
        
        dest.FormContent.Description = new PublishedHtmlContent();
        dest.FormContent.Description.Markup = _markupEngine.RenderHtml(updateOfferingReq.FormContent.Description.Html).IfNotNull(x => new HtmlEncodedString(x.ToString())).ToHtmlString();
        dest.FormContent.Description.Format = PropertyFormat.Html;
        
        dest.FormContent.Summary = updateOfferingReq.FormContent.Summary;
        
        dest.FormState = new PublishedDonationFormState();
        dest.FormState.CartItem = new PublishedCartItem();
        dest.FormState.CartItem.Type = CartItemType.NewDonation;
        dest.FormState.CartItem.Currency = currency;
        dest.FormState.CartItem.Value = new MoneyRes();
        dest.FormState.CartItem.Value.Currency = currency;
        dest.FormState.CartItem.Value.Amount = 0d;

        var allocation = updateOfferingReq.FormState.CartItem.Type == CartItemType.NewDonation
                             ? updateOfferingReq.FormState.CartItem.NewDonation?.Allocation
                             : updateOfferingReq.FormState.CartItem.NewGiving?.Allocation;
        
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
            dest.FormState.CartItem.NewDonation.Allocation.Feedback.New = new PublishedNewFeedbackIntent();
            dest.FormState.CartItem.NewDonation.Allocation.Feedback.New.Scheme = allocation.Feedback.New.Scheme;
        } else if (allocation.Type == AllocationType.Sponsorship) {
            dest.FormState.CartItem.NewDonation.Allocation.Sponsorship = new PublishedSponsorshipIntent();
            dest.FormState.CartItem.NewDonation.Allocation.Sponsorship.New = new PublishedNewSponsorshipIntent();
            dest.FormState.CartItem.NewDonation.Allocation.Sponsorship.New.Scheme = allocation.Sponsorship.New.Scheme;
        }

        if (allocation.Type == AllocationType.Fund) {
            dest.FormState.Options = new PublishedDonationFormOptions();
            dest.FormState.Options.SuggestedAmounts = GetPublishedDonationFormSuggestedAmounts(updateOfferingReq.FormState.Options.SuggestedAmounts.ToList());
        }

        dest.Options = new PublishedOfferingOptions();
        dest.Options.AllowCrowdfunding = updateOfferingReq.Options.AllowCrowdfunding;
    }
    
    private PublishedFundDimensionValues GetPublishedFundDimensionValues(AllocationIntentReq allocation) {
        var publishedFundDimensionValues = new PublishedFundDimensionValues();
        publishedFundDimensionValues.Dimension1 = allocation.FundDimensions.Dimension1;
        publishedFundDimensionValues.Dimension2 = allocation.FundDimensions.Dimension2;
        publishedFundDimensionValues.Dimension3 = allocation.FundDimensions.Dimension3;
        publishedFundDimensionValues.Dimension4 = allocation.FundDimensions.Dimension4;

        return publishedFundDimensionValues;
    }

    private IDictionary<string, ICollection<PublishedDonationFormSuggestedAmount>> GetPublishedDonationFormSuggestedAmounts(List<DonationFormSuggestedAmountsReq> suggestedAmountsReq) {
        var res = new Dictionary<string, ICollection<PublishedDonationFormSuggestedAmount>>();

        var oneTimeAmountsReq = suggestedAmountsReq.Where(x => x.GiftType == GiftType.OneTime).SelectMany(x => x.Amounts);
        var recurringAmountsReq = suggestedAmountsReq.Where(x => x.GiftType == GiftType.Recurring).SelectMany(x => x.Amounts);

        var oneTimePublishedSuggestedAmounts = oneTimeAmountsReq.Select(x => new PublishedDonationFormSuggestedAmount { Amount = x.Amount, Description = x.Description});
        var recurringPublishedSuggestedAmounts = recurringAmountsReq.Select(x => new PublishedDonationFormSuggestedAmount { Amount = x.Amount, Description = x.Description});

        res.Add(GiftType.OneTime.ToEnumString(), oneTimePublishedSuggestedAmounts.ToList());
        res.Add(GiftType.Recurring.ToEnumString(), recurringPublishedSuggestedAmounts.ToList());

        return res;
    }
}