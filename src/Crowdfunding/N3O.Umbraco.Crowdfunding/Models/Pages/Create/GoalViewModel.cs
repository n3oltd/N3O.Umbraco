using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public class GoalViewModel {
    public Guid GoalId { get; set; }
    public string GoalName { get; set; }
    public AllocationType Type { get; set; }
    public IEnumerable<FeedbackCustomFieldDefinitionElement> CustomFieldDefinitions { get; set; }
    
    public static GoalViewModel For(CampaignGoalElement campaignGoal) {
        var viewModel = new GoalViewModel();
        viewModel.GoalId = campaignGoal.Content().Key;
        viewModel.GoalName = campaignGoal.Title;
        viewModel.Type = campaignGoal.Type;

        if (viewModel.Type == AllocationTypes.Feedback) {
            viewModel.CustomFieldDefinitions = campaignGoal.Feedback.Scheme.CustomFields;
        }

        return viewModel;
    }
}