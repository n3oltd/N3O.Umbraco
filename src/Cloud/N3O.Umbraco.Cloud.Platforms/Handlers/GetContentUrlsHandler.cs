using Flurl;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Cloud.Platforms.Queries;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Utilities;
using Slugify;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Handlers;

public class GetContentUrlsHandler : IRequestHandler<GetContentUrlsQuery, None, ContentUrlsRes> {
    private readonly IContentTypeService _contentTypeService;
    private readonly IContentService _contentService;
    private readonly IContentCache _contentCache;
    private readonly ISlugHelper _slugHelper;

    public GetContentUrlsHandler(IContentTypeService contentTypeService,
                                 IContentService contentService,
                                 IContentCache contentCache,
                                 ISlugHelper slugHelper) {
        _contentTypeService = contentTypeService;
        _contentService = contentService;
        _contentCache = contentCache;
        _slugHelper = slugHelper;
    }

    public Task<ContentUrlsRes> Handle(GetContentUrlsQuery req, CancellationToken cancellationToken) {
        var content = _contentService.GetById(req.ContentId.Value);
        var urlSettings = _contentCache.Single<UrlSettingsContent>();

        var isCampaign = content != null && content.IsCampaign(_contentTypeService);
        var isOffering = content != null && content.IsOffering(_contentTypeService);

        if (content == null || !content.Published || urlSettings == null || (!isCampaign && !isOffering)) {
            return Task.FromResult(new ContentUrlsRes());
        }

        string path;

        if (isCampaign) {
            path = _contentCache.GetCampaignPath(_slugHelper, content.Name);
        } else {
            var parent = _contentService.GetById(content.ParentId);

            path = parent == null ? null : _contentCache.GetOfferingPath(_slugHelper, parent.Name, content.Name);
        }

        var res = new ContentUrlsRes();

        if (path.HasValue()) {
            if (urlSettings.StagingBaseUrl.HasValue()) {
                res.StagingUrl = new Url(urlSettings.StagingBaseUrl).AppendPathSegment(path);
            }

            if (urlSettings.ProductionBaseUrl.HasValue()) {
                res.ProductionUrl = new Url(urlSettings.ProductionBaseUrl).AppendPathSegment(path);
            }
        }

        return Task.FromResult(res);
    }
}
