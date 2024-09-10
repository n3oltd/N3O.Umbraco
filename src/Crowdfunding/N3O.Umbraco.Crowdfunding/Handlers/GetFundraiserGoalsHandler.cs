using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class GetFundraiserGoalsHandler :
    IRequestHandler<GetFundraiserGoalsQuery, None, FundraiserGoalsRes> {
    private readonly IContentLocator _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public GetFundraiserGoalsHandler(IUmbracoMapper mapper, IContentLocator contentLocator) {
        _mapper = mapper;
        _contentLocator = contentLocator;
    }

    public Task<FundraiserGoalsRes> Handle(GetFundraiserGoalsQuery req, CancellationToken cancellationToken) {
        var content = req.ContentId.Run(_contentLocator.ById<FundraiserContent>, true);
        
        var res = _mapper.Map<FundraiserContent, FundraiserGoalsRes>(content);
        
        return Task.FromResult(res);
    }
}