using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers {
    public class GetDescendantsContentTypesHandler :
        IRequestHandler<GetDescendantsContentTypesQuery, None, IEnumerable<ContentTypeSummary>> {
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;

        public GetDescendantsContentTypesHandler(IContentService contentService, IContentTypeService contentTypeService) {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
        }
        
        public Task<IEnumerable<ContentTypeSummary>> Handle(GetDescendantsContentTypesQuery req,
                                                            CancellationToken cancellationToken) {
            var content = req.ContentId.Run(_contentService.GetById, true);
            var contentType = _contentTypeService.Get(content.ContentType.Id);
            var descendantsContentsTypes = new List<IContentType>();

            PopulateDescendantsContentTypes(descendantsContentsTypes, contentType);

            var res = new List<ContentTypeSummary>();

            foreach (var allowedContentType in descendantsContentsTypes.OrderBy(x => x.Name)) {
                res.Add(new ContentTypeSummary {
                    Alias = allowedContentType.Alias,
                    Name = allowedContentType.Name
                });
            }

            return Task.FromResult<IEnumerable<ContentTypeSummary>>(res);
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
}