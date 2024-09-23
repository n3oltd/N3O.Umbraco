using N3O.Umbraco.Constants;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Financial;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Crowdfunding.Models;

public class ViewCampaignFundraisersViewModel {
    public string Name { get; private set; }
    public string Subtitle { get; private set; }
    public string AvatarLink { get; private set; }
    public Money RaisedValue { get; private set; }
    public Money TargetValue { get; private set; }

    public static ViewCampaignFundraisersViewModel For(FundraiserContent fundraiser,
                                                       IEnumerable<Contribution> fundraisersContributions) {
        var viewModel = new ViewCampaignFundraisersViewModel();
            
        viewModel.Name = fundraiser.Name;
        viewModel.AvatarLink = fundraiser.Owner.Value<string>(MemberConstants.Member.Properties.AvatarLink);
        viewModel.Subtitle = fundraiser.Name;
        viewModel.TargetValue = new Money(fundraiser.Goals.Sum(x => x.Amount), fundraiser.Currency);
        viewModel.RaisedValue = new Money(fundraisersContributions.Sum(x => x.CrowdfunderAmount), fundraiser.Currency);

        return viewModel;
    }
}