using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Cloud.Platforms.Queries;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Handlers;

public class GetPreviewHtmlHandler : IRequestHandler<GetPreviewHtmlQuery, Dictionary<string, object>, PreviewHtmlRes> {
    private readonly IReadOnlyList<IPreviewHtmlGenerator> _previewHtmlGenerators;

    public GetPreviewHtmlHandler(IEnumerable<IPreviewHtmlGenerator> previewHtmlGenerators) {
        _previewHtmlGenerators = previewHtmlGenerators.ApplyAttributeOrdering();
    }
    
    public async Task<PreviewHtmlRes> Handle(GetPreviewHtmlQuery req, CancellationToken cancellationToken) {
        var previewHtmlGenerator = req.ContentTypeAlias.Run(x => _previewHtmlGenerators.FirstOrDefault(y => y.CanGeneratePreview(x)),
                                                            true);
        
        var res = new PreviewHtmlRes();
        (res.ETag, res.Html) = await previewHtmlGenerator.GeneratePreviewHtmlAsync(req.Model);

        return res;
    }
}