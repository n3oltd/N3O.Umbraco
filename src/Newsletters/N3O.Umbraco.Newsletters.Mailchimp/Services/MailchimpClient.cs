using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Newsletters.Mailchimp.Extensions;
using N3O.Umbraco.Newsletters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Newsletters.Mailchimp;

public class MailchimpClient : INewslettersClient {
    private readonly ILogger _logger;
    private readonly ITextFormatter _textFormatter;
    private readonly IMailChimpManager _manager;
    private readonly string _audienceId;

    public MailchimpClient(ILogger logger, ITextFormatter textFormatter, IMailChimpManager manager, string audienceId) {
        _logger = logger;
        _textFormatter = textFormatter;
        _manager = manager;
        _audienceId = audienceId;
    }

    public async Task<SubscribeResult> SubscribeAsync(IContact contact, CancellationToken cancellationToken = default) {
        try {
            var mergeValues = new Dictionary<string, string> {
                { "FNAME", contact.FirstName },
                { "LNAME", contact.LastName }
            };

            var member = CreateMember(contact.Email, mergeValues);

            await _manager.Members.AddOrUpdateAsync(_audienceId, member);

            return SubscribeResult.ForSuccess();
        } catch (Exception ex) {
            if (ex.IsInvalidApiKey()) {
                _logger.LogError(ex, "Mailchimp API key is invalid");
            } else if (ex.IsNotFound()) {
                _logger.LogError(ex, "Mailchimp audience not found");
            } else {
                _logger.LogError(ex, "Error subscribing {Email}", contact.Email);
            }
            
            return SubscribeResult.ForFailure(_textFormatter.Format<Strings>(s => s.GeneralError),
                                              ex.ToString());
        }
    }

    private Member CreateMember(string email, IReadOnlyDictionary<string, string> mergeValues) {
        var member = new Member();
        member.EmailAddress = email;
        member.Status = Status.Subscribed;

        if (mergeValues.HasAny()) {
            member.MergeFields = mergeValues.ToDictionary(x => x.Key, x => (object) x.Value);
        }

        return member;
    }

    public class Strings : CodeStrings {
        public string GeneralError => "Sorry, we could not subscribe you at this time";
    }
}
