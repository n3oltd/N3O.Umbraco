using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers {
    public class GetExportablePropertiesHandler
        : IRequestHandler<GetExportablePropertiesQuery, None, ExportableProperties> {
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;
        private readonly IReadOnlyList<IExportPropertyFilter> _propertyFilters;

        public GetExportablePropertiesHandler(IContentService contentService,
                                              IContentTypeService contentTypeService,
                                              IDataTypeService dataTypeService,
                                              IEnumerable<IExportPropertyFilter> propertyFilters) {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
            _propertyFilters = propertyFilters.ToList();
        }
        
        public Task<ExportableProperties> Handle(GetExportablePropertiesQuery req, CancellationToken cancellationToken) {
            var containerContent = req.ContentId.Run(_contentService.GetById, true);
            var contentType = _contentTypeService.GetContentTypeForContainerContent(containerContent.ContentTypeId);

            var exportableProperties = contentType.GetUmbracoProperties(_dataTypeService)
                                                  .Where(x => x.CanInclude(_propertyFilters))
                                                  .Select(x => new ExportableProperty(x.Type.Alias, x.GetName()))
                                                  .ToList();

            return Task.FromResult(new ExportableProperties(exportableProperties));
        }
    }
}