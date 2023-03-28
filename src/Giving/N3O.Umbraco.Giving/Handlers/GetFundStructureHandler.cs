using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Giving.Queries;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Handlers;

public class GetFundStructureHandler : IRequestHandler<GetFundStructureQuery, None, FundStructureRes> {
    private readonly IFundStructureAccessor _fundStructureAccessor;
    private readonly IUmbracoMapper _mapper;

    public GetFundStructureHandler(IFundStructureAccessor fundStructureAccessor, IUmbracoMapper mapper) {
        _fundStructureAccessor = fundStructureAccessor;
        _mapper = mapper;
    }
    
    public Task<FundStructureRes> Handle(GetFundStructureQuery req, CancellationToken cancellationToken) {
        var fundStructure = _fundStructureAccessor.GetFundStructure();
        var res = _mapper.Map<FundStructure, FundStructureRes>(fundStructure);

        return Task.FromResult(res);
    }
}
