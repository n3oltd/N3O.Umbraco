using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class GetPagePropertyValueHandler : IRequestHandler<GetPagePropertyValueQuery, None, PagePropertyValueRes> {
    private readonly IContentService _contentService;
    private readonly IUmbracoMapper _mapper;

    public GetPagePropertyValueHandler(IUmbracoMapper mapper, IContentService contentService) {
        _mapper = mapper;
        _contentService = contentService;
    }

    public Task<PagePropertyValueRes> Handle(GetPagePropertyValueQuery req, CancellationToken cancellationToken) {
        var page = req.PageId.Run(_contentService.GetById, true);
        var property = req.PropertyAlias.Run(alias => page.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(alias)), true);
        
        var res = _mapper.Map<IProperty, PagePropertyValueRes>(property);
        
        return Task.FromResult(res);
    }
}