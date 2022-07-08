using N3O.Umbraco.Data.Criteria;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
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
    IRequestHandler<FindDataTypesQuery, DataTypeCriteria, IEnumerable<DataTypeSummary>> {
    private readonly IDataTypeService _dataTypeService;
    private readonly IUmbracoMapper _mapper;

    public FindDataTypesHandler(IDataTypeService dataTypeService, IUmbracoMapper mapper) {
        _dataTypeService = dataTypeService;
        _mapper = mapper;
    }
    
    public Task<IEnumerable<DataTypeSummary>> Handle(FindDataTypesQuery req, CancellationToken cancellationToken) {
        var dataTypes = _dataTypeService.GetByEditorAlias(req.Model.EditorAlias)
                                        .Select(_mapper.Map<IDataType, DataTypeSummary>);

        return Task.FromResult(dataTypes);
    }
}
