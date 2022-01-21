using FluentEmail.Core.Interfaces;
using N3O.Umbraco.Content;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout {
    public class CheckoutReceipt : ICheckoutReceipt {
        private readonly IContentCache _contentCache;
        private readonly ISender _sender;

        public CheckoutReceipt(IContentCache contentCache, ISender sender) {
            _contentCache = contentCache;
            _sender = sender;
        }
        
        public Task SendAsync() {
            throw new NotImplementedException();
            
            // var checkoutTemplateEmail = _contentCache.Single<CheckoutTemplateEmailContent>();
            //
            // var viewModel = new CheckoutReceiptEmailModel(_localClock, _formatter, this);
            //
            // var email = FluentEmail.Core.Email
            //                        .From(checkoutTemplateEmail.FromEmail, checkoutTemplateEmail.FromName)
            //                        .UsingTemplateEngine(new HandlebarsTemplateRenderer())
            //                        .To(Account.Email.Address)
            //                        .Subject(checkoutTemplateEmail.Subject)
            //                        .UsingTemplate(checkoutTemplateEmail.Body, viewModel);
            //
            // _emailSender.Send(email);
        }
    }
}