using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers {
    public class GetAllowedContentTypesHandler :
        IRequestHandler<GetAllowedContentTypesQuery, None, IEnumerable<ContentTypeSummary>> {
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;

        public GetAllowedContentTypesHandler(IContentService contentService, IContentTypeService contentTypeService) {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
        }
        
        public Task<IEnumerable<ContentTypeSummary>> Handle(GetAllowedContentTypesQuery req,
                                                            CancellationToken cancellationToken) {
            var content = req.ContentId.Run(_contentService.GetById, true);
            var contentType = _contentTypeService.Get(content.ContentType.Id);

            var res = new List<ContentTypeSummary>();

            foreach (var allowedContentType in contentType.AllowedContentTypes) {
                var childContentType = _contentTypeService.Get(allowedContentType.Id.Value);
                
                res.Add(new ContentTypeSummary {
                    Alias = childContentType.Alias,
                    Name = childContentType.Name
                });
            }

            return Task.FromResult<IEnumerable<ContentTypeSummary>>(res);
        }
    }
}