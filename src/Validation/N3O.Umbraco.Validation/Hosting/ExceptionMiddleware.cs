using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using System;
using System.Net;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Validation;

public class ExceptionMiddleware : IMiddleware {
    private readonly IFormatter _formatter;
    private readonly IJsonProvider _jsonProvider;
    private readonly Lazy<ILogger<ExceptionMiddleware>> _logger;
    private readonly Lazy<IUmbracoContextFactory> _umbracoContextFactory;
    private readonly Lazy<IWebHostEnvironment> _webHostEnvironment;
    
    public ExceptionMiddleware(IFormatter formatter,
                               IJsonProvider jsonProvider,
                               Lazy<ILogger<ExceptionMiddleware>> logger,
                               Lazy<IUmbracoContextFactory> umbracoContextFactory,
                               Lazy<IWebHostEnvironment> webHostEnvironment) {
        _formatter = formatter;
        _jsonProvider = jsonProvider;
        _logger = logger;
        _umbracoContextFactory = umbracoContextFactory;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        try {
            await next(context);
        } catch (Exception ex) {
            using (_umbracoContextFactory.Value.EnsureUmbracoContext()) {
                var problemDetailsException = ex as ExceptionWithProblemDetails;
            
                if (problemDetailsException == null) {
                    problemDetailsException = new UnhandledExceptionWrapper(ex);
                }
            
                if (ex is not ValidationException) {
                    var endpoint = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();

                    _logger.Value
                           .LogError(ex,
                                     "An unhandled exception occured executing {ControllerName} / {ActionName}. Exception message: {Message}",
                                     endpoint.ControllerName,
                                     endpoint.ActionName,
                                     ex.Message);
                }

                await WriteProblemDetailsAsync(context.Response, problemDetailsException);
            }
        }
    }
    
    private async Task WriteProblemDetailsAsync(HttpResponse response, ExceptionWithProblemDetails exception) {
        response.Clear();

        var problemDetails = exception.GetProblemDetails(_formatter);
        
        if (_webHostEnvironment.Value.IsProduction()) {
            problemDetails = new ProblemDetails((HttpStatusCode) problemDetails.Status,
                                                problemDetails.Title,
                                                _formatter.Text.Format<Strings>(s => s.GeneralMessage));
        }
        
        var json = _jsonProvider.SerializeObject(problemDetails);

        response.StatusCode = problemDetails.Status;
        response.ContentType = "application/problem+json";

        await response.WriteAsync(json);
    }
    
    public class Strings : CodeStrings {
        public string GeneralMessage => "Internal server error. Please contact support if the problem persists.";
    }
    
}