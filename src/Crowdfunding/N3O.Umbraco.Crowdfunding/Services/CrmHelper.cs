using N3O.Umbraco.Crm.Engage;
using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Subscription;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AllocationType = N3O.Umbraco.Crm.Engage.Clients.AllocationType;
using Currency = N3O.Umbraco.Financial.Currency;
using EngageCurrency = N3O.Umbraco.Crm.Engage.Clients.Currency;

namespace N3O.Umbraco.CrowdFunding;

public class CrmHelper : ICrmHelper {
    private readonly ClientFactory<CrowdfundingClient> _client;
    private readonly ISubscriptionAccessor _subscriptionAccessor;

    public CrmHelper(ClientFactory<CrowdfundingClient> client, ISubscriptionAccessor subscriptionAccessor) {
        _client = client;
        _subscriptionAccessor = subscriptionAccessor;
    }

    public async Task<CampaignRes> CreatePledgeForCampaignAsync(CampaignContent campaignContent) {
        var req = GetCreateCampaignReq(campaignContent);
        
        var client = await _client.CreateAsync(_subscriptionAccessor.GetSubscription());
        
        var res = await client.InvokeAsync<CreateCampaignReq, CampaignRes>(x => x.CreateCampaignAsync, req);

        return res;
    }
    
    public async Task<FundraiserRes> CreatePledgeForFundraiserAsync(FundraiserContent fundraiserContent) {
        var req = GetCreateFundraiserReq(fundraiserContent);
        
        var client = await _client.CreateAsync(_subscriptionAccessor.GetSubscription());
        
        var res = await client.InvokeAsync<CreateFundraiserReq, FundraiserRes>(x => x.CreateFundraiserAsync, req);

        return res;
    }

    public async Task UpdateAllocationsAsync(Guid id,
                                             Currency currency,
                                             IEnumerable<GoalElement> goals) {
        var engageCurrency = (EngageCurrency) Enum.Parse(typeof(EngageCurrency), currency.Id, true);

        var req = GetCrowdfunderAllocationsReq(goals, engageCurrency);
        
        var client = await _client.CreateAsync(_subscriptionAccessor.GetSubscription());

        var crowdfunder = await client.InvokeAsync<CrowdfunderRes>(x => x.GetCrowdfunderByIdAsync, id.ToString());

        await client.InvokeAsync(x => x.SyncCrowdfunderAllocationsAsync, crowdfunder.RevisionId, req);
    }
    
    public async Task UpdateNameAsync(Guid id, string name) {
        var req = new CrowdfunderNameReq();
        req.Name = name;
        
        var client = await _client.CreateAsync(_subscriptionAccessor.GetSubscription());

        var crowdfunder = await client.InvokeAsync<CrowdfunderRes>(x => x.GetCrowdfunderByIdAsync, id.ToString());
        
        await client.InvokeAsync(x => x.UpdateCrowdfunderNameAsync, crowdfunder.RevisionId, req);
    }
    
    public async Task UpdateSlugAsync(Guid id, string url) {
        var req = new CrowdfunderUrlReq();
        req.Url = url;
        
        var client = await _client.CreateAsync(_subscriptionAccessor.GetSubscription());

        var crowdfunder = await client.InvokeAsync<CrowdfunderRes>(x => x.GetCrowdfunderByIdAsync, id.ToString());
        
        await client.InvokeAsync(x => x.UpdateCrowdfunderUrlAsync, crowdfunder.RevisionId, req);
    }
    
    private CreateCampaignReq GetCreateCampaignReq(CampaignContent campaignContent) {
        var req = new CreateCampaignReq();
        req.Crowdfunder = GetCreateCrowdfunderReq(campaignContent.Content().Key,
                                                  campaignContent.Title,
                                                  campaignContent.Content().AbsoluteUrl(),
                                                  campaignContent.Currency,
                                                  campaignContent.Goals);
        
        return req;
    }
    
    private CreateFundraiserReq GetCreateFundraiserReq(FundraiserContent fundraiserContent) {
        var req = new CreateFundraiserReq();
        req.Account = GetAccountInfo();
        req.CampaignId = fundraiserContent.Campaign.Content().Key.ToString();
        
        req.Crowdfunder = GetCreateCrowdfunderReq(fundraiserContent.Content().Key,
                                                  fundraiserContent.Title,
                                                  fundraiserContent.Content().AbsoluteUrl(),
                                                  fundraiserContent.Currency,
                                                  fundraiserContent.Goals);

        return req;
    }

    private CreateCrowdfunderReq GetCreateCrowdfunderReq( Guid id,
                                                          string title,
                                                          string url,
                                                          Currency currency,
                                                          IEnumerable<GoalElement> goals) {
        var engageCurrency = (EngageCurrency) Enum.Parse(typeof(EngageCurrency), currency.Id, true);
        
        var req = new CreateCrowdfunderReq();
        req.Id = id;
        req.Name = title;
        req.Url = url;
        req.Currency = engageCurrency;
        req.Allocations = GetCrowdfunderAllocationsReq(goals, engageCurrency);
        
        return req;
    }

    private CrowdfunderAllocationsReq GetCrowdfunderAllocationsReq(IEnumerable<GoalElement> allocations,
                                                                   EngageCurrency engageCurrency) {
        var req = new CrowdfunderAllocationsReq();

        var items = new List<CrowdfunderAllocationReq>();

        foreach (var allocation in allocations) {
            var item = new CrowdfunderAllocationReq();
            item.Type = (AllocationType) Enum.Parse(typeof(AllocationType), allocation.Type.ToString(), true);
            item.FundDimensions = GetFundDimensionValuesReq(allocation);
            item.Value = GetMoneyReq(allocation.Amount, engageCurrency);

            if (allocation.Type == AllocationTypes.Fund) {
                item.Fund = GetNewFundAllocationReq(allocation);
            } else if (allocation.Type == AllocationTypes.Feedback) {
                item.Feedback = GetCrowdfunderFeedbackReq(allocation);
            } else {
                throw UnrecognisedValueException.For(allocation.Type);
            }
            
            items.Add(item);
        }
        
        req.Items = items;
        return req;
    }

    private NewFundAllocationReq GetNewFundAllocationReq(GoalElement goal) {
        var req = new NewFundAllocationReq();
        req.DonationItem = goal.Fund.DonationItem.Name;
        
        return req;
    }

    private CrowdfunderFeedbackReq GetCrowdfunderFeedbackReq(GoalElement goal) {
        var req = new CrowdfunderFeedbackReq();
        req.Scheme = goal.Feedback.Scheme.Name;
        req.CustomFields = GetNewCustomFieldsReq(goal);
        
        return req;
    }

    private List<NewCustomFieldReq> GetNewCustomFieldsReq(GoalElement goal) {
        var fields = new List<NewCustomFieldReq>();

        foreach (var customField in goal.Feedback.CustomFields) {
            var customFieldReq = new NewCustomFieldReq();
            customFieldReq.Alias = customField.Alias;

            if (customField.Type == FeedbackCustomFieldTypes.Text) {
                customFieldReq.Text = customField.Text;
            } else if (customField.Type == FeedbackCustomFieldTypes.Bool) {
                customFieldReq.Bool = customField.Bool;
            } else if (customField.Type == FeedbackCustomFieldTypes.Date) {
                customFieldReq.Date = customField.Date?.ToLocalDate().ToYearMonthDayString();
            } else {
                throw UnrecognisedValueException.For(customField.Type);
            }
                    
            fields.Add(customFieldReq);
        }

        return fields;
    }
    
    private FundDimensionValuesReq GetFundDimensionValuesReq(GoalElement goal) {
        var req = new FundDimensionValuesReq();
        req.Dimension1 = goal.FundDimension1?.Name;
        req.Dimension2 = goal.FundDimension2?.Name;
        req.Dimension3 = goal.FundDimension3?.Name;
        req.Dimension4 = goal.FundDimension4?.Name;

        return req;
    }

    private MoneyReq GetMoneyReq(decimal amount, EngageCurrency engageCurrency) {
        var req = new MoneyReq();
        req.Amount = (double?) amount;
        req.Currency = engageCurrency;

        return req;
    }

    //TODO Waiting on Shagufta to get account by id or reference
    private AccountInfoReq GetAccountInfo() {
        var account = new AccountInfoReq();
        account.Id = "a7bc160f-483e-4f7e-97fc-c36995b61f73";
        account.Reference = new ReferenceReq();
        account.Reference.Type = ReferenceType.AC;
        account.Reference.Number = 1000438;
        account.Name = "Mr malik";
        account.Initials = "tm";

        return account;
    }
}