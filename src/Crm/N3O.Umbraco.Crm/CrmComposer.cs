using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Crm;

public class CrmComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(CrmConstants.ApiName);
        
        builder.Services.AddTransient<IAccountIdentityAccessor, AccountIdentityAccessor>();
        builder.Services.AddTransient<ICrmCartIdAccessor, CrmCartIdAccessor>();
    }
}