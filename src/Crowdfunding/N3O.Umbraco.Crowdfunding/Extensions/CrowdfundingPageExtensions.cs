using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Crowdfunding.Lookups;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class CrowdfundingPageExtensions {
    public static void SetContentValues(IContentPublisher contentPublisher,
                                         IContentLocator contentLocator,
                                         CreatePageReq req) {
        var campaign = contentLocator.ById(req.CampaignId.GetValueOrThrow());
        
        contentPublisher.Content.Label(CrowdfundingPage.Properties.PageSlug).Set(req.Slug);
        contentPublisher.Content.Label(CrowdfundingPage.Properties.PageTitle).Set(req.Name);
        contentPublisher.Content.Label(CrowdfundingPage.Properties.PageStatus).Set(CrowdfundingPageStatuses.Pending.Name);
        contentPublisher.Content.Label(CrowdfundingPage.Properties.FundraiserName).Set(req.FundraiserName);
        contentPublisher.Content.ContentPicker(CrowdfundingPage.Properties.Campaign).SetContent(campaign);
        
        PopulateAllocations(contentPublisher, req.PageAllocation);
    }
    
    private static void AddAllocation(IContentBuilder contentBuilder, PageAllocationReq allocation) {
        contentBuilder.TextBox(CrowdfundingPageAllocation.Properties.Title).Set(allocation.Title);
        contentBuilder.DataList(CrowdfundingPageAllocation.Properties.Type).SetLookups(allocation.Type);
        contentBuilder.Numeric(CrowdfundingPageAllocation.Properties.Amount).SetDecimal(allocation.Value.Amount);
        contentBuilder.ContentPicker(CrowdfundingPageAllocation.Properties.FundDimension1).SetContent(allocation.FundDimensions.Dimension1);
        contentBuilder.ContentPicker(CrowdfundingPageAllocation.Properties.FundDimension2).SetContent(allocation.FundDimensions.Dimension2);
        contentBuilder.ContentPicker(CrowdfundingPageAllocation.Properties.FundDimension3).SetContent(allocation.FundDimensions.Dimension3);
        contentBuilder.ContentPicker(CrowdfundingPageAllocation.Properties.FundDimension4).SetContent(allocation.FundDimensions.Dimension4);
        
        var priceHandles = contentBuilder.Nested(CrowdfundingPageAllocation.Properties.PriceHandles);

        foreach (var priceHandle in allocation.PriceHandles.OrEmpty()) {
            PopulatePriceHandles(priceHandles.Add(CrowdfundingPageAllocation.Properties.PriceHandle.Alias), priceHandle);
        }
    }
    
    private static void AddFeedbackAllocation(IContentBuilder contentBuilder, PageAllocationReq allocation) {
        contentBuilder.ContentPicker(CrowdfundingFeedbackPageAllocation.Properties.Scheme)
                      .SetContent(allocation.Feedback.Scheme);
        
        AddAllocation(contentBuilder, allocation);
    }

    private static void AddFundAllocation(IContentBuilder contentBuilder, PageAllocationReq allocation) {
        contentBuilder.ContentPicker(CrowdfundingFundPageAllocation.Properties.DonationItem)
                      .SetContent(allocation.Fund.DonationItem);
        
        AddAllocation(contentBuilder, allocation);
    }
    
    private static void AddSponsorshipAllocation(IContentBuilder contentBuilder, PageAllocationReq allocation) {
        contentBuilder.ContentPicker(CrowdfundingSponsorshipPageAllocation.Properties.Scheme)
                      .SetContent(allocation.Sponsorship.Scheme);
        
        AddAllocation(contentBuilder, allocation);
    }
    
    private static void PopulateAllocations(IContentPublisher contentPublisher, IEnumerable<PageAllocationReq> pageAllocations) {
        var nestedContent = contentPublisher.Content.Nested(CrowdfundingPage.Properties.Allocations);
        
        foreach (var allocation in pageAllocations) {
            if (allocation.Type == AllocationTypes.Fund) {
                AddFundAllocation(nestedContent.Add(CrowdfundingFundPageAllocation.Alias), allocation);
            } else if (allocation.Type == AllocationTypes.Sponsorship) {
                AddSponsorshipAllocation(nestedContent.Add(CrowdfundingSponsorshipPageAllocation.Alias), allocation);
            } else if (allocation.Type == AllocationTypes.Feedback) {
                AddFeedbackAllocation(nestedContent.Add(CrowdfundingFeedbackPageAllocation.Alias), allocation);
            } else {
                throw UnrecognisedValueException.For(allocation.Type);
            }
        }
    }
    
    private static void PopulatePriceHandles(IContentBuilder contentBuilder, PriceHandleReq priceHandleElement) {
        contentBuilder.Numeric(CrowdfundingPageAllocation.Properties.PriceHandle.Properties.Amount).SetDecimal(priceHandleElement.Amount);
        contentBuilder.TextBox(CrowdfundingPageAllocation.Properties.PriceHandle.Properties.Description).Set(priceHandleElement.Description);
    }
}