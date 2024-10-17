using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderCardViewModel {
    public string JumboImagePath { get; private set; }
    public string TallImagePath { get; private set; }
    public string WideImagePath { get; private set; }
    public string Name { get; private set; }
    public int PercentageComplete { get; private set; }
    public Money RaisedTotal { get; private set; }
    public CrowdfunderOwnerViewModel Owner { get; private set; }
    
    public static CrowdfunderCardViewModel For(ILookups lookups, Crowdfunder crowdfunder) {
        var currency = lookups.FindById<Currency>(crowdfunder.CurrencyCode);
        var raisedTotal = new Money(crowdfunder.NonDonationsTotalQuote + crowdfunder.ContributionsTotalQuote, currency);
        var percentageComplete = Math.Min(100, (raisedTotal.Amount / crowdfunder.GoalsTotalQuote) * 100);
        
        var viewModel = new CrowdfunderCardViewModel();

        viewModel.JumboImagePath = crowdfunder.JumboImage;
        viewModel.TallImagePath = crowdfunder.TallImage;
        viewModel.WideImagePath = crowdfunder.WideImage;
        viewModel.Name = crowdfunder.Name;
        viewModel.RaisedTotal = raisedTotal;
        viewModel.PercentageComplete = (int) percentageComplete;
        viewModel.Owner = CrowdfunderOwnerViewModel.For(crowdfunder.OwnerName, crowdfunder.OwnerProfilePicture, null);

        return viewModel;
    }
}