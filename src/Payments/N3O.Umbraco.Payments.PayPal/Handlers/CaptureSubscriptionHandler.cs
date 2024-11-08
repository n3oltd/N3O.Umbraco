using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.PayPal.Clients;
using N3O.Umbraco.Payments.PayPal.Clients.Models;
using N3O.Umbraco.Payments.PayPal.Clients.PayPalErrors;
using N3O.Umbraco.Payments.PayPal.Commands;
using N3O.Umbraco.Payments.PayPal.Models;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCredential;
using Refit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Handlers;

public class CaptureSubscriptionHandler : PaymentsHandler<CaptureSubscriptionCommand, PayPalSubscriptionReq, PayPalCredential> {
    private readonly IContentCache _contentCache;
    private readonly ISubscriptionsClient _subscriptionsClient;

    public CaptureSubscriptionHandler(IContentCache contentCache,
                                      IPaymentsScope paymentsScope,
                                      ISubscriptionsClient subscriptionsClient) : base(paymentsScope) {
        _contentCache = contentCache;
        _subscriptionsClient = subscriptionsClient;
    }

    protected override async Task HandleAsync(CaptureSubscriptionCommand req,
                                                   PayPalCredential payment,
                                                   PaymentsParameters parameters,
                                                   CancellationToken cancellationToken) {
        var request = new ApiActivateSubscriptionReq();

        request.Id = req.Model.SubscriptionId;
        request.Reason = req.Model.Reason;

        try {
            await _subscriptionsClient.ActivateSubscriptionAsync(request);

            payment.Subscribed(req.Model.SubscriptionId, req.Model.Reason);
        } catch (ApiException apiException) {
            payment.Error((int) apiException.StatusCode, apiException.Message);
        }
    }
}