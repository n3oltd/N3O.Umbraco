using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System.Linq;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Mediator;

public class MediatorComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddMediatR(opt => {
            opt.RegisterServicesFromAssemblies(OurAssemblies.GetAllAssemblies().ToArray());
        });

        builder.Services.AddTransient<IMediator, Mediator>();
        builder.Services.AddTransient<IRequestFactory, RequestFactory>();
        builder.Services.AddSingleton<None>();

        RegisterAll(t => t.IsSubclassOrSubInterfaceOfGenericType(typeof(Request<,>)),
                    t => builder.Services.AddTransient(t, t));
    }
}
