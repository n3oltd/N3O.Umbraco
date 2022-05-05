using N3O.Umbraco.Data.Criteria;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers {
    public class FindDataTypesHandler :
        IRequestHandler<FindDataTypesQuery, DataTypeCriteria, IEnumerable<DataTypeSummary>> {
        private readonly IDataTypeService _dataTypeService;

        public FindDataTypesHandler(IDataTypeService dataTypeService) {
            _dataTypeService = dataTypeService;
        }
        
        public Task<IEnumerable<DataTypeSummary>> Handle(FindDataTypesQuery req, CancellationToken cancellationToken) {
            throw new System.NotImplementedException();
        }
    }
}