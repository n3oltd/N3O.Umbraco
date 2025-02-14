using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public partial class CrowdfundingController {
    [AllowAnonymous]
    [HttpPost("addToCart")]
    public async Task<ActionResult> AddToCart(CrowdfundingCartReq crowdfundingReq) {
        var validationFailures = await _validation.Value.ValidateModelAsync(crowdfundingReq);

        if (validationFailures.HasAny()) {
            _validationHandler.Value.Handle(validationFailures);
        }
        
        var req = crowdfundingReq.ToBulkAddToCartReq(_contentLocator.Value,
                                                     _jsonProvider.Value,
                                                     _crowdfundingUrlBuilder.Value);
        
        var revisionId = await _mediator.Value.SendAsync<BulkAddToCartCommand, BulkAddToCartReq, RevisionId>(req);

        _cartCookie.Value.SetValue(revisionId);
            
        return Ok();
    }
}