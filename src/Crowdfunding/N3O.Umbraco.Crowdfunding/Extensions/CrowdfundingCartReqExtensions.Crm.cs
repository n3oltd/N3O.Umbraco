using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.Content;
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
using AllocationType = N3O.Umbraco.Cloud.Engage.Clients.AllocationType;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static partial class CrowdfundingCartReqExtensions {
    public static ConnectBulkAddToCartReq ToConnectBulkAddToCartReq(this CrowdfundingCartReq crowdfundingReq,
                                                                    IContentLocator contentLocator,
                                                                    IJsonProvider jsonProvider) {
        var connectBulkAddToCartReq = new ConnectBulkAddToCartReq();
        connectBulkAddToCartReq.Items = crowdfundingReq.Items
                                                       .Select(x => ToConnectAddToCartReq(contentLocator,
                                                                                          jsonProvider,
                                                                                          crowdfundingReq,
                                                                                          x))
                                                       .ToList();

        return connectBulkAddToCartReq;
    }

    private static ConnectAddToCartReq ToConnectAddToCartReq(IContentLocator contentLocator,
                                                             IJsonProvider jsonProvider,
                                                             CrowdfundingCartReq crowdfundingReq,
                                                             CrowdfundingCartItemReq itemReq) {
        var crowdfunderContent = contentLocator.GetCrowdfunderContent(crowdfundingReq.Crowdfunding.CrowdfunderId.GetValueOrThrow(),
                                                                      crowdfundingReq.Type);
        
        // TODO This should be checked in validator
        var goal = crowdfunderContent.Goals.Single(x => x.Id == itemReq.GoalId);

        var connectAddToCartReq = new ConnectAddToCartReq();
        connectAddToCartReq.Type = TransactionType.Donation;
        connectAddToCartReq.Quantity = 1;
        connectAddToCartReq.Item = new ConnectCartItemReq();
        
        connectAddToCartReq.Item.Type = (AllocationType) Enum.Parse(typeof(AllocationType), goal.Type.Id, true);;
        connectAddToCartReq.Item.Value = new MoneyReq();
        connectAddToCartReq.Item.Value.Amount = (double?) itemReq.Value.Amount;
        connectAddToCartReq.Item.Value.Currency = (Currency) Enum.Parse(typeof(Currency), itemReq.Value.Currency.Id, true);

        connectAddToCartReq.Item.FundDimensions = new FundDimensionValuesReq();
        connectAddToCartReq.Item.FundDimensions.Dimension1 = goal.FundDimensions.Dimension1?.Name;
        connectAddToCartReq.Item.FundDimensions.Dimension2 = goal.FundDimensions.Dimension2?.Name;
        connectAddToCartReq.Item.FundDimensions.Dimension3 = goal.FundDimensions.Dimension3?.Name;
        connectAddToCartReq.Item.FundDimensions.Dimension4 = goal.FundDimensions.Dimension4?.Name;
        
        if (goal.Type == AllocationTypes.Fund) {
            connectAddToCartReq.Item.Fund = new ConnectFundCartItemReq();
            connectAddToCartReq.Item.Fund.DonationItem = goal.Fund.DonationItem.Name;
        } else if (goal.Type == AllocationTypes.Feedback) {
            connectAddToCartReq.Item.Feedback = new ConnectFeedbackCartItemReq();
            connectAddToCartReq.Item.Feedback.Scheme = goal.Feedback.Scheme.Name;
            
            if (itemReq.HasValue(x => x.Feedback?.CustomFields)) {
                connectAddToCartReq.Item.Feedback.CustomFields = new NewCustomFieldsReq();
                connectAddToCartReq.Item.Feedback.CustomFields.Entries = itemReq.Feedback
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
        
        connectAddToCartReq.Item.ExtensionsData = extensionsData;

        return connectAddToCartReq;
    }
}