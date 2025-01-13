using N3O.Umbraco.Crm.Lookups;
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

public class GetFundraiserPagesHandler : IRequestHandler<GetFundraiserPagesQuery, FundraiserPagesCriteria, FundraiserDashboardRes> {
    private readonly ICrowdfunderRepository _crowdfunderRepository;
    private readonly IUmbracoMapper _umbracoMapper;
    
    public GetFundraiserPagesHandler(ICrowdfunderRepository crowdfunderRepository, IUmbracoMapper umbracoMapper) {
        _crowdfunderRepository = crowdfunderRepository;
        _umbracoMapper = umbracoMapper;
    }

    public async Task<FundraiserDashboardRes> Handle(GetFundraiserPagesQuery req, CancellationToken cancellationToken) {
        var nextPage = req.Model.CurrentPage.GetValueOrDefault() + 1;
        var pageSize = req.Model.PageSize.GetValueOrDefault();
        
        var fundraisers = await _crowdfunderRepository.GetPagedCrowdfundersAsync(CrowdfunderTypes.Fundraiser,
                                                                                 nextPage,
                                                                                 pageSize);
        
        var res = _umbracoMapper.Map<Page<Crowdfunder>, FundraiserDashboardRes>(fundraisers);

        return res;
    }
}