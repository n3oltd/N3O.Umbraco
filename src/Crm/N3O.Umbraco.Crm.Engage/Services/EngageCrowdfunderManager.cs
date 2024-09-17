using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Crm.Engage.Extensions;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EngageCurrency = N3O.Umbraco.Crm.Engage.Clients.Currency;
using FundDimensionValuesReq = N3O.Umbraco.Crm.Engage.Clients.FundDimensionValuesReq;
using MoneyReq = N3O.Umbraco.Crm.Engage.Clients.MoneyReq;

namespace N3O.Umbraco.Crm.Engage;

public class EngageCrowdfunderManager : ICrowdfunderManager {
    private readonly ClientFactory<CrowdfundingClient> _clientFactory;
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private readonly IContentLocator _contentLocator;
    private readonly Lazy<IAccountIdentityAccessor> _accountIdentityAccessor;

    public EngageCrowdfunderManager(ClientFactory<CrowdfundingClient> clientFactory,
                                    ISubscriptionAccessor subscriptionAccessor,
                                    IContentLocator contentLocator,
                                    Lazy<IAccountIdentityAccessor> accountIdentityAccessor) {
        _clientFactory = clientFactory;
        _subscriptionAccessor = subscriptionAccessor;
        _contentLocator = contentLocator;
        _accountIdentityAccessor = accountIdentityAccessor;
    }

    public async Task CreateCampaignAsync(ICampaign campaign) {
        var req = GetCreateCampaignReq(campaign);

        var client = await _clientFactory.CreateAsync(_subscriptionAccessor.GetSubscription());

        var res = await client.InvokeAsync<CreateCampaignReq, CampaignRes>(x => x.CreateCampaignAsync, req);
    }

    public async Task CreateFundraiserAsync(IFundraiser fundraiser) {
        var req = GetCreateFundraiserReq(fundraiser);

        var client = await _clientFactory.CreateAsync(_subscriptionAccessor.GetSubscription());

        var res = await client.InvokeAsync<CreateFundraiserReq, FundraiserRes>(x => x.CreateFundraiserAsync, req);
    }

    public async Task UpdateCrowdfunderAsync(string id, ICrowdfunder crowdfunder) {
        var client = await _clientFactory.CreateAsync(_subscriptionAccessor.GetSubscription());
        var crowdfunderRes = await client.InvokeAsync<CrowdfunderRes>(x => x.GetCrowdfunderByIdAsync, id);

        var syncCrowdfunderReq = new SyncCrowdfunderReq();

        syncCrowdfunderReq.Name = new CrowdfunderNameReq();
        syncCrowdfunderReq.Name.Value = crowdfunder.Name;
        
        syncCrowdfunderReq.Url = new CrowdfunderUrlReq();
        syncCrowdfunderReq.Url.Value = crowdfunder.Url(_contentLocator);
        
        syncCrowdfunderReq.Allocations = GetCrowdfunderAllocationsReq(crowdfunder.Goals,
                                                                      crowdfunder.Currency.ToEngageCurrency());

        await client.InvokeAsync(x => x.SyncCrowdfunderAsync, crowdfunderRes.RevisionId, syncCrowdfunderReq);
    }

    private CreateCampaignReq GetCreateCampaignReq(ICampaign campaign) {
        var req = new CreateCampaignReq();
        req.Crowdfunder = GetCreateCrowdfunderReq(campaign);

        return req;
    }

    private CreateFundraiserReq GetCreateFundraiserReq(IFundraiser fundraiser) {
        var req = new CreateFundraiserReq();
        req.Account = _accountIdentityAccessor.Value.GetToken();
        req.CampaignId = fundraiser.CampaignId.ToString();
        req.Crowdfunder = GetCreateCrowdfunderReq(fundraiser);

        return req;
    }

    private CreateCrowdfunderReq GetCreateCrowdfunderReq(ICrowdfunder crowdfunder) {
        var req = new CreateCrowdfunderReq();
        req.Id = crowdfunder.Id;
        req.Name = crowdfunder.Name;
        req.Url = crowdfunder.Url(_contentLocator);
        req.Currency = crowdfunder.Currency.ToEngageCurrency();
        req.Allocations = GetCrowdfunderAllocationsReq(crowdfunder.Goals, crowdfunder.Currency.ToEngageCurrency());

        return req;
    }

    private CrowdfunderAllocationsReq GetCrowdfunderAllocationsReq(IEnumerable<ICrowdfunderGoal> goals,
                                                                   EngageCurrency currency) {
        var req = new CrowdfunderAllocationsReq();

        var items = new List<CrowdfunderAllocationReq>();

        foreach (var goal in goals) {
            var item = new CrowdfunderAllocationReq();
            item.Type = goal.Type.ToEngageAllocationType();
            item.FundDimensions = GetFundDimensionValuesReq(goal.FundDimensions);
            item.Value = GetMoneyReq(goal.Amount, currency);

            if (goal.Type == AllocationTypes.Fund) {
                item.Fund = GetNewFundAllocationReq(goal.Fund);
            } else if (goal.Type == AllocationTypes.Feedback) {
                item.Feedback = GetCrowdfunderFeedbackReq(goal.Feedback);
            } else {
                throw UnrecognisedValueException.For(goal.Type);
            }

            items.Add(item);
        }

        req.Items = items;
        return req;
    }

    private NewFundAllocationReq GetNewFundAllocationReq(IFundCrowdfunderGoal fundGoal) {
        var req = new NewFundAllocationReq();
        req.DonationItem = fundGoal.DonationItem.Name;

        return req;
    }

    private CrowdfunderFeedbackReq GetCrowdfunderFeedbackReq(IFeedbackCrowdfunderGoal feedbackGoal) {
        var req = new CrowdfunderFeedbackReq();
        req.Scheme = feedbackGoal.Scheme.Name;
        req.CustomFields = GetNewCustomFieldsReq(feedbackGoal);

        return req;
    }

    private List<NewCustomFieldReq> GetNewCustomFieldsReq(IFeedbackCrowdfunderGoal feedbackGoal) {
        var fields = new List<NewCustomFieldReq>();

        foreach (var customField in feedbackGoal.CustomFields) {
            var customFieldReq = new NewCustomFieldReq();
            customFieldReq.Alias = customField.Alias;

            if (customField.Type == FeedbackCustomFieldTypes.Text) {
                customFieldReq.Text = customField.Text;
            } else if (customField.Type == FeedbackCustomFieldTypes.Bool) {
                customFieldReq.Bool = customField.Bool;
            } else if (customField.Type == FeedbackCustomFieldTypes.Date) {
                customFieldReq.Date = customField.Date?.ToYearMonthDayString();
            } else {
                throw UnrecognisedValueException.For(customField.Type);
            }

            fields.Add(customFieldReq);
        }

        return fields;
    }

    private FundDimensionValuesReq GetFundDimensionValuesReq(IFundDimensionValues fundDimensions) {
        var req = new FundDimensionValuesReq();
        req.Dimension1 = fundDimensions.Dimension1?.Name;
        req.Dimension2 = fundDimensions.Dimension2?.Name;
        req.Dimension3 = fundDimensions.Dimension3?.Name;
        req.Dimension4 = fundDimensions.Dimension4?.Name;

        return req;
    }

    private MoneyReq GetMoneyReq(decimal amount, EngageCurrency currency) {
        var req = new MoneyReq();
        req.Amount = (double?) amount;
        req.Currency = currency;

        return req;
    }
}