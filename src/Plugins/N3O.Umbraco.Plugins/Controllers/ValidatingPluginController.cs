using N3O.Umbraco.Extensions;
using N3O.Umbraco.Plugins.Controllers;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Validation.Hosting.Controllers;

public class ValidatingPluginController : PluginController {
    private readonly IValidation _validation;
    private readonly Lazy<IValidationHandler> _validationHandler;
    
    public ValidatingPluginController(IValidation validation, Lazy<IValidationHandler> validationHandler) {
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