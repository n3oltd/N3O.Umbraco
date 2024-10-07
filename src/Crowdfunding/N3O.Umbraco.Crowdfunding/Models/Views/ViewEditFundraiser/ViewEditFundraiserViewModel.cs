using N3O.Umbraco.Constants;
using N3O.Umbraco.Context;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Crowdfunding.Models;

public class ViewEditFundraiserViewModel : CrowdfunderViewModel<FundraiserContent> {
    private FundraiserAccessControl _fundraiserAccessControl;
    
    public override bool EditMode() {
        return _fundraiserAccessControl.CanEditAsync(Content.Content()).GetAwaiter().GetResult();
    }

    public static async Task<ViewEditFundraiserViewModel> ForAsync(ICrowdfundingViewModelFactory viewModelFactory,
                                                                   ICurrencyAccessor currencyAccessor,
                                                                   IForexConverter forexConverter,
                                                                   ILookups lookups,
                                                                   ITextFormatter textFormatter,
                                                                   FundraiserAccessControl fundraiserAccessControl,
                                                                   ViewEditFundraiserPage page,
                                                                   IReadOnlyDictionary<string, string> query,
                                                                   FundraiserContent fundraiser,
                                                                   IEnumerable<Contribution> contributions) {
        var viewModel = await ForAsync<ViewEditFundraiserViewModel>(viewModelFactory,
                                                                    currencyAccessor,
                                                                    forexConverter,
                                                                    lookups,                                                                    
                                                                    page,
                                                                    query,
                                                                    fundraiser,
                                                                    CrowdfunderTypes.Fundraiser,
                                                                    contributions,
                                                                    () => GetOwnerInfo(textFormatter, fundraiser));

        viewModel._fundraiserAccessControl = fundraiserAccessControl;
        
        return viewModel;
    }
    
    private static CrowdfunderOwnerViewModel GetOwnerInfo(ITextFormatter textFormatter, FundraiserContent fundraiser) {
        var name = fundraiser.Owner?.Name;
        var profileImage = fundraiser.Owner?.Value<string>(MemberConstants.Member.Properties.AvatarLink);
        var strapline = textFormatter.Format<Strings>(s => s.Strapline_1, fundraiser.Campaign.Name);

        return CrowdfunderOwnerViewModel.For(name, profileImage, strapline);
    }

    public class Strings : CodeStrings {
        public string Strapline_1 => "Fundraising for {0}";
    }
}