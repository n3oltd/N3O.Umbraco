using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Crm.Engage.Exceptions;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Validation;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using ProblemDetails = N3O.Umbraco.Exceptions.ProblemDetails;

namespace N3O.Umbraco.Crm.Engage;

public class ServiceClient<TClient> {
    private readonly ILogger<ServiceClient<TClient>> _logger;
    private readonly TClient _client;
    private readonly IJsonProvider _jsonProvider;
    private readonly string _subscriptionId;

    public ServiceClient(TClient client,
                         IJsonProvider jsonProvider,
                         ILogger<ServiceClient<TClient>> logger,
                         string subscriptionId) {
        _client = client;
        _jsonProvider = jsonProvider;
        _subscriptionId = subscriptionId;
        _logger = logger;
    }

    public async Task InvokeAsync(Func<TClient, Func<string, string, string, string, string, string, CancellationToken, Task>> resolve,
                                  CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync(null, "false", "false", null, null, _subscriptionId, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }

    public async Task InvokeAsync(Func<TClient, Func<string, string, string, string, string, string, string, CancellationToken, Task>> resolve,
                                  string routeParameter1,
                                  CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync(routeParameter1, null, "false", "false", null, null, _subscriptionId, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }

    public async Task InvokeAsync(Func<TClient, Func<string, string, string, string, string, string, string, string, CancellationToken, Task>> resolve,
                                  string routeParameter1,
                                  string routeParameter2,
                                  CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync(routeParameter1, routeParameter2, null, "false", "false", null, null, _subscriptionId, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }

    public async Task InvokeAsync<TReq>(Func<TClient, Func<string, string, string, string, string, string, TReq, CancellationToken, Task>> resolve,
                                        TReq req,
                                        CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync(null, "false", "false", null, null, _subscriptionId, req, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }
    
    public async Task InvokeAsync<TReq>(Func<TClient, Func<string, string, string, string, string, string, string, TReq, CancellationToken, Task>> resolve,
                                        TReq req,
                                        CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync(null, null, "false", "false", null, null, _subscriptionId, req, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }

    public async Task<TRes> InvokeAsync<TRes>(Func<TClient, Func<string, string, string, string, string, string, CancellationToken, Task<TRes>>> resolve,
                                              CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            var res = await funcAsync(null, "false", "false", null, null, _subscriptionId, cancellationToken);

            return res;
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }

    public async Task InvokeAsync<TReq>(Func<TClient, Func<string, string, string, string, string, string, string, TReq, CancellationToken, Task>> resolve,
                                        string routeParameter1,
                                        TReq req,
                                        CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync(routeParameter1, null, "false", "false", null, null, _subscriptionId, req, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }

    public async Task InvokeAsync<TReq>(Func<TClient, Func<string, string, string, string, string, string, string, string, TReq, CancellationToken, Task>> resolve,
                                        string routeParameter1,
                                        string routeParameter2,
                                        TReq req,
                                        CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync(routeParameter1, routeParameter2, null, "false", "false", null, null, _subscriptionId, req, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }

    public async Task<TRes> InvokeAsync<TRes>(Func<TClient, Func<string, string, string, string, string, string, string, CancellationToken, Task<TRes>>> resolve,
                                              string routeParameter1,
                                              CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            var res = await funcAsync(routeParameter1,
                                      null,
                                      "false",
                                      "false",
                                      null,
                                      null,
                                      _subscriptionId,
                                      cancellationToken);

            return res;
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }

    public async Task<TRes> InvokeAsync<TRes>(Func<TClient, Func<string, string, string, string, string, string, string, string, CancellationToken, Task<TRes>>> resolve,
                                              string routeParameter1,
                                              string routeParameter2,
                                              CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            var res = await funcAsync(routeParameter1,
                                      routeParameter2,
                                      null,
                                      "false",
                                      "false",
                                      null,
                                      null,
                                      _subscriptionId,
                                      cancellationToken);

            return res;
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }

    public async Task<TRes> InvokeAsync<TReq, TRes>(Func<TClient, Func<string, string, string, string, string, string, string, string, TReq, CancellationToken, Task<TRes>>> resolve,
                                                    string routeParameter1,
                                                    string routeParameter2,
                                                    TReq req,
                                                    CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            var res = await funcAsync(routeParameter1,
                                      routeParameter2,
                                      null,
                                      "false",
                                      "false",
                                      null,
                                      null,
                                      _subscriptionId,
                                      req,
                                      cancellationToken);

            return res;
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }

    public async Task<TRes> InvokeAsync<TReq, TRes>(Func<TClient, Func<string, string, string, string, string, string, TReq, CancellationToken, Task<TRes>>> resolve,
                                                    TReq req,
                                                    CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            var res = await funcAsync(null, "false", "false", null, null, _subscriptionId, req, cancellationToken);

            return res;
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToExceptionWithProblemDetails(ex);
        }
    }

    public async Task<TRes> InvokeAsync<TReq, TRes>(Func<TClient, Func<string, string, string, string, string, string, string, TReq, CancellationToken, Task<TRes>>> resolve,
                                                    string routeParameter1,
                                                    TReq req,
                                                    CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            var res = await funcAsync(routeParameter1, null, "false", "false", null, null, _subscriptionId, req, cancellationToken);

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

                return new ServiceClientException(exception, problemDetails);
            }
        } catch (Exception ex) {
            _logger.LogError(ex, "Error occured converting Engage exception to ServiceClientException {Error}", exception.Message);
            
            throw;
        }
    }

    private bool IsApiException(Exception exception) {
        return exception.GetType().FullName.Contains("ApiException");
    }
}