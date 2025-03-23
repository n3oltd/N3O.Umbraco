using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using AllocationType = N3O.Umbraco.Crm.Engage.Clients.AllocationType;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static partial class CrowdfundingCartReqExtensions {
    public static BulkAddToCartReq ToBulkAddToCrmCartReq(this CrowdfundingCartReq crowdfundingReq,
                                                         IContentLocator contentLocator,
                                                         IJsonProvider jsonProvider) {
        var bulkAddToCartReq = new BulkAddToCartReq();
        bulkAddToCartReq.Items = crowdfundingReq.Items
                                                .Select(x => ToAddToCrmCartReq(contentLocator,
                                                                               jsonProvider,
                                                                               crowdfundingReq,
                                                                               x))
                                                .ToList();

        return bulkAddToCartReq;
    }

    private static AddToCartReq ToAddToCrmCartReq(IContentLocator contentLocator,
                                                  IJsonProvider jsonProvider,
                                                  CrowdfundingCartReq crowdfundingReq,
                                                  CrowdfundingCartItemReq itemReq) {
        var crowdfunderContent = contentLocator.GetCrowdfunderContent(crowdfundingReq.Crowdfunding.CrowdfunderId.GetValueOrThrow(),
                                                                      crowdfundingReq.Type);
        
        // TODO This should be checked in validator
        var goal = crowdfunderContent.Goals.Single(x => x.Id == itemReq.GoalId);

        var addToCartReq = new AddToCartReq();
        addToCartReq.Type = TransactionType.Donation;
        addToCartReq.Quantity = 1;
        addToCartReq.Item = new CartItemReq();
        
        addToCartReq.Item.Type = (AllocationType) Enum.Parse(typeof(AllocationType), goal.Type.Id, true);;
        addToCartReq.Item.Value = new MoneyReq();
        addToCartReq.Item.Value.Amount = (double?) itemReq.Value.Amount;
        addToCartReq.Item.Value.Currency = (Currency) Enum.Parse(typeof(Currency), itemReq.Value.Currency.Id, true);

        addToCartReq.Item.FundDimensions = new FundDimensionValuesReq();
        addToCartReq.Item.FundDimensions.Dimension1 = goal.FundDimensions.Dimension1?.Name;
        addToCartReq.Item.FundDimensions.Dimension2 = goal.FundDimensions.Dimension2?.Name;
        addToCartReq.Item.FundDimensions.Dimension3 = goal.FundDimensions.Dimension3?.Name;
        addToCartReq.Item.FundDimensions.Dimension4 = goal.FundDimensions.Dimension4?.Name;
        
        if (goal.Type == AllocationTypes.Fund) {
            addToCartReq.Item.Fund = new FundCartItemReq();
            addToCartReq.Item.Fund.DonationItem = goal.Fund.DonationItem.Name;
        } else if (goal.Type == AllocationTypes.Feedback) {
            addToCartReq.Item.Feedback = new FeedbackCartItemReq();
            addToCartReq.Item.Feedback.Scheme = goal.Feedback.Scheme.Name;
            
            if (itemReq.HasValue(x => x.Feedback?.CustomFields)) {
                addToCartReq.Item.Feedback.CustomFields = new NewCustomFieldsReq();
                addToCartReq.Item.Feedback.CustomFields.Entries = itemReq.Feedback
                                                                         .CustomFields
                                                                         .Entries
                                                                         .Select(x => x.ToFeedbackCustomField(goal.Feedback.Scheme))
                                                                         .Select(x => new NewCustomFieldReq { Alias = x.Alias, Bool = x.Bool, Date = x.Date?.ToYearMonthDayString(), Text = x.Text})
                                                                         .ToList();
            }
        } else {
            throw UnrecognisedValueException.For(goal.Type);
        }

        var extensionsData = new Dictionary<string, JToken>();
        extensionsData.Set(jsonProvider, CrowdfundingConstants.Allocations.Extensions.Key, crowdfundingReq.Crowdfunding);
        
        addToCartReq.Item.ExtensionsData = extensionsData;

        return addToCartReq;
    }
}