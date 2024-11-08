using N3O.Umbraco.Financial;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Payments.PayPal.Commands;

public class GetOrCreatePlanCommand : Request<MoneyReq, string> { }