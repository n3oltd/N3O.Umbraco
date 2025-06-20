using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Exceptions;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Validation;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using ProblemDetails = N3O.Umbraco.Exceptions.ProblemDetails;

namespace N3O.Umbraco.Cloud;

public class ServiceClient<TClient> {
    private readonly ILogger<ServiceClient<TClient>> _logger;
    private readonly TClient _client;
    private readonly IJsonProvider _jsonProvider;

    public ServiceClient(TClient client,
                         IJsonProvider jsonProvider,
                         ILogger<ServiceClient<TClient>> logger) {
        _client = client;
        _jsonProvider = jsonProvider;
        _logger = logger;
    }

    public async Task InvokeAsync(Func<TClient, Task> apiCallAsync) {
        try {
            await apiCallAsync(_client);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }
    
    public async Task<TRes> InvokeAsync<TRes>(Func<TClient, Task<TRes>> apiCallAsync) {
        try {
            var res = await apiCallAsync(_client);

            return res;
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }
    
    private ExceptionWithProblemDetails ToExceptionWithProblemDetails(Exception exception) {
        try {
            var content = JsonConvert.SerializeObject(exception.GetType().GetProperty("Result").GetValue(exception));
            var statusCode = (int) exception.GetType().GetProperty("StatusCode").GetValue(exception);

            if (statusCode >= 500) {
                _logger.LogError(exception, "Error calling API: {Error}", exception.Message);
            }

            if (statusCode == StatusCodes.Status412PreconditionFailed) {
                var problemDetails = _jsonProvider.DeserializeObject<ValidationProblemDetails>(content);

                return new ValidationException(problemDetails.Errors);
            } else {
                var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(content);

                return new CloudApiException(problemDetails, exception);
            }
        } catch (Exception ex) {
            _logger.LogError(ex,
                             $"Error occured converting exception to {nameof(CloudApiException)}: {{Error}}",
                             exception.Message);
            
            throw;
        }
    }

    private bool IsApiException(Exception exception) {
        return exception.GetType().FullName.Contains("ApiException");
    }
}