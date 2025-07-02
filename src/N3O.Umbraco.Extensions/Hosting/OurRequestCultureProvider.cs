using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using N3O.Umbraco.Extensions;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting;

public class OurRequestCultureProvider : IRequestCultureProvider {
    private const string CultureQueryParameter = "culture";
    
    public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext) {
        if (httpContext.Request.Query.ContainsKey(CultureQueryParameter)) {
            var culture = httpContext.Request.Query[CultureQueryParameter].Single();

            if (CultureInfo.GetCultures(CultureTypes.AllCultures).Any(c => c.Name.EqualsInvariant(culture))) {
                return Task.FromResult(new ProviderCultureResult(culture, culture));
            }
        }

        return Task.FromResult<ProviderCultureResult>(null);
    }
}