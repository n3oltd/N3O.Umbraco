using N3O.Umbraco.Crowdfunding.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public class CreatePageViewModel {
    public IEnumerable<GoalViewModel> Goals { get; set; }
    
    public static CreatePageViewModel For(CampaignContent campaign) {
        var goals = new List<GoalViewModel>();

        foreach (var goal in campaign.Goals) {
            goals.Add(GoalViewModel.For(goal));
        }
        
        var viewModel = new CreatePageViewModel();
        viewModel.Goals = goals;

        return viewModel;
    }
}