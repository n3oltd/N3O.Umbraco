using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using N3O.Umbraco.Utilities;
using System.Linq;

namespace N3O.Umbraco.Hosting;

public class HttpStatusCodesConvention : IActionModelConvention {
    public void Apply(ActionModel action) {
        if (OurAssemblies.IsOurAssembly(action.ActionMethod.DeclaringType?.Assembly)) {
            ApplyNotFoundFilter(action);
            ApplyValidationErrorFilter(action);
        }
    }
    
    private void ApplyNotFoundFilter(ActionModel action) {
        if(action.Attributes.OfType<HttpGetAttribute>().Any()) {
            action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status404NotFound));
        }
    }
    
    private void ApplyValidationErrorFilter(ActionModel action) {
        if(action.Attributes.OfType<HttpPostAttribute>().Any()) {
            action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status412PreconditionFailed));
        } 
    }
}