using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanSubscriptionReq;

namespace N3O.Umbraco.Payments.PayPal.Commands;

public class CreateSubscriptionCommand : Request<PayPalCreateSubscriptionReq, PayPalCreateSubscriptionRes> { }