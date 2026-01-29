using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Notifications;

[SkipDuringSync]
public class ContentValidationHandler : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly ILogger _logger;
    private readonly IContentHelper _contentHelper;
    private readonly IVariationContextAccessor _variationContextAccessor;
    private readonly IReadOnlyList<IContentValidator> _contentValidators;

    public ContentValidationHandler(ILogger<ContentValidationHandler> logger,
                                    IContentHelper contentHelper,
                                    IEnumerable<IContentValidator> contentValidators,
                                    IVariationContextAccessor variationContextAccessor) {
        _logger = logger;
        _contentHelper = contentHelper;
        _variationContextAccessor = variationContextAccessor;
        _contentValidators = contentValidators.OrEmpty().ToList();
    }

    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            var contentProperties = _contentHelper.GetContentProperties(content, _variationContextAccessor?.VariationContext?.Culture);

            Validate(contentProperties, notification);
        }

        return Task.CompletedTask;
    }

    private void Validate(ContentProperties content, ContentSavingNotification notification) {
        try {
            foreach (var contentValidator in _contentValidators) {
                if (contentValidator.IsValidator(content)) {
                    contentValidator.Validate(content);
                }
            }

            var elements = content.ElementsProperties.OrEmpty().SelectMany(x => x.Value).ToList();
            
            foreach (var element in elements) {
                Validate(element, notification);
            }
        } catch (ContentValidationErrorException error) {
            notification.CancelOperation(error.EventMessage);
        } catch (ContentValidationWarningException warning) {
            notification.Messages.Add(warning.EventMessage);
        } catch (Exception ex) {
            _logger.LogError(ex,
                             "Error whilst validating content of type {Type} with ID {ID}",
                             content.ContentTypeAlias,
                             content.Id);
        }
    }
}
