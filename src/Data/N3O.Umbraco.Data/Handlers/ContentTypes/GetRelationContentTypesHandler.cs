using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers;

public class GetRelationContentTypesHandler :
    IRequestHandler<GetRelationContentTypesQuery, string, IEnumerable<ContentTypeRes>> {
    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;

    public GetRelationContentTypesHandler(IContentService contentService, IContentTypeService contentTypeService) {
        _contentService = contentService;
        _contentTypeService = contentTypeService;
    }
    
    public Task<IEnumerable<ContentTypeRes>> Handle(GetRelationContentTypesQuery req,
                                                    CancellationToken cancellationToken) {
        var content = req.ContentId.Run(_contentService.GetById, true);
        var contentType = _contentTypeService.Get(content.ContentType.Id);
        var relationContentsTypes = new List<IContentType>();

        switch (req.Model.ToLowerInvariant()) {
            case "child":
                relationContentsTypes.AddRange(contentType.AllowedContentTypes
                                                          .Select(x => _contentTypeService.Get(x.Id.Value)));
                break;
            
            case "descendant":
                PopulateDescendantsContentTypes(relationContentsTypes, contentType);
                break;
            
            default:
                throw UnrecognisedValueException.For(req.Model);
        }

        var res = new List<ContentTypeRes>();

        foreach (var allowedContentType in relationContentsTypes.OrderBy(x => x.Name)) {
            res.Add(new ContentTypeRes {
                Alias = allowedContentType.Alias,
                Name = allowedContentType.Name
            });
        }

        return Task.FromResult<IEnumerable<ContentTypeRes>>(res);
    }

    private void PopulateDescendantsContentTypes(List<IContentType> list, IContentType contentType) {
        var toProcess = contentType.AllowedContentTypes.Where(x => list.None(c => c.Id == x.Id.Value));
        
        foreach (var allowedContentType in toProcess) {
            var childContentType = _contentTypeService.Get(allowedContentType.Id.Value);
            
            list.AddIfNotExists(childContentType);

            PopulateDescendantsContentTypes(list, childContentType);
        }
    }
}
