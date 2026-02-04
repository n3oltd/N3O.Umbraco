using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Newsletters.Models;
using N3O.Umbraco.Validation;
using N3O.Umbraco.Validation.Controllers;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Newsletters.Controllers;

[ApiDocument(NewslettersConstants.ApiName)]
public class NewslettersController : ValidatingApiController {
    private readonly ILogger<NewslettersController> _logger;
    private readonly INewslettersClient _client;

    public NewslettersController(ILogger<NewslettersController> logger,
                                 INewslettersClient client,
                                 IValidation validation,
                                 Lazy<IValidationHandler> validationHandler) 
        : base(validation, validationHandler) {
        _logger = logger;
        _client = client;
    }

    [HttpPost("subscribe")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<SubscribeResult>> Subscribe(ContactReq req) {
        await ValidateAsync(req);
        
        var result = await _client.SubscribeAsync(req);

        if (result.Subscribed) {
            return Ok(result);
        } else {
            _logger.LogError("Failed to subscribe {Req} due to error {Error}", req, result.ErrorDetails);
            
            return UnprocessableEntity(result);
        }
    }
}
