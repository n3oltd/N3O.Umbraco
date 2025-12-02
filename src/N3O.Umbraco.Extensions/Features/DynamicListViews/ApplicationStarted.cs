using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using static N3O.Umbraco.Features.DynamicListViews.DynamicListViewConstants;

namespace N3O.Umbraco.Features.DynamicListViews;

public class ApplicationStarted : INotificationAsyncHandler<UmbracoApplicationStartedNotification> {
    private readonly IContentTypeService _contentTypeService;
    private readonly IContentService _contentService;
    private readonly ICoreScopeProvider _provider;

    public ApplicationStarted(IContentTypeService contentTypeService,
                              IContentService contentService,
                              ICoreScopeProvider provider) {
        _contentTypeService = contentTypeService;
        _contentService = contentService;
        _provider = provider;
    }

    public Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken) {
        var dynamicListViewContent = GetDynamicListViewContent().ToList();

        var contentWithDynamicListViewsEnabled = dynamicListViewContent.Where(x => x.GetValue<bool>(Properties.EnableDynamicListView))
                                                                       .Select(x => x.Id);

        DynamicListViewsCache.AddRange(contentWithDynamicListViewsEnabled);

        return Task.CompletedTask;
    }

    private IEnumerable<IContent> GetDynamicListViewContent() {
        var contentTypes = _contentTypeService.GetAll()
                                              .Where(x => x.CompositionPropertyTypes
                                                           .Select(y => y.Alias)
                                                           .Any(y => y.EqualsInvariant(Properties.EnableDynamicListView)))
                                              .Select(x => x.Id)
                                              .ToList();

        var query = _provider.CreateQuery<IContent>().Where(x => contentTypes.Contains(x.ContentTypeId));

        var content = _contentService.GetPagedDescendants(-1, 0, int.MaxValue, out _, query).ToList();

        return content;
    }
}