using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static partial class CrowdfundingCartReqExtensions {
    public static BulkAddToCartReq ToBulkAddToCartReq(this CrowdfundingCartReq crowdfundingReq,
                                                      IContentLocator contentLocator,
                                                      IJsonProvider jsonProvider,
                                                      ILookups lookups,
                                                      ICrowdfundingUrlBuilder crowdfundingUrlBuilder) {
        var bulkAddToCartReq = new BulkAddToCartReq();
        bulkAddToCartReq.Items = crowdfundingReq.Items
                                                .Select(x => ToAddToCartReq(contentLocator,
                                                                            jsonProvider,
                                                                            lookups,
                                                                            crowdfundingUrlBuilder,
                                                                            crowdfundingReq,
                                                                            x))
                                                .ToList();

        return bulkAddToCartReq;
    }

    private static AddToCartReq ToAddToCartReq(IContentLocator contentLocator,
                                               IJsonProvider jsonProvider,
                                               ILookups lookups,
                                               ICrowdfundingUrlBuilder crowdfundingUrlBuilder,
                                               CrowdfundingCartReq crowdfundingReq,
                                               CrowdfundingCartItemReq itemReq) {
        var crowdfunderContent = contentLocator.GetCrowdfunderContent(crowdfundingReq.Crowdfunding.CrowdfunderId.GetValueOrThrow(),
                                                                      crowdfundingReq.Type);
        
        // TODO This should be checked in validator
        var goal = crowdfunderContent.Goals.Single(x => x.Id == itemReq.GoalId);

        var addToCartReq = new AddToCartReq();
        addToCartReq.GivingType = GivingTypes.Donation;
        addToCartReq.Quantity = 1;
        addToCartReq.Allocation = new AllocationReq();
        
        addToCartReq.Allocation.Type = goal.Type;
        addToCartReq.Allocation.Value = itemReq.Value;

        var fundDimensions = goal.GetFundDimensionValues(lookups);

        addToCartReq.Allocation.FundDimensions = new FundDimensionValuesReq();
        addToCartReq.Allocation.FundDimensions.Dimension1 = fundDimensions.Dimension1;
        addToCartReq.Allocation.FundDimensions.Dimension2 = fundDimensions.Dimension2;
        addToCartReq.Allocation.FundDimensions.Dimension3 = fundDimensions.Dimension3;
        addToCartReq.Allocation.FundDimensions.Dimension4 = fundDimensions.Dimension4;
        
        if (goal.Type == AllocationTypes.Fund) {
            addToCartReq.Allocation.Fund = new FundAllocationReq();
            addToCartReq.Allocation.Fund.DonationItem = goal.Fund.GetDonationItem(lookups);
        } else if (goal.Type == AllocationTypes.Feedback) {
            addToCartReq.Allocation.Feedback = new FeedbackAllocationReq();
            var feedbackScheme = goal.Feedback.GetScheme(lookups);
            
            addToCartReq.Allocation.Feedback.Scheme = feedbackScheme;
            
            if (itemReq.HasValue(x => x.Feedback?.CustomFields)) {
                addToCartReq.Allocation.Feedback.CustomFields = new FeedbackNewCustomFieldsReq();
                addToCartReq.Allocation.Feedback.CustomFields.Entries = itemReq.Feedback
                                                                               .CustomFields
                                                                               .Entries
                                                                               .Select(x => x.ToFeedbackCustomField(feedbackScheme))
                                                                               .Select(x => new FeedbackNewCustomFieldReq(x))
                                                                               .ToList();
            }
        } else {
            throw UnrecognisedValueException.For(goal.Type);
        }

        addToCartReq.Allocation.PledgeUrl = crowdfunderContent.Url(crowdfundingUrlBuilder);

        addToCartReq.Allocation.Extensions = new Dictionary<string, JToken>();
        addToCartReq.Allocation.Extensions.Set(jsonProvider,
                                               CrowdfundingConstants.Allocations.Extensions.Key,
                                               crowdfundingReq.Crowdfunding);

        return addToCartReq;
    }
}