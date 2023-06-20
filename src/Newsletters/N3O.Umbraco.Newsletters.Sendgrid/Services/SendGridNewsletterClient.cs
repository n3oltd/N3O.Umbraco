using SendGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Newsletters.SendGrid.Extensions;
using N3O.Umbraco.Newsletters.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Newsletters.SendGrid;

public class SendGridNewsletterClient : INewslettersClient
{
    private readonly ILogger _logger;
    private readonly ITextFormatter _textFormatter;
    private readonly SendGridClient _baseClient;
    private readonly string _audienceId;


    public SendGridNewsletterClient (ILogger<SendGridNewsletterClient> logger, ITextFormatter textFormatter, SendGridClient baseClient,string audienceId)
    {
        _logger = logger;
        _textFormatter = textFormatter;
        _baseClient = baseClient;
        _audienceId = audienceId;

    }

    public async Task<SubscribeResult> SubscribeAsync(IContact contact, CancellationToken cancellationToken = default)
    {
        try
        {
            var contacts = new List<object>
            {
                new
                {
                    email = contact.Email,
                    first_name = contact.FirstName,
                    last_name = contact.LastName
                }
            };

            string reqData = JsonConvert.SerializeObject( new
            {
                ListIds = new List<string> { _audienceId },
                Contacts = contacts
            });

            await _baseClient.RequestAsync(
                method: SendGridClient.Method.PUT,
                urlPath: "marketing/contacts",
                requestBody: reqData
                );

            return SubscribeResult.ForSuccess();

        }
        catch (Exception ex)
        {
            if (ex.IsInvalidApiKey())
            {
                _logger.LogError(ex, "SendGrid API key is invalid");
            }
            else if (ex.IsNotFound())
            {
                _logger.LogError(ex, "SendGrid audience not found");
            }
            else
            {
                _logger.LogError(ex, "Error subscribing {Email}", contact.Email);
            }

            return SubscribeResult.ForFailure(_textFormatter.Format<Strings>(s => s.GeneralError),
                                              ex.ToString());
        }
    }

    public class Strings : CodeStrings
    {
        public string GeneralError => "Sorry, we could not subscribe you at this time";
    }
}