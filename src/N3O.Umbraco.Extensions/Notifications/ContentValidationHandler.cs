using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Notifications {
    public class ContentValidationHandler : INotificationAsyncHandler<ContentSavingNotification> {
        private readonly ILogger _logger;
        private readonly IContentHelper _contentHelper;
        private readonly IReadOnlyList<IContentValidator> _contentValidators;
        private readonly IReadOnlyList<INestedContentItemValidator> _nestedContentItemValidators;

        public ContentValidationHandler(ILogger<ContentValidationHandler> logger,
                                        IContentHelper contentHelper,
                                        IEnumerable<IContentValidator> contentValidators,
                                        IEnumerable<INestedContentItemValidator> nestedContentItemValidators) {
            _logger = logger;
            _contentHelper = contentHelper;
            _contentValidators = contentValidators.OrEmpty().ToList();
            _nestedContentItemValidators = nestedContentItemValidators.OrEmpty().ToList();
        }

        public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
            foreach (var content in notification.SavedEntities) {
                try {
                    ValidateContent(content);

                    foreach (var nestedContent in _contentHelper.GetNestedContent(content)) {
                        ValidateNestedContent(content, nestedContent);
                    }
                } catch (ContentValidationErrorException error) {
                    notification.CancelOperation(error.PopupMessage);
                } catch (ContentValidationWarningException warning) {
                    notification.Messages.Add(warning.PopupMessage);
                } catch (Exception ex) {
                    _logger.LogError(ex,
                                     "Error whilst validating content of type {Type} with ID {ID}",
                                     content.ContentType.Alias,
                                     content.Id);
                }
            }
        
            return Task.CompletedTask;
        }

        private void ValidateContent(IContent content) {
            foreach (var contentValidator in _contentValidators) {
                if (contentValidator.IsValidator(content)) {
                    contentValidator.Validate(content);
                }
            }
        }

        private void ValidateNestedContent(IContent content, IPublishedElement item) {
            foreach (var validator in _nestedContentItemValidators) {
                if (validator.IsValidator(item)) {
                    validator.Validate(item, content);
                }
            }
        }
    }
}