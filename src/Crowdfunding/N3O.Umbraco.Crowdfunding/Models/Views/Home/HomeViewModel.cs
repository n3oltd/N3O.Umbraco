﻿using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public class HomeViewModel : CrowdfundingViewModel {
    public IReadOnlyList<CrowdfunderCardViewModel> ActiveCampaigns { get; private set; }
    public IReadOnlyList<CrowdfunderCardViewModel> AlmostCompleteFundraisers { get; private set; }
    public IReadOnlyList<CrowdfunderCardViewModel> NewFundraisers { get; private set; }
    public HomePageTemplateContent HomePageTemplate { get; private set; }
    
    public static async Task<HomeViewModel> ForAsync(ICrowdfundingViewModelFactory viewModelFactory,
                                                     ILookups lookups,
                                                     HomePage page,
                                                     IReadOnlyDictionary<string, string> query,
                                                     IReadOnlyList<Crowdfunder> activeCampaigns,
                                                     IReadOnlyList<Crowdfunder> almostCompleteFundraisers,
                                                     IReadOnlyList<Crowdfunder> newFundraisers,
                                                     HomePageTemplateContent homepageTemplate) {
        var viewModel = await viewModelFactory.CreateViewModelAsync<HomeViewModel>(page, query);
        
        viewModel.ActiveCampaigns = activeCampaigns.Select(x => CrowdfunderCardViewModel.For(lookups, x)).ToList();
        viewModel.AlmostCompleteFundraisers = almostCompleteFundraisers.Select(x => CrowdfunderCardViewModel.For(lookups, x)).ToList();
        viewModel.NewFundraisers = newFundraisers.Select(x => CrowdfunderCardViewModel.For(lookups, x)).ToList();
        viewModel.HomePageTemplate = homepageTemplate;

        return viewModel;
    }
}