using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers {
    public class GetContentTypeByAliasHandler : IRequestHandler<GetContentTypeByAliasQuery, None, ContentTypeRes> {
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;
        private readonly IUmbracoMapper _mapper;

        public GetContentTypeByAliasHandler(IContentTypeService contentTypeService,
                                            IDataTypeService dataTypeService,
                                            IUmbracoMapper mapper) {
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
            _mapper = mapper;
        }

        public Task<ContentTypeRes> Handle(GetContentTypeByAliasQuery req, CancellationToken cancellationToken) {
            var contentType = _contentTypeService.Get(req.ContentType.Value);

            var properties = contentType.GetUmbracoProperties(_dataTypeService, _contentTypeService)
                                        .Select(_mapper.Map<UmbracoPropertyInfo, UmbracoPropertyInfoRes>)
                                        .ToList();

            var res = new ContentTypeRes();
            res.Alias = contentType.Alias;
            res.Name = contentType.Name;
            res.Properties = properties;

            return Task.FromResult(res);
        }
    }
}