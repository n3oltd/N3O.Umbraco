using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using N3O.Umbraco.Email.Commands;
using N3O.Umbraco.Email.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NewEmail = FluentEmail.Core.Email;

namespace N3O.Umbraco.Email.Handlers;

public class SendEmailHandler : IRequestHandler<SendEmailCommand, SendEmailReq, None> {
    private readonly IJsonProvider _jsonProvider;
    private readonly ITemplateRenderer _renderer;
    private readonly ISender _sender;

    public SendEmailHandler(IJsonProvider jsonProvider, ITemplateRenderer renderer, ISender sender) {
        _jsonProvider = jsonProvider;
        _renderer = renderer;
        _sender = sender;
    }

    public async Task<None> Handle(SendEmailCommand req, CancellationToken cancellationToken) {
        var templateModel = _jsonProvider.DeserializeObject(req.Model.ModelJson, Type.GetType(req.Model.ModelType));
        var email = NewEmail.From(req.Model.From.Email, req.Model.From.Name);

        email.Renderer = _renderer;
        email.Sender = _sender;

        AddRecipients(req.Model.To, email.To);
        AddRecipients(req.Model.Cc, email.CC);
        AddRecipients(req.Model.Bcc, email.BCC);

        email.Subject(await _renderer.ParseAsync(req.Model.Subject, templateModel, false));
        email.UsingTemplate(req.Model.Body, templateModel);
        email.Tag("website");

        var response = await email.SendAsync(cancellationToken);

        if (!response.Successful) {
            var message = $"Error occured while sending email to {email.Data.ToAddresses.First().Name}";
            
            foreach (var errorMessage in response.ErrorMessages) {
                message += $"\n{errorMessage}";
            }
            
            throw new Exception(message);
        }

        return None.Empty;
    }

    private void AddRecipients(IEnumerable<IEmailIdentity> recipients, Func<string, string, IFluentEmail> add) {
        foreach (var recipient in recipients.OrEmpty()) {
            add(recipient.Email, recipient.Name);
        }
    }
}
