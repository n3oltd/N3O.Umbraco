using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers;

public class GetContentTypeByAliasHandler : IRequestHandler<GetContentTypeByAliasQuery, None, ContentTypeRes> {
    private readonly IContentTypeService _contentTypeService;
    private readonly IUmbracoMapper _mapper;

    public GetContentTypeByAliasHandler(IContentTypeService contentTypeService, IUmbracoMapper mapper) {
        _contentTypeService = contentTypeService;
        _mapper = mapper;
    }

    public Task<ContentTypeRes> Handle(GetContentTypeByAliasQuery req, CancellationToken cancellationToken) {
        var contentType = _contentTypeService.Get(req.ContentType.Value);

        var res = _mapper.Map<IContentType, ContentTypeRes>(contentType);

        return Task.FromResult(res);
    }
}