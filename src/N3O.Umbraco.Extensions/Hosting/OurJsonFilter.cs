using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Buffers;

namespace N3O.Umbraco.Hosting;

[AttributeUsage(AttributeTargets.Class)]
public class OurJsonFilter : ActionFilterAttribute {
    public override void OnActionExecuted(ActionExecutedContext ctx) {
        if (ctx.Result is ObjectResult objectResult) {
            var jsonOptions = ctx.HttpContext
                                 .RequestServices
                                 .GetRequiredService<IOptions<MvcNewtonsoftJsonOptions>>()
                                 .Value;
            var arrayPool = ctx.HttpContext.RequestServices.GetRequiredService<ArrayPool<char>>();
            var mvcOptions = ctx.HttpContext.RequestServices.GetRequiredService<IOptions<MvcOptions>>().Value;
            
            objectResult.Formatters.Insert(0, new OurJsonOutputFormatter(jsonOptions, arrayPool, mvcOptions));
        }
    }
}
