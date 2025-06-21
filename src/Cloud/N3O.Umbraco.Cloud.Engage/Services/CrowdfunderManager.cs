using N3O.Umbraco.Authentication.Auth0.Lookups;
using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Cloud.Engage.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdfunderType = N3O.Umbraco.Cloud.Engage.Lookups.CrowdfunderType;
using EngageAllocationType = N3O.Umbraco.Cloud.Engage.Clients.AllocationType;
using EngageCurrency = N3O.Umbraco.Cloud.Engage.Clients.Currency;
using FundDimensionValuesReq = N3O.Umbraco.Cloud.Engage.Clients.FundDimensionValuesReq;
using MoneyReq = N3O.Umbraco.Cloud.Engage.Clients.MoneyReq;

namespace N3O.Umbraco.Cloud.Engage;

public class CrowdfunderManager : ICrowdfunderManager {
    private readonly ClientFactory<CrowdfundingClient> _clientFactory;
    private readonly IServiceProvider _serviceProvider;
    private readonly Lazy<IAccountIdentityAccessor> _accountIdentityAccessor;
    private ServiceClient<CrowdfundingClient> _client;

    public CrowdfunderManager(ClientFactory<CrowdfundingClient> clientFactory,
                              IServiceProvider serviceProvider,
                              Lazy<IAccountIdentityAccessor> accountIdentityAccessor) {
        _clientFactory = clientFactory;
        _serviceProvider = serviceProvider;
        _accountIdentityAccessor = accountIdentityAccessor;
    }

    public async Task CreateCampaignAsync(ICampaign campaign, IEnumerable<string> webhookUrls) {
        var req = GetCreateCampaignReq(campaign, webhookUrls);

        var client = await GetClientAsync(CrowdfunderTypes.Campaign);

        await client.InvokeAsync(x => x.CreateCampaignAsync(req));
    }

    public async Task CreateFundraiserAsync(IFundraiser fundraiser, IEnumerable<string> webhookUrls) {
        var req = GetCreateFundraiserReq(fundraiser, webhookUrls);

        var client = await GetClientAsync(CrowdfunderTypes.Fundraiser);

        await client.InvokeAsync(x => x.CreateFundraiserAsync(req));
    }

    public async Task UpdateCrowdfunderAsync(string id,
                                             ICrowdfunder crowdfunder,
                                             bool toggleStatus,
                                             IEnumerable<string> webhookUrls) {
        var client = await GetClientAsync(crowdfunder.Type);
        var crowdfunderRes = await client.InvokeAsync(x => x.GetCrowdfunderByIdAsync(id));

        var syncCrowdfunderReq = new SyncCrowdfunderReq();

        syncCrowdfunderReq.Name = new CrowdfunderNameReq();
        syncCrowdfunderReq.Name.Value = crowdfunder.Name;
        
        syncCrowdfunderReq.Url = new CrowdfunderUrlReq();
        syncCrowdfunderReq.Url.Value = crowdfunder.Url(_serviceProvider);

        if (crowdfunder.Status.CanToggle && toggleStatus) {
            if (crowdfunder.Status.ToggleAction == CrowdfunderActivationActions.Activate) {
                syncCrowdfunderReq.Activate = true;
            } else if (crowdfunder.Status.ToggleAction == CrowdfunderActivationActions.Deactivate) {
                syncCrowdfunderReq.Deactivate = true;
            } else {
                throw UnrecognisedValueException.For(crowdfunder.Status.ToggleAction);
            }
        }
        
        syncCrowdfunderReq.Allocations = GetCrowdfunderAllocationsReq(crowdfunder.Goals,
                                                                      crowdfunder.Currency.ToEnum<EngageCurrency>());
        
        var req = new CreateJobReqSyncCrowdfunderReq();
        req.NotificationUrls = webhookUrls.ToList();
        req.Data = syncCrowdfunderReq;

        await client.InvokeAsync(x => x.SyncCrowdfunderAsync(crowdfunderRes.RevisionId, req));
    }
    
    private async Task<ServiceClient<CrowdfundingClient>> GetClientAsync(CrowdfunderType crowdfunderType) {
        ClientType clientType;
        
        if (crowdfunderType == CrowdfunderTypes.Campaign) {
            clientType = ClientTypes.BackOffice;
        } else if (crowdfunderType == CrowdfunderTypes.Fundraiser) {
            clientType = ClientTypes.Members;
        } else {
            throw UnrecognisedValueException.For(crowdfunderType);
        }
        
        if (_client == null) {
            _client = await _clientFactory.CreateAsync(clientType);
        }

        return _client;
    }

    private CreateJobReqCreateCampaignReq GetCreateCampaignReq(ICampaign campaign, IEnumerable<string> webhookUrls) {
        var campaignReq = new CreateCampaignReq();
        campaignReq.Crowdfunder = GetCreateCrowdfunderReq(campaign);

        var req = new CreateJobReqCreateCampaignReq();
        req.NotificationUrls = webhookUrls.ToList();
        req.Data = campaignReq;

        return req;
    }

    private CreateJobReqCreateFundraiserReq GetCreateFundraiserReq(IFundraiser fundraiser, IEnumerable<string> webhookUrls) {
        var fundraiserReq = new CreateFundraiserReq();
        fundraiserReq.Account = _accountIdentityAccessor.Value.GetToken();
        fundraiserReq.CampaignId = fundraiser.CampaignId.ToString();
        fundraiserReq.Crowdfunder = GetCreateCrowdfunderReq(fundraiser);
        
        var req = new CreateJobReqCreateFundraiserReq();
        req.NotificationUrls = webhookUrls.ToList();
        req.Data = fundraiserReq;

        return req;
    }

    private CreateCrowdfunderReq GetCreateCrowdfunderReq(ICrowdfunder crowdfunder) {
        var req = new CreateCrowdfunderReq();
        req.Id = crowdfunder.Id.ToString();
        req.Name = crowdfunder.Name;
        req.Url = crowdfunder.Url(_serviceProvider);
        req.Currency = crowdfunder.Currency.ToEnum<EngageCurrency>();
        req.Allocations = GetCrowdfunderAllocationsReq(crowdfunder.Goals, crowdfunder.Currency.ToEnum<EngageCurrency>());

        return req;
    }

    private CrowdfunderAllocationsReq GetCrowdfunderAllocationsReq(IEnumerable<ICrowdfunderGoal> goals,
                                                                   EngageCurrency? currency) {
        var req = new CrowdfunderAllocationsReq();

        var items = new List<CrowdfunderAllocationReq>();

        foreach (var goal in goals) {
            var item = new CrowdfunderAllocationReq();
            item.Type = goal.Type.ToEnum<EngageAllocationType>();
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

    private MoneyReq GetMoneyReq(decimal amount, EngageCurrency? currency) {
        var req = new MoneyReq();
        req.Amount = (double?) amount;
        req.Currency = currency;

        return req;
    }
}