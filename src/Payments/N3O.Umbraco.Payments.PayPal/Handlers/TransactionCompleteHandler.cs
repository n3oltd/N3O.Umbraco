using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.PayPal.Client;
using N3O.Umbraco.Payments.PayPal.Client.Models;
using N3O.Umbraco.Payments.PayPal.Commands;
using N3O.Umbraco.Payments.PayPal.Content;
using N3O.Umbraco.Payments.PayPal.Extensions;
using N3O.Umbraco.Payments.PayPal.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Handlers {
    public class TransactionCompleteHandler : PaymentsHandler<TransactionCompleteCommand, PayPalTransactionReq, PayPalPayment> {
        private readonly IContentCache _contentCache;
        private readonly IPayPalClient _payPalClient;

        public TransactionCompleteHandler(IContentCache contentCache, IPaymentsScope paymentsScope, IPayPalClient payPalClient) : base(paymentsScope) {
            _contentCache = contentCache;
            _payPalClient = payPalClient;
        }

        protected override async Task HandleAsync(TransactionCompleteCommand req,
                                                  PayPalPayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            var settings = _contentCache.Single<PayPalSettingsContent>();

            var request = GetAuthorizePaymentReq(req.Model, parameters, settings);

            var res = await _payPalClient.AuthorizePaymentAsync(request);

            if (res.IsAuthorised()) {
                payment.Paid(req.Model.Email, res.Id);
            } else if (res.IsDeclined()) {
                payment.Declined(res.Id, req.Model.Email, res.StatusDetails?.Reason);
            } else if (res.IsFailed()) {
                payment.Error(res.Id, res.StatusDetails?.Reason);
            }
        }

        private AuthorizePaymentReq GetAuthorizePaymentReq(PayPalTransactionReq req, PaymentsParameters parameters, PayPalSettingsContent settings) {
            var request = new AuthorizePaymentReq();
            request.FinalCapture = true;
            request.InvoiceId = parameters.FlowId;
            request.NoteToPayer = parameters.GetTransactionDescription(settings);
            request.SoftDescriptor = parameters.GetTransactionId(settings, req.AuthorizationId);
            request.AuthorizationId = req.AuthorizationId;

            return request;
        }
    }
}