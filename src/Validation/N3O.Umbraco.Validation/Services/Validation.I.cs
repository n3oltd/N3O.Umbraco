using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Validation;

public interface IValidation {
    Task<IEnumerable<ValidationFailure>> ValidateModelAsync<T>(T model,
                                                               bool implicitlyValidateChildProperties = false,
                                                               string validationPrefix = null,
                                                               CancellationToken cancellationToken = default);

    Task<IEnumerable<ValidationFailure>> ValidateModelAsync(Type modelType,
                                                            object model,
                                                            bool implicitlyValidateChildProperties = false,
                                                            string validationPrefix = null,
                                                            CancellationToken cancellationToken = default);
}