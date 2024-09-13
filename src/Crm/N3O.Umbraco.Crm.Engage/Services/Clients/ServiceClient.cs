using Microsoft.Extensions.Logging;
using N3O.Umbraco.Crm.Engage.Exceptions;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Validation;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crm.Engage;

public class ServiceClient<TClient> {
    private readonly ILogger<ServiceClient<TClient>> _logger;
    private readonly TClient _client;
    private readonly string _subscriptionId;

    public ServiceClient(TClient client,
                         ILogger<ServiceClient<TClient>> logger,
                         string subscriptionId) {
        _client = client;
        _subscriptionId = subscriptionId;
        _logger = logger;
    }

    public async Task InvokeAsync(Func<TClient, Func<string, string, string, string, string, CancellationToken, Task>> resolve,
                                  CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync("false", "false", null, null, _subscriptionId, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToServiceClientException(ex);
        }
    }

    public async Task InvokeAsync(Func<TClient, Func<string, string, string, string, string, string, CancellationToken, Task>> resolve,
                                  string routeParameter1,
                                  CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync(routeParameter1, "false", "false", null, null, _subscriptionId, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToServiceClientException(ex);
        }
    }

    public async Task InvokeAsync(Func<TClient, Func<string, string, string, string, string, string, string, CancellationToken, Task>> resolve,
                                  string routeParameter1,
                                  string routeParameter2,
                                  CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync(routeParameter1, routeParameter2, "false", "false", null, null, _subscriptionId, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToServiceClientException(ex);
        }
    }

    public async Task InvokeAsync<TReq>(Func<TClient, Func<string, string, string, string, string, TReq, CancellationToken, Task>> resolve,
                                        TReq req,
                                        CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync("false", "false", null, null, _subscriptionId, req, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToServiceClientException(ex);
        }
    }

    public async Task<TRes> InvokeAsync<TRes>(Func<TClient, Func<string, string, string, string, string, CancellationToken, Task<TRes>>> resolve,
                                              CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            var res = await funcAsync("false", "false", null, null, _subscriptionId, cancellationToken);

            return res;
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToServiceClientException(ex);
        }
    }

    public async Task InvokeAsync<TReq>(Func<TClient, Func<string, string, string, string, string, string, TReq, CancellationToken, Task>> resolve,
                                        string routeParameter1,
                                        TReq req,
                                        CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync(routeParameter1, "false", "false", null, null, _subscriptionId, req, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToServiceClientException(ex);
        }
    }

    public async Task InvokeAsync<TReq>(Func<TClient, Func<string, string, string, string, string, string, string, TReq, CancellationToken, Task>> resolve,
                                        string routeParameter1,
                                        string routeParameter2,
                                        TReq req,
                                        CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            await funcAsync(routeParameter1, routeParameter2, "false", "false", null, null, _subscriptionId, req, cancellationToken);
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToServiceClientException(ex);
        }
    }

    public async Task<TRes> InvokeAsync<TRes>(Func<TClient, Func<string, string, string, string, string, string, CancellationToken, Task<TRes>>> resolve,
                                              string routeParameter1,
                                              CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            var res = await funcAsync(routeParameter1,
                                      "false",
                                      "false",
                                      null,
                                      null,
                                      _subscriptionId,
                                      cancellationToken);

            return res;
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToServiceClientException(ex);
        }
    }

    public async Task<TRes> InvokeAsync<TRes>(Func<TClient, Func<string, string, string, string, string, string, string, CancellationToken, Task<TRes>>> resolve,
                                              string routeParameter1,
                                              string routeParameter2,
                                              CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            var res = await funcAsync(routeParameter1,
                                      routeParameter2,
                                      "false",
                                      "false",
                                      null,
                                      null,
                                      _subscriptionId,
                                      cancellationToken);

            return res;
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToServiceClientException(ex);
        }
    }

    public async Task<TRes> InvokeAsync<TReq, TRes>(Func<TClient, Func<string, string, string, string, string, string, string, TReq, CancellationToken, Task<TRes>>> resolve,
                                                    string routeParameter1,
                                                    string routeParameter2,
                                                    TReq req,
                                                    CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            var res = await funcAsync(routeParameter1,
                                      routeParameter2,
                                      "false",
                                      "false",
                                      null,
                                      null,
                                      _subscriptionId,
                                      req,
                                      cancellationToken);

            return res;
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToServiceClientException(ex);
        }
    }

    public async Task<TRes> InvokeAsync<TReq, TRes>(Func<TClient, Func<string, string, string, string, string, TReq, CancellationToken, Task<TRes>>> resolve,
                                                    TReq req,
                                                    CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            var res = await funcAsync("false", "false", null, null, _subscriptionId, req, cancellationToken);

            return res;
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToServiceClientException(ex);
        }
    }

    public async Task<TRes> InvokeAsync<TReq, TRes>(Func<TClient, Func<string, string, string, string, string, string, TReq, CancellationToken, Task<TRes>>> resolve,
                                                    string routeParameter1,
                                                    TReq req,
                                                    CancellationToken cancellationToken = default) {
        var funcAsync = resolve(_client);

        try {
            var res = await funcAsync(routeParameter1, "false", "false", null, null, _subscriptionId, req, cancellationToken);

            return res;
        } catch (Exception ex) when (IsApiException(ex)) {
            throw ToServiceClientException(ex);
        }
    }

    private ServiceClientException ToServiceClientException(Exception exception) {
        _logger.LogError(exception, "Error calling API: {Error}", exception.Message);

        try {
            var content = JsonConvert.SerializeObject(exception.GetType().GetProperty("Result").GetValue(exception));

            if ((int) exception.GetType().GetProperty("StatusCode").GetValue(exception) == (int) HttpStatusCode.PreconditionFailed) {
                var problemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(content);

                return new ServiceClientException(exception, problemDetails);
            } else {
                var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(content);

                return new ServiceClientException(problemDetails);
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