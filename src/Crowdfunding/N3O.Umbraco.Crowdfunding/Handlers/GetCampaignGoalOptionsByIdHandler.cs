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

public class GetCampaignGoalOptionsByIdHandler : IRequestHandler<GetCampaignGoalOptionsByIdQuery, None, GoalOptionRes> {
    private readonly IContentLocator _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public GetCampaignGoalOptionsByIdHandler(IContentLocator contentLocator, IUmbracoMapper mapper) {
        _contentLocator = contentLocator;
        _mapper = mapper;
    }
    
    public Task<GoalOptionRes> Handle(GetCampaignGoalOptionsByIdQuery req, CancellationToken cancellationToken) {
        var content = req.ContentId.Run(_contentLocator.ById<CampaignContent>, true);
        var goal = req.GoalId.Run(x => content.GoalOptions.Single(o => o.GoalId == x), true);
        
        var res = _mapper.Map<CampaignGoalOptionElement, GoalOptionRes>(goal);
        
        return Task.FromResult(res);
    }
}