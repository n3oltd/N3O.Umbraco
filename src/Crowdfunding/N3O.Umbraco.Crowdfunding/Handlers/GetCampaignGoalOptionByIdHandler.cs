using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Mediator;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class GetCampaignGoalOptionByIdHandler : IRequestHandler<GetCampaignGoalOptionByIdQuery, None, GoalOptionRes> {
    private readonly IContentLocator _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public GetCampaignGoalOptionByIdHandler(IContentLocator contentLocator, IUmbracoMapper mapper) {
        _contentLocator = contentLocator;
        _mapper = mapper;
    }
    
    public Task<GoalOptionRes> Handle(GetCampaignGoalOptionByIdQuery req, CancellationToken cancellationToken) {
        var campaign = req.CampaignId.Run(_contentLocator.ById<CampaignContent>, true);
        var goalOption = req.GoalOptionId.Run(x => campaign.GoalOptions.SingleOrDefault(o => o.GoalId == x), true);
        
        var res = _mapper.Map<CampaignGoalOptionElement, GoalOptionRes>(goalOption);
        
        return Task.FromResult(res);
    }
}