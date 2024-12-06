using N3O.Umbraco.Data.Converters;
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

namespace N3O.Umbraco.Data.Handlers;

public class GetImportablePropertiesHandler
    : IRequestHandler<GetImportablePropertiesQuery, None, DataProperties> {
    private readonly IContentTypeService _contentTypeService;
    private readonly IDataTypeService _dataTypeService;
    private readonly IReadOnlyList<IPropertyConverter> _propertyConverters;
    private readonly IReadOnlyList<IImportPropertyFilter> _propertyFilters;

    public GetImportablePropertiesHandler(IContentTypeService contentTypeService,
                                          IDataTypeService dataTypeService,
                                          IEnumerable<IPropertyConverter> propertyConverters,
                                          IEnumerable<IImportPropertyFilter> propertyFilters) {
        _contentTypeService = contentTypeService;
        _dataTypeService = dataTypeService;
        _propertyConverters = propertyConverters.ToList();
        _propertyFilters = propertyFilters.ToList();
    }
    
    public Task<DataProperties> Handle(GetImportablePropertiesQuery req, CancellationToken cancellationToken) {
        var contentType = _contentTypeService.Get(req.ContentType);

        var importableProperties = contentType.GetUmbracoProperties(_dataTypeService, _contentTypeService)
                                              .Where(x => x.HasPropertyConverter(_propertyConverters) &&
                                                          x.CanInclude(_propertyFilters))
                                              .Select(x => new DataProperty(x.Type.Alias, x.GetColumnTitle()))
                                              .ToList();

        return Task.FromResult(new DataProperties(importableProperties));
    }
}
