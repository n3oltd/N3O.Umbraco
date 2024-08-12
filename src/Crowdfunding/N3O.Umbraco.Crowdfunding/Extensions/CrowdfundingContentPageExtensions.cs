using N3O.Umbraco.Constants;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Models.FundraisingPage;
using N3O.Umbraco.CrowdFunding.Services;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.CrowdFunding.Extensions;

public static class CrowdfundingContentPageExtensions {
    public static async Task<CrowdfundingContentPageViewModel> GetCrowdfundingContentPageViewModelAsync(this CrowdfundingPageContent content,
                                                                                                      FundraisingPageHelper pageHelper) {
        var pageContributions = await pageHelper.GetContributionsForPageAsync(content.Content().Key);

        var model = new CrowdfundingContentPageViewModel();
        model.Content = content;
        model.Allocations = GetAllocations(pageHelper, content);
        model.CampaignFundraisers = await GetCampaignFundraisersAsync(pageHelper, content);
        model.OwnerInfo = GetOwnerInfo(content);
        model.Progress = GetPageProgress(pageHelper, pageContributions, content);
        model.PageDonations = GetPageDonations(pageHelper, pageContributions);
        

        return model;
    }

    private static IEnumerable<CrowdfundingPageAllocation> GetAllocations(FundraisingPageHelper pageHelper,
                                                                          CrowdfundingPageContent content) {
        var allocations = new List<CrowdfundingPageAllocation>();

        foreach (var allocation in content.Allocations) {
            var newAllocation = new CrowdfundingPageAllocation();
            
            newAllocation.Title = allocation.Title;
            newAllocation.Amount = allocation.Amount;
            newAllocation.FundDimension1Value = allocation.Dimension1;
            newAllocation.FundDimension2Value = allocation.Dimension2;
            newAllocation.FundDimension3Value = allocation.Dimension3;
            newAllocation.FundDimension4Value = allocation.Dimension4;
            newAllocation.DonationItem = allocation.DonationItem;
            newAllocation.SponsorshipScheme = allocation.SponsorshipScheme;
            newAllocation.FeedbackScheme = allocation.FeedbackScheme;
            newAllocation.FeedbackScheme = allocation.FeedbackScheme;
            
            var priceHandles = new List<CrowdfundingPageAllocationPriceHandle>();

            foreach (var priceHandle in allocation.PriceHandles) {
                var newPriceHandle = new CrowdfundingPageAllocationPriceHandle();
                newPriceHandle.Amount = pageHelper.GetQuoteMoney(priceHandle.Amount);
                newPriceHandle.Description = priceHandle.Description;
                
                priceHandles.Add(newPriceHandle);
            }

            newAllocation.PriceHandles = priceHandles;
            
            allocations.Add(newAllocation);
        }

        return allocations;
    }

    private static async Task<IEnumerable<CrowdfundingPageCampaignFundraisers>> GetCampaignFundraisersAsync(FundraisingPageHelper pageHelper,
                                                                                                            CrowdfundingPageContent content) {
        var campaignPages = pageHelper.GetAllPagesForCampaign(content.Campaign.Content().Key);
        var pageIds = campaignPages.OrEmpty().Select(x => x.Content().Key);
        var contributions = await pageHelper.GetContributionsForPagesAsync(pageIds);
        
        var fundraisers = new List<CrowdfundingPageCampaignFundraisers>();
        
        foreach (var campaignPage in campaignPages) {
            var pageContributions = contributions.Where(x => x.PageId == campaignPage.Content().Key);
            
            var fundraiser = new CrowdfundingPageCampaignFundraisers();
            fundraiser.Name = campaignPage.Fundraiser?.Name;
            fundraiser.AvatarLink = campaignPage.Fundraiser?.Value<string>(MemberConstants.Member.Properties.AvatarLink);
            fundraiser.TargetAmount = pageHelper.GetQuoteMoney(campaignPage.Allocations.Sum(x => x.Amount));
            fundraiser.RaisedAmount = pageHelper.GetQuoteMoney(pageContributions.Sum(x => x.QuoteAmount.Normalize()));

            fundraisers.Add(fundraiser);
        }

        return fundraisers;
    }

    private static CrowdfundingPageOwner GetOwnerInfo(CrowdfundingPageContent content) {
        var owner = new CrowdfundingPageOwner();
        owner.Name = content.Fundraiser?.Name;
        owner.AvatarLink = content.Fundraiser?.Value<string>(MemberConstants.Member.Properties.AvatarLink);

        return owner;
    }

    private static IReadOnlyList<FundraisingContentPageDonations> GetPageDonations(FundraisingPageHelper pageHelper,
                                                                                   IReadOnlyList<CrowdfundingContribution> contributions) {
        var donations = new List<FundraisingContentPageDonations>();

        foreach (var contribution in contributions) {
            var donation = new FundraisingContentPageDonations();
            donation.IsAnonymous = contribution.Anonymous;
            donation.Comment = contribution.Comment;
            donation.DonatedAt = contribution.Timestamp;
            donation.Amount = pageHelper.GetQuoteMoney(contribution.QuoteAmount.Normalize());
            donation.AvatarLink = GravatarGeneratorExtensions.GetGravatarUrl(contribution.Anonymous ?
                                                                                 "Anonymous" :
                                                                                 contribution.Name);

            if (!donation.IsAnonymous) {
                donation.Name = contribution.Name;
            }

            donations.Add(donation);
        }

        return donations;
    }

    private static CrowdfundingPageProgress GetPageProgress(FundraisingPageHelper pageHelper,
                                                            IReadOnlyList<CrowdfundingContribution> pageContributions,
                                                            CrowdfundingPageContent content) {
        var progress = new CrowdfundingPageProgress();
        progress.TargetAmount = pageHelper.GetQuoteMoney(content.Allocations.Sum(x => x.Amount));
        progress.RaisedAmount = pageHelper.GetQuoteMoney(pageContributions.Sum(x => x.BaseAmount.Normalize()));
        progress.SupportersCount = pageContributions.Count;
        progress.PercentageCompleted = progress.RaisedAmount.Amount / progress.TargetAmount.Amount * 100;

        return progress;
    }
}