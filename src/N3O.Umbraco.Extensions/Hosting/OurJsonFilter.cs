using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Json;
using System;
using System.Buffers;

namespace N3O.Umbraco.Hosting {
    [AttributeUsage(AttributeTargets.Class)]
    public class OurJsonFilter : ActionFilterAttribute {
        public override void OnActionExecuted(ActionExecutedContext ctx) {
            if (ctx.Result is ObjectResult objectResult) {
                var jsonProvider = ctx.HttpContext.RequestServices.GetRequiredService<IJsonProvider>();
                var serializerSettings = jsonProvider.GetSettings();
                var arrayPool = ctx.HttpContext.RequestServices.GetRequiredService<ArrayPool<char>>();
                var mvcOptions = ctx.HttpContext.RequestServices.GetRequiredService<IOptions<MvcOptions>>().Value;
                
                objectResult.Formatters.Insert(0, new OurJsonOutputFormatter(serializerSettings, arrayPool, mvcOptions));
            }
        }
    }
}