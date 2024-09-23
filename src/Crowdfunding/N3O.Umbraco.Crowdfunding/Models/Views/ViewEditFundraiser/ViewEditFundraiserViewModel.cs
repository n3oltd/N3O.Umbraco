using N3O.Umbraco.Constants;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Localization;
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
                                                                   ITextFormatter textFormatter,
                                                                   FundraiserAccessControl fundraiserAccessControl,
                                                                   ViewEditFundraiserPage page,
                                                                   FundraiserContent fundraiser,
                                                                   IEnumerable<Contribution> contributions) {
        var viewModel = await ForAsync<ViewEditFundraiserViewModel>(viewModelFactory,
                                                                    page,
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