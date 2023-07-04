using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.DirectDebitUK.Commands;
using N3O.Umbraco.Payments.DirectDebitUK.Models;
using N3O.Umbraco.Payments.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.DirectDebitUK.Controllers;

[ApiDocument(DirectDebitUKConstants.ApiName)]
public class DirectDebitUKController : ApiController {
    private readonly IMediator _mediator;

    public DirectDebitUKController(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost("credentials/{flowId:entityId}/store")]
    public async Task<ActionResult<PaymentFlowRes<DirectDebitUKCredential>>> StoreCard(UKBankAccountReq req) {
        var res = await _mediator.SendAsync<StoreAccountDetailsCommand, UKBankAccountReq, PaymentFlowRes<DirectDebitUKCredential>>(req);

        return Ok(res);
    }
}