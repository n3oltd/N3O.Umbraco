using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using N3O.Umbraco.Email.Commands;
using N3O.Umbraco.Email.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NewEmail = FluentEmail.Core.Email;

namespace N3O.Umbraco.Email.Handlers {
    public class SendEmailHandler : IRequestHandler<SendEmailCommand, SendEmailReq, None> {
        private readonly ITemplateRenderer _renderer;
        private readonly ISender _sender;

        public SendEmailHandler(ITemplateRenderer renderer, ISender sender) {
            _renderer = renderer;
            _sender = sender;
        }
        
        public async Task<None> Handle(SendEmailCommand req, CancellationToken cancellationToken) {
            var templateModel = GetTemplateModel(req.Model.Model);
            var email = NewEmail.From(req.Model.From.Email, req.Model.From.Name);

            email.Renderer = _renderer;
            email.Sender = _sender;

            AddRecipients(req.Model.To, email.To);
            AddRecipients(req.Model.Cc, email.CC);
            AddRecipients(req.Model.Bcc, email.BCC);

            email.Subject(await _renderer.ParseAsync(req.Model.Subject, templateModel, false));
            email.UsingTemplate(req.Model.Body, templateModel);

            await email.SendAsync(cancellationToken);

            return None.Empty;
        }

        private object GetTemplateModel(object model) {
            if (model is JObject jObject) {
                var dict = new Dictionary<string, object>();
                
                foreach (var (key, value) in jObject) {
                    dict[key] = value.ConvertToObject();
                }

                model = dict;
            }

            return model;
        }
        
        private void AddRecipients(IEnumerable<IEmailIdentity> recipients, Func<string, string, IFluentEmail> add) {
            foreach (var recipient in recipients.OrEmpty()) {
                add(recipient.Email, recipient.Name);
            }
        }
    }
}