using MediatR;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Validation;

public class ValidatorPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> {
    private readonly IValidation _validation;
    private readonly Lazy<IValidationHandler> _validationHandler;

    public ValidatorPipelineBehaviour(IValidation validation, Lazy<IValidationHandler> validationHandler) {
        _validation = validation;
        _validationHandler = validationHandler;
    }

    public async Task<TResponse> Handle(TRequest req,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken) {
        if (!req.GetType().HasAttribute<NoValidationAttribute>()) {
            var model = (req as IModel)?.Model;

            if (model != null) {
                var modelType = model.GetType();

                var validationFailures = await _validation.ValidateModelAsync(modelType,
                                                                              model,
                                                                              true,
                                                                              cancellationToken: cancellationToken);

                if (validationFailures.HasAny()) {
                    _validationHandler.Value.Handle(validationFailures);
                }
            }
        }

        var response = await next();

        return response;
    }
}