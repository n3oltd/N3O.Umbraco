using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanSubscriptionReq;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCredential;
using Org.BouncyCastle.Asn1.Ocsp;

namespace N3O.Umbraco.Payments.PayPal.Commands;

public class CreateSubscriptionCommand : Request<PayPalCreateSubscriptionReq, PayPalCreateSubscriptionRes> { }