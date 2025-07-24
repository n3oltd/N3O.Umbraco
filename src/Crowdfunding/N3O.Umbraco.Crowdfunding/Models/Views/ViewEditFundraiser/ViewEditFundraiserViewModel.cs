using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Context;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public class ViewEditFundraiserViewModel : CrowdfunderViewModel<FundraiserContent> {
    private FundraiserAccessControl _fundraiserAccessControl;
    private bool _preview;
    
    public override bool EditMode() {
        return _fundraiserAccessControl.CanEditAsync(Content.Content()).GetAwaiter().GetResult() && !_preview;
    }
    
    public bool HasUnallocatedFunds { get; private set; }
    public bool HasPendingGoalsWithPricing { get; private set; }

    public static async Task<ViewEditFundraiserViewModel> ForAsync(ICrowdfundingViewModelFactory viewModelFactory,
                                                                   ICurrencyAccessor currencyAccessor,
                                                                   IForexConverter forexConverter,
                                                                   ICrowdfundingUrlBuilder urlBuilder,
                                                                   ILookups lookups,
                                                                   ITextFormatter textFormatter,
                                                                   FundraiserAccessControl fundraiserAccessControl,
                                                                   ViewEditFundraiserPage page,
                                                                   IReadOnlyDictionary<string, string> query,
                                                                   FundraiserContent fundraiser,
                                                                   IEnumerable<Contribution> contributions,
                                                                   bool preview) {
        var viewModel = await ForAsync<ViewEditFundraiserViewModel>(viewModelFactory,
                                                                    currencyAccessor,
                                                                    forexConverter,
                                                                    urlBuilder,
                                                                    lookups,
                                                                    page,
                                                                    query,
                                                                    fundraiser,
                                                                    CrowdfunderTypes.Fundraiser,
                                                                    contributions,
                                                                    () => GetOwnerInfo(textFormatter, fundraiser));
        
        var contributionsTotal = contributions.OrEmpty().Sum(x => x.CrowdfunderAmount);
        var goalsWithPricing = fundraiser.Goals.Where(x => x.HasPricing).Sum(x => x.Amount);
        
        viewModel.HasUnallocatedFunds = fundraiser.Goals.Sum(x => x.Amount) < contributions.Sum(x => x.CrowdfunderAmount);
        viewModel.HasPendingGoalsWithPricing = goalsWithPricing > 0 && contributionsTotal < goalsWithPricing;

        viewModel._fundraiserAccessControl = fundraiserAccessControl;
        viewModel._preview = preview;
        
        return viewModel;
    }
    
    private static CrowdfunderOwnerViewModel GetOwnerInfo(ITextFormatter textFormatter, FundraiserContent fundraiser) {
        var name = fundraiser.Owner?.Name;
        var profileImage = fundraiser.Owner?.AvatarLink;
        var strapline = textFormatter.Format<Strings>(s => s.Strapline_1, fundraiser.Campaign.Name);

        return CrowdfunderOwnerViewModel.For(name, profileImage, strapline);
    }

    public class Strings : CodeStrings {
        public string Strapline_1 => "Fundraising for {0}";
    }
}