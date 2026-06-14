using N3O.Umbraco.Data.Criteria;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
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

public class FindDataTypesHandler :
    IRequestHandler<FindDataTypesQuery, DataTypeCriteria, IEnumerable<DataTypeRes>> {
    private readonly IDataTypeService _dataTypeService;
    private readonly DataTypeQueryFilter _dataTypeQueryFilter;
    private readonly IUmbracoMapper _mapper;

    public FindDataTypesHandler(IDataTypeService dataTypeService,
                                DataTypeQueryFilter dataTypeQueryFilter,
                                IUmbracoMapper mapper) {
        _dataTypeService = dataTypeService;
        _dataTypeQueryFilter = dataTypeQueryFilter;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<DataTypeRes>> Handle(FindDataTypesQuery req, CancellationToken cancellationToken) {
        var dataTypes = await _dataTypeService.GetAllAsync();

        dataTypes = _dataTypeQueryFilter.Apply(dataTypes, req.Model);

        return dataTypes.Select(_mapper.Map<IDataType, DataTypeRes>);
    }
}
