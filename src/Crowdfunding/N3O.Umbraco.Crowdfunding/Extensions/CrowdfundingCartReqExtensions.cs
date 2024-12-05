using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class CrowdfundingCartReqExtensions {
    public static BulkAddToCartReq ToBulkAddToCartReq(this CrowdfundingCartReq crowdfundingReq,
                                                      IContentLocator contentLocator,
                                                      IJsonProvider jsonProvider) {
        var bulkAddToCartReq = new BulkAddToCartReq();
        bulkAddToCartReq.Items = crowdfundingReq.Items
                                                .Select(x => ToAddToCartReq(contentLocator,
                                                                            jsonProvider,
                                                                            crowdfundingReq,
                                                                            x))
                                                .ToList();

        return bulkAddToCartReq;
    }

    private static AddToCartReq ToAddToCartReq(IContentLocator contentLocator,
                                               IJsonProvider jsonProvider,
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

        addToCartReq.Allocation.FundDimensions = new FundDimensionValuesReq();
        addToCartReq.Allocation.FundDimensions.Dimension1 = goal.FundDimensions.Dimension1;
        addToCartReq.Allocation.FundDimensions.Dimension2 = goal.FundDimensions.Dimension2;
        addToCartReq.Allocation.FundDimensions.Dimension3 = goal.FundDimensions.Dimension3;
        addToCartReq.Allocation.FundDimensions.Dimension4 = goal.FundDimensions.Dimension4;
        
        if (goal.Type == AllocationTypes.Fund) {
            addToCartReq.Allocation.Fund = new FundAllocationReq();
            addToCartReq.Allocation.Fund.DonationItem = goal.Fund.DonationItem;
        } else if (goal.Type == AllocationTypes.Feedback) {
            addToCartReq.Allocation.Feedback = new FeedbackAllocationReq();
            addToCartReq.Allocation.Feedback.Scheme = goal.Feedback.Scheme;
            
            if (itemReq.HasValue(x => x.Feedback?.CustomFields)) {
                addToCartReq.Allocation.Feedback.CustomFields = new FeedbackNewCustomFieldsReq();
                addToCartReq.Allocation.Feedback.CustomFields.Entries = itemReq.Feedback
                                                                               .CustomFields
                                                                               .Entries
                                                                               .Select(x => x.ToFeedbackCustomField(goal.Feedback.Scheme))
                                                                               .Select(x => new FeedbackNewCustomFieldReq(x))
                                                                               .ToList();
            }
        } else {
            throw UnrecognisedValueException.For(goal.Type);
        }

        addToCartReq.Allocation.Extensions = new Dictionary<string, JToken>();
        addToCartReq.Allocation.Extensions.Set(jsonProvider,
                                               CrowdfundingConstants.Allocations.Extensions.Key,
                                               crowdfundingReq.Crowdfunding);

        return addToCartReq;
    }
}