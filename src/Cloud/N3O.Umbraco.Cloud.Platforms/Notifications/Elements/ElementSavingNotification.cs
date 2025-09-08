using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ElementSavingNotification : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly ILookups _lookups;

    public ElementSavingNotification(ILookups lookups) {
        _lookups = lookups;
    }

    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        var elementTypes = _lookups.GetAll<ElementType>();
        
        foreach (var content in notification.SavedEntities) {
            var elementType = elementTypes.SingleOrDefault(x => x.ContentTypeAlias.EqualsInvariant(content.ContentType.Alias));

            if (elementType != null) {
                PopulateEmbedCode(content, elementType);
            }
        }
        
        return Task.CompletedTask;
    }

    private void PopulateEmbedCode(IContent content, ElementType elementType) {
        var embedCode = GenerateEmbedCode(elementType, content.Key);
        
        // TODO Put this embed code in the calculated property
    }

    private string GenerateEmbedCode(ElementType elementType, Guid contentKey) {
        var tag = new TagBuilder(elementType.TagName);
        
        tag.Attributes.Add("id", contentKey.ToString("D"));

        return tag.RenderSelfClosingTag().ToHtmlString();
    }
}