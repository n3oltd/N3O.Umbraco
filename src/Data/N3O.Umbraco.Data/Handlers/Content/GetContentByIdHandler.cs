using N3O.Umbraco.Data.Queries;
using N3O.Umbraco.Content;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Handlers;

public class GetContentByIdHandler : IRequestHandler<GetContentByIdQuery, None, object> {
    private readonly IContentLocator _contentLocator;
    private readonly IJsonProvider _jsonProvider;

    public GetContentByIdHandler(IContentLocator contentLocator, IJsonProvider jsonProvider) {
        _contentLocator = contentLocator;
        _jsonProvider = jsonProvider;
    }
    
    public Task<object> Handle(GetContentByIdQuery req, CancellationToken cancellationToken) {
        var content = req.ContentId.Run(_contentLocator.ById, true);
        
        var res = _jsonProvider.DeserializeDynamic(_jsonProvider.SerializeObject(content));

        return Task.FromResult(res);
    }
}
