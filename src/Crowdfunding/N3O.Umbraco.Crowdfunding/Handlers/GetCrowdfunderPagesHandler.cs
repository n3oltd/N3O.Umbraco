using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Mediator;
using NPoco;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class GetCrowdfunderPagesHandler : IRequestHandler<GetCrowdfunderPagesQuery, CrowdfunderPagesCriteria, CrowdfunderDashboardRes> {
    private readonly ICrowdfunderRepository _crowdfunderRepository;
    private readonly IUmbracoMapper _umbracoMapper;
    
    public GetCrowdfunderPagesHandler(ICrowdfunderRepository crowdfunderRepository, IUmbracoMapper umbracoMapper) {
        _crowdfunderRepository = crowdfunderRepository;
        _umbracoMapper = umbracoMapper;
    }

    public async Task<CrowdfunderDashboardRes> Handle(GetCrowdfunderPagesQuery req, CancellationToken cancellationToken) {
        var nextPage = req.Model.CurrentPage.GetValueOrDefault() + 1;
        var pageSize = req.Model.PageSize.GetValueOrDefault();
        
        var fundraisers = await _crowdfunderRepository.SearchPagedAsync(req.Model.Type, nextPage, pageSize);
        
        var res = _umbracoMapper.Map<Page<Crowdfunder>, CrowdfunderDashboardRes>(fundraisers);

        return res;
    }
}