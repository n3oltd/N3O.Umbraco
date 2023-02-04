using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using MimeKit;
using N3O.Umbraco.Email.Extensions;
using N3O.Umbraco.Email.Lookups;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Email.Amazon;

public class AmazonSender : ISender {
    private readonly IMimeMessageBuilder _mimeMessageBuilder;
    private readonly AmazonSimpleEmailServiceClient _sesClient;

    public AmazonSender(IMimeMessageBuilder mimeMessageBuilder, string accessKey, string secretKey, string regionCode) {
        _mimeMessageBuilder = mimeMessageBuilder;

        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        var regionEndpoint = typeof(RegionEndpoint).GetConstantOrStaticValues<RegionEndpoint>()
                                                   .SingleOrDefault(x => x.SystemName.EqualsInvariant(regionCode));

        _sesClient = new AmazonSimpleEmailServiceClient(credentials, regionEndpoint);
    }

    public SendResponse Send(IFluentEmail email, CancellationToken? cancellationToken = null) {
        var sendResponse = SendAsync(email, cancellationToken).GetAwaiter().GetResult();

        return sendResponse;
    }

    public async Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? cancellationToken = null) {
        var mimeMessage = BuildMimeMessage(email.Data);

        var sendResponse = await SendViaAmazonAsync(mimeMessage, cancellationToken.GetValueOrDefault());

        return sendResponse;
    }

    private MimeMessage BuildMimeMessage(EmailData email) {
        var message = _mimeMessageBuilder.BuildMessage(email.FromAddress.ToEmailIdentity(),
                                                       email.OrEmpty(x => x.ToAddresses)
                                                            .Select(x => x.ToEmailIdentity())
                                                            .ToList(),
                                                       email.OrEmpty(x => x.CcAddresses)
                                                            .Select(x => x.ToEmailIdentity())
                                                            .ToList(),
                                                       email.OrEmpty(x => x.BccAddresses)
                                                            .Select(x => x.ToEmailIdentity())
                                                            .ToList(),
                                                       email.Subject,
                                                       email.Body,
                                                       email.IsHtml ? BodyFormats.Html : BodyFormats.Text,
                                                       email.OrEmpty(x => x.Attachments)
                                                            .Select(x => x.ToEmailAttachment())
                                                            .ToList());

        return message;
    }

    private async Task<SendResponse> SendViaAmazonAsync(MimeMessage mimeMessage, CancellationToken cancellationToken) {
        var sendResponse = new SendResponse();

        try {
            using (var messageStream = mimeMessage.ToStream()) {
                var req = new SendRawEmailRequest();
                req.RawMessage = new RawMessage(messageStream);

                var sesResponse = await _sesClient.SendRawEmailAsync(req, cancellationToken);

                sendResponse.MessageId = sesResponse.MessageId;
            }
        } catch (AccountSendingPausedException ex) {
            sendResponse.ErrorMessages.Add($"{ex.StatusCode}");
            sendResponse.ErrorMessages.Add($"{ex.Message}");
        } catch (MailFromDomainNotVerifiedException ex) {
            sendResponse.ErrorMessages.Add($"{ex.StatusCode}");
            sendResponse.ErrorMessages.Add($"{ex.Message}");
        } catch (MessageRejectedException ex) {
            sendResponse.ErrorMessages.Add($"{ex.StatusCode}");
            sendResponse.ErrorMessages.Add($"{ex.Message}");
        } catch (Exception ex) {
            sendResponse.ErrorMessages.Add(ex.Message);
        }

        return sendResponse;
    }
}