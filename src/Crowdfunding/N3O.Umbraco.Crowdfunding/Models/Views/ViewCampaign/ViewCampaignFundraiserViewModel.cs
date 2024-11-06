using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Models;

public class ViewCampaignFundraiserViewModel {
    public string Name { get; private set; }
    public string Subtitle { get; private set; }
    public string OwnerAvatarLink { get; private set; }
    public Money GoalsTotal { get; private set; }
    public Money ContributionsTotal { get; private set; }

    public static ViewCampaignFundraiserViewModel For(FundraiserContent fundraiser,
                                                      IEnumerable<Contribution> contributions) {
        var viewModel = new ViewCampaignFundraiserViewModel();
            
        viewModel.Name = fundraiser.Name;
        viewModel.Subtitle = null;
        viewModel.OwnerAvatarLink = fundraiser.Owner.AvatarLink;
        viewModel.GoalsTotal = new Money(fundraiser.Goals.Sum(x => x.Amount), fundraiser.Currency);
        viewModel.ContributionsTotal = contributions.HasAny()
                                           ? new Money(contributions.Sum(x => x.CrowdfunderAmount), fundraiser.Currency)
                                           : fundraiser.Currency.Zero();

        return viewModel;
    }
}