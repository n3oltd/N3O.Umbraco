using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Validation.Hosting.Controllers;

public class ValidatingApiController : ApiController {
    private readonly IValidation _validation;
    private readonly Lazy<IValidationHandler> _validationHandler;
    
    public ValidatingApiController(IValidation validation, Lazy<IValidationHandler> validationHandler) {
        _validation = validation;
        _validationHandler = validationHandler;
    }

    public async Task ValidateAsync<T>(T req) {
        var failures = await _validation.ValidateModelAsync(req, true);

        if (failures.HasAny()) {
            _validationHandler.Value.Handle(failures);
        }
    }
}