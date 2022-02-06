using N3O.Umbraco.Email.Commands;
using N3O.Umbraco.Email.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Scheduler;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Email {
    public class FluentEmailBuilder : IFluentEmailBuilder {
        private readonly Lazy<IBackgroundJob> _backgroundJob;
        private readonly List<string> _recipientEmails = new();
        
        private EmailIdentityReq _from;
        private readonly List<EmailIdentityReq> _to = new();
        private readonly List<EmailIdentityReq> _cc = new();
        private readonly List<EmailIdentityReq> _bcc = new();
        private string _subject;
        private string _body;
        private object _model;

        public FluentEmailBuilder(Lazy<IBackgroundJob> backgroundJob) {
            _backgroundJob = backgroundJob;
        }

        public IFluentEmailBuilder From(string email, string name = null) {
            _from = new EmailIdentityReq();

            _from.Email = email;
            _from.Name = name;

            return this;
        }

        public IFluentEmailBuilder To(string email, string name = null) {
            return AddRecipient(_to, email, name);
        }

        public IFluentEmailBuilder Cc(string email, string name = null) {
            return AddRecipient(_cc, email, name);
        }

        public IFluentEmailBuilder Bcc(string email, string name = null) {
            return AddRecipient(_bcc, email, name);
        }

        public IFluentEmailBuilder Subject(string text) {
            _subject = text;

            return this;
        }

        public IFluentEmailBuilder Body(string content) {
            _body = content;

            return this;
        }

        public IFluentEmailBuilder Model(object model) {
            _model = model;

            return this;
        }

        public SendEmailReq Build() {
            var req = new SendEmailReq();
            req.From = _from;
            req.To = _to;
            req.Cc = _cc;
            req.Bcc = _bcc;
            req.Subject = _subject;
            req.Body = _body;
            req.Model = _model;

            return req;
        }

        public void Queue() {
            var req = Build();
            var jobName = $"Send {req.Subject} to {_recipientEmails.Join("; ")}";
            
            _backgroundJob.Value.Enqueue<SendEmailCommand, SendEmailReq>(jobName, req);
        }

        private IFluentEmailBuilder AddRecipient(List<EmailIdentityReq> list, string email, string name) {
            if (list.None(x => x.Email.EqualsInvariant(email))) {
                var identity = new EmailIdentityReq();
                identity.Name = name;
                identity.Email = email;

                list.Add(identity);
                
                _recipientEmails.AddIfNotExists(email);
            }

            return this;
        }
    }
}
