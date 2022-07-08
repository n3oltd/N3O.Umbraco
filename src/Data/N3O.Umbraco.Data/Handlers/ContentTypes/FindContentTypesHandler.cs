using N3O.Umbraco.Data.Criteria;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.QueryFilters;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers;

public class FindContentTypesHandler :
    IRequestHandler<FindContentTypesQuery, ContentTypeCriteria, IEnumerable<ContentTypeRes>> {
    private readonly IContentTypeService _contentTypeService;
    private readonly ContentTypeQueryFilter _contentTypeQueryFilter;
    private readonly IUmbracoMapper _mapper;

    public FindContentTypesHandler(IContentTypeService contentTypeService,
                                   ContentTypeQueryFilter contentTypeQueryFilter,
                                   IUmbracoMapper mapper) {
        _contentTypeService = contentTypeService;
        _contentTypeQueryFilter = contentTypeQueryFilter;
        _mapper = mapper;
    }

    public Task<IEnumerable<ContentTypeRes>> Handle(FindContentTypesQuery req, CancellationToken cancellationToken) {
        var contentTypes = _contentTypeService.GetAll();

        contentTypes = _contentTypeQueryFilter.Apply(contentTypes, req.Model);

        var res = contentTypes.Select(x => _mapper.Map<IContentType, ContentTypeRes>(x)).ToList();

        return Task.FromResult<IEnumerable<ContentTypeRes>>(res);
    }
}
