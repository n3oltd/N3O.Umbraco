using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class GetNestedPropertySchemaHandler :
    IRequestHandler<GetNestedPropertySchemaQuery, None, NestedPropertySchemaRes> {
    public async Task<NestedPropertySchemaRes> Handle(GetNestedPropertySchemaQuery req,
                                                      CancellationToken cancellationToken) {
        // TODO Talha
        throw new NotImplementedException();
    }
}