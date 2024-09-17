using Humanizer;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderContributionViewModel {
    public string Name { get; private set; }
    public string Comment { get; private set; }
    public string AvatarLink { get; private set; }
    public string DonatedAt { get; private set; }
    public bool IsAnonymous { get; private set; }
    public Money Value { get; private set; }
    public Money TaxReliefValue { get; private set; }

    public static CrowdfunderContributionViewModel For(IFormatter formatter,
                                                       Currency crowdfunderCurrency,
                                                       Contribution contribution) {
        var viewModel = new CrowdfunderContributionViewModel();

        viewModel.IsAnonymous = contribution.Anonymous;
        viewModel.Comment = contribution.Comment;
        viewModel.DonatedAt = contribution.Timestamp.Humanize();
        viewModel.Name = contribution.Name ?? formatter.Text.Format<Strings>(s => s.Anonymous);
        viewModel.AvatarLink = viewModel.Name.GetGravatarUrl();
        viewModel.Value = new Money(contribution.CrowdfunderAmount, crowdfunderCurrency);
        viewModel.TaxReliefValue = new Money(contribution.TaxReliefCrowdfunderAmount, crowdfunderCurrency);

        return viewModel;
    }

    public class Strings : CodeStrings {
        public string Anonymous => "Anonymous";
    }
}