using N3O.Umbraco.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Llms;

public class LlmsSettingsRemoved :
    INotificationAsyncHandler<ContentMovedToRecycleBinNotification>,
    INotificationAsyncHandler<ContentUnpublishedNotification> {
    private readonly ILlmsTxt _llmsTxt;

    public LlmsSettingsRemoved(ILlmsTxt llmsTxt) {
        _llmsTxt = llmsTxt;
    }

    public Task HandleAsync(ContentMovedToRecycleBinNotification notification, CancellationToken cancellationToken) {
        Process(notification.MoveInfoCollection.Select(x => x.Entity));

        return Task.CompletedTask;
    }

    public Task HandleAsync(ContentUnpublishedNotification notification, CancellationToken cancellationToken) {
        Process(notification.UnpublishedEntities);

        return Task.CompletedTask;
    }

    private void Process(IEnumerable<IContent> entities) {
        if (entities.Any(x => x.ContentType.Alias == AliasHelper<LlmsSettingsContent>.ContentTypeAlias())) {
            _llmsTxt.Remove();
        }
    }
}
