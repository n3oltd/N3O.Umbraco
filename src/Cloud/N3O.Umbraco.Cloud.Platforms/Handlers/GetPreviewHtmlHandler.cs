using N3O.Umbraco.Cloud.Platforms.Queries;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Handlers;

public class GetPreviewHtmlHandler : IRequestHandler<GetPreviewHtmlQuery, Dictionary<string, object>, string> {
    private readonly IReadOnlyList<IPreviewTagGenerator> _previewTagGenerators;

    public GetPreviewHtmlHandler(IEnumerable<IPreviewTagGenerator> previewTagGenerators) {
        _previewTagGenerators = previewTagGenerators.ApplyAttributeOrdering();
    }
    
    public async Task<string> Handle(GetPreviewHtmlQuery req, CancellationToken cancellationToken) {
        var previewTagGenerator = req.ContentTypeAlias.Run(x => _previewTagGenerators.FirstOrDefault(y => y.CanGeneratePreview(x)),
                                                           true);

        var res = await previewTagGenerator.GeneratePreviewTagAsync(req.Model);

        return res;
    }
}