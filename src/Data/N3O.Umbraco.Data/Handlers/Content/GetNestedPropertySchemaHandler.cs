using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Handlers;

public class GetNestedPropertySchemaHandler : IRequestHandler<GetNestedPropertySchemaQuery, None, NestedSchemaRes> {
    public async Task<NestedSchemaRes> Handle(GetNestedPropertySchemaQuery req,
                                              CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
}