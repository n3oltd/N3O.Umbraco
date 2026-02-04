using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Newsletters.Models;
using N3O.Umbraco.Newsletters.SendGrid.Content;
using N3O.Umbraco.Newsletters.SendGrid.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Newsletters.SendGrid;

public class SendGridNewslettersClient : INewslettersClient {
    private readonly ILogger _logger;
    private readonly ITextFormatter _textFormatter;
    private readonly IMarketingApiClient _marketingApiClient;
    private readonly IContentCache _contentCache;

    public SendGridNewslettersClient(ILogger<SendGridNewslettersClient> logger,
                                     ITextFormatter textFormatter,
                                     IMarketingApiClient marketingApiClient,
                                     IContentCache contentCache) {
        _logger = logger;
        _textFormatter = textFormatter;
        _marketingApiClient = marketingApiClient;
        _contentCache = contentCache;
    }

    public async Task<SubscribeResult> SubscribeAsync(IContact contact, CancellationToken cancellationToken = default) {
        try {
            var settings = _contentCache.Single<SendGridSettingsContent>();

            var reservedFieldValues = new Dictionary<string, object>();
            reservedFieldValues["email"] = contact.Email;

            if (contact.FirstName.HasValue()) {
                reservedFieldValues["first_name"] = contact.FirstName;
            }

            if (contact.LastName.HasValue()) {
                reservedFieldValues["last_name "] = contact.LastName;
            }

            await _marketingApiClient.AddOrUpdateContactAsync(contact.Email,
                                                              settings.ListId,
                                                              reservedFieldValues,
                                                              null);

            return SubscribeResult.ForSuccess();
        } catch (Exception ex) {
            if (ex.IsInvalidApiKey()) {
                _logger.LogError(ex, "SendGrid API key is invalid");
            } else if (ex.IsNotFound()) {
                _logger.LogError(ex, "SendGrid list not found");
            } else {
                _logger.LogError(ex, "Error subscribing {Email}", contact.Email);
            }
        
            return SubscribeResult.ForFailure(_textFormatter.Format<Strings>(s => s.GeneralError),
                                              ex.ToString());
        }
    }

    public class Strings : CodeStrings {
        public string GeneralError => "Sorry, we could not subscribe you at this time";
    }
}