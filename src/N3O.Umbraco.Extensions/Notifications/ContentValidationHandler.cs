using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Notifications;

public class ContentValidationHandler : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly ILogger _logger;
    private readonly IContentHelper _contentHelper;
    private readonly IReadOnlyList<IContentValidator> _contentValidators;

    public ContentValidationHandler(ILogger<ContentValidationHandler> logger,
                                    IContentHelper contentHelper,
                                    IEnumerable<IContentValidator> contentValidators) {
        _logger = logger;
        _contentHelper = contentHelper;
        _contentValidators = contentValidators.OrEmpty().ToList();
    }

    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            var contentProperties = _contentHelper.GetContentProperties(content);

            Validate(contentProperties, notification);
        }

        return Task.CompletedTask;
    }

    private void Validate(ContentProperties content, ContentSavingNotification notification) {
        try {
            foreach (var contentValidator in _contentValidators) {
                if (contentValidator.IsValidator(content)) {
                    try {
                        contentValidator.Validate(content);
                    } catch (Exception ex) {
                        _logger.LogError(ex, $"Failed to execute content validator {contentValidator.GetType().GetFriendlyName().Quote()}");
                    }
                }
            }

            var nestedContents = content.NestedContentProperties
                                        .OrEmpty()
                                        .SelectMany(x => x.Value)
                                        .ToList();
            
            foreach (var nestedContent in nestedContents) {
                Validate(nestedContent, notification);
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
