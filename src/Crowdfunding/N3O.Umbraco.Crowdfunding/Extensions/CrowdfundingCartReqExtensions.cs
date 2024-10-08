using J2N.Collections.Generic;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class CrowdfundingCartReqExtensions {
    public static BulkAddToCartReq ToBulkAddToCartReq(this CrowdfundingCartReq crowdfundingReq,
                                                      IContentLocator contentLocator,
                                                      IJsonProvider jsonProvider) {
        var bulkAddToCartReq = new BulkAddToCartReq();
        bulkAddToCartReq.Items = crowdfundingReq.Items
                                                .Select(x => ToAddToCartReq(crowdfundingReq,
                                                                            x,
                                                                            contentLocator,
                                                                            jsonProvider))
                                                .ToList();

        return bulkAddToCartReq;
    }

    private static AddToCartReq ToAddToCartReq(CrowdfundingCartReq crowdfundingReq,
                                               CrowdfundingCartItemReq itemReq,
                                               IContentLocator contentLocator,
                                               IJsonProvider jsonProvider) {
        ICrowdfunderContent crowdfunderContent;
        
        if (crowdfundingReq.Type == CrowdfunderTypes.Campaign) {
            crowdfunderContent = contentLocator.ById<CampaignContent>(crowdfundingReq.Crowdfunding.CrowdfunderId.GetValueOrThrow());
        } else if (crowdfundingReq.Type == CrowdfunderTypes.Fundraiser) {
            crowdfunderContent = contentLocator.ById<FundraiserContent>(crowdfundingReq.Crowdfunding.CrowdfunderId.GetValueOrThrow());
        } else {
            throw UnrecognisedValueException.For(crowdfundingReq.Type);
        } 
        
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
        
        var serializerSettings = jsonProvider.GetSettings();
        var jsonSerializer = JsonSerializer.Create(serializerSettings);
            
        var extensions = new Dictionary<string, JToken>();
        extensions.Add(CrowdfundingConstants.Allocations.Extensions.Key, JToken.FromObject(crowdfundingReq.Crowdfunding, jsonSerializer));
        
        addToCartReq.Allocation.Extensions = extensions;

        return addToCartReq;
    }
}