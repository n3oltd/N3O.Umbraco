using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class ContentPublisherExtensions {
    public static void PopulateFundraiser(this IContentPublisher contentPublisher,
                                          IContentLocator contentLocator,
                                          CreateFundraiserReq req,
                                          IPublishedContent member) {
        var campaign = contentLocator.ById(req.CampaignId.GetValueOrThrow());
        
        contentPublisher.Content.Label(Fundraiser.Properties.Slug).Set(req.Slug);
        contentPublisher.Content.Label(Fundraiser.Properties.Title).Set(req.Title);
        contentPublisher.Content.Label(Fundraiser.Properties.Status).Set(FundraiserStatuses.Pending.Name);
        contentPublisher.Content.ContentPicker(Fundraiser.Properties.Owner).SetContent(member);
        contentPublisher.Content.ContentPicker(Fundraiser.Properties.Campaign).SetContent(campaign);
        
        PopulateAllocations(contentPublisher, req.Allocations);
    }
    
    private static void PopulateAllocations(IContentPublisher contentPublisher,
                                            IEnumerable<FundraiserAllocationReq> fundraiserAllocations) {
        var nestedContent = contentPublisher.Content.Nested(Fundraiser.Properties.Allocations);
        
        foreach (var fundraiserAllocation in fundraiserAllocations) {
            if (fundraiserAllocation.Type == AllocationTypes.Fund) {
                AddFundAllocation(nestedContent.Add(FundraiserAllocation.Fund.Alias), fundraiserAllocation);
            } else if (fundraiserAllocation.Type == AllocationTypes.Feedback) {
                AddFeedbackAllocation(nestedContent.Add(FundraiserAllocation.Feedback.Alias), fundraiserAllocation);
            } else if (fundraiserAllocation.Type == AllocationTypes.Sponsorship) {
                AddSponsorshipAllocation(nestedContent.Add(FundraiserAllocation.Sponsorship.Alias), fundraiserAllocation);
            } else {
                throw UnrecognisedValueException.For(fundraiserAllocation.Type);
            }
        }
    }
    
    private static void AddFundAllocation(IContentBuilder contentBuilder, FundraiserAllocationReq allocation) {
        contentBuilder.ContentPicker(FundraiserAllocation.Fund.Properties.DonationItem)
                      .SetContent(allocation.Fund.DonationItem);
        
        AddAllocation(contentBuilder, allocation);
    }
    
    private static void AddFeedbackAllocation(IContentBuilder contentBuilder, FundraiserAllocationReq allocation) {
        contentBuilder.ContentPicker(FundraiserAllocation.Feedback.Properties.Scheme)
                      .SetContent(allocation.Feedback.Scheme);
        
        AddAllocation(contentBuilder, allocation);
    }
    
    private static void AddSponsorshipAllocation(IContentBuilder contentBuilder, FundraiserAllocationReq allocation) {
        contentBuilder.ContentPicker(FundraiserAllocation.Sponsorship.Properties.Scheme)
                      .SetContent(allocation.Sponsorship.Scheme);
        
        AddAllocation(contentBuilder, allocation);
    }
    
    private static void AddAllocation(IContentBuilder contentBuilder, FundraiserAllocationReq allocation) {
        contentBuilder.TextBox(FundraiserAllocation.Properties.Title).Set(allocation.Title);
        contentBuilder.DataList(FundraiserAllocation.Properties.Type).SetLookups(allocation.Type);
        contentBuilder.Numeric(FundraiserAllocation.Properties.Amount).SetDecimal(allocation.Value.Amount);
        contentBuilder.ContentPicker(FundraiserAllocation.Properties.Dimension1).SetContent(allocation.FundDimensions.Dimension1);
        contentBuilder.ContentPicker(FundraiserAllocation.Properties.Dimension2).SetContent(allocation.FundDimensions.Dimension2);
        contentBuilder.ContentPicker(FundraiserAllocation.Properties.Dimension3).SetContent(allocation.FundDimensions.Dimension3);
        contentBuilder.ContentPicker(FundraiserAllocation.Properties.Dimension4).SetContent(allocation.FundDimensions.Dimension4);
        
        var priceHandles = contentBuilder.Nested(FundraiserAllocation.Properties.PriceHandles);

        foreach (var priceHandle in allocation.PriceHandles.OrEmpty()) {
            PopulatePriceHandles(priceHandles.Add(FundraiserAllocation.Properties.PriceHandle.Alias), priceHandle);
        }
    }
    
    private static void PopulatePriceHandles(IContentBuilder contentBuilder, PriceHandleReq priceHandleElement) {
        contentBuilder.Numeric(FundraiserAllocation.Properties.PriceHandle.Properties.Amount).SetDecimal(priceHandleElement.Amount);
        contentBuilder.TextBox(FundraiserAllocation.Properties.PriceHandle.Properties.Description).Set(priceHandleElement.Description);
    }
}