using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanRes;

namespace N3O.Umbraco.Payments.PayPal.Commands;

public class CreatePlanCommand : Request<None, PayPalCreatePlanRes>{ }