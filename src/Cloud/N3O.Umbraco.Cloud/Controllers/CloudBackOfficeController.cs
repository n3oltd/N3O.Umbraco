using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;

namespace N3O.Umbraco.Cloud.Controllers;

[ApiDocument(CloudConstants.BackOfficeApiName)]
public class CloudBackOfficeController : BackofficeAuthorizedApiController {
    private readonly ISubscriptionAccessor _subscriptionAccessor;

    public CloudBackOfficeController(ISubscriptionAccessor subscriptionAccessor) {
        _subscriptionAccessor = subscriptionAccessor;
    }

    [HttpGet("subscription/code")]
    public ActionResult<string> GetSubscriptionCode() {
        var subscription = _subscriptionAccessor.GetSubscription();
        
        return Ok(subscription.Id.Code);
    }
}