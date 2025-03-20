using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Events;

public abstract class CheckoutEvent : Request<WebhookCheckout, None> { }