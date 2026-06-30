using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Cloud.Platforms.Queries;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Handlers;

public class GetPreviewHtmlHandler : IRequestHandler<GetPreviewHtmlQuery, Dictionary<string, object>, PreviewHtmlRes> {
    private readonly IReadOnlyList<IPreviewHtmlGenerator> _previewHtmlGenerators;
    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;

    public GetPreviewHtmlHandler(IEnumerable<IPreviewHtmlGenerator> previewHtmlGenerators,
                                 IContentService contentService,
                                 IContentTypeService contentTypeService) {
        _previewHtmlGenerators = previewHtmlGenerators.ApplyAttributeOrdering();
        _contentService = contentService;
        _contentTypeService = contentTypeService;
    }

    public async Task<PreviewHtmlRes> Handle(GetPreviewHtmlQuery req, CancellationToken cancellationToken) {
        try {
            var previewHtmlGenerator = req.ContentId.Run(id => {
                var contentTypeAlias = GetContentTypeAlias(id);

                return _previewHtmlGenerators.FirstOrDefault(x => x.CanGeneratePreview(contentTypeAlias));
            }, true);

            var res = new PreviewHtmlRes();
            (res.ETag, res.Html) = await previewHtmlGenerator.GeneratePreviewHtmlAsync(req.Model);

            return res;
        } catch (Exception e) {
            var res = new PreviewHtmlRes();
            res.Html = e.Message;

            return res;
        }
    }

    private string GetContentTypeAlias(Guid contentId) {
        var content = _contentService.GetById(contentId);

        if (content == null) {
            return null;
        }

        var contentType = _contentTypeService.Get(content.ContentTypeId);

        return contentType?.Alias;
    }
}
