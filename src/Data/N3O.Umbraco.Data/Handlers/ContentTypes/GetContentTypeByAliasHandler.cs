using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers {
    public class GetContentTypeByAliasHandler :
        IRequestHandler<GetContentTypeByAliasQuery, None, ContentTypeRes> {
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;

        public GetContentTypeByAliasHandler(IContentTypeService contentTypeService,
                                            IDataTypeService dataTypeService) {
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
        }

        public Task<ContentTypeRes> Handle(GetContentTypeByAliasQuery req,
                                           CancellationToken cancellationToken) {
            var contentType = _contentTypeService.Get(req.ContentType.Value);

            var properties = contentType.GetUmbracoProperties(_dataTypeService, _contentTypeService)
                                        .Select(x => new UmbracoPropertyRes(x.Type.Name, x.Type.Alias, x.Group.Name, x.DataType.EditorAlias))
                                        .ToList();

            var res = new ContentTypeRes();
            res.Properties = properties;
            res.Name = contentType.Name;

            return Task.FromResult(res);
        }
    }
}