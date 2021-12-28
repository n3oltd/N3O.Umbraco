using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Composing;

public abstract class Composer : IComposer {
    // TODO Remove once Umbraco exposes this in composers
    public static IWebHostEnvironment WebHostEnvironment { get; set; }
    
    public abstract void Compose(IUmbracoBuilder builder);

    protected void RegisterAll(Func<Type, bool> typePredicate, Action<Type> registerAction) {
        RegisterAll(typePredicate, (t, _) => registerAction(t));
    }

    protected void RegisterAll(Func<Type, bool> typePredicate, Action<Type, int> registerAction) {
        var matches = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                  !t.IsGenericTypeDefinition &&
                                                  typePredicate(t))
                                   .ApplyAttributeOrdering()
                                   .SelectWithIndex()
                                   .ToList();

        foreach (var (type, index) in matches) {
            registerAction(type, index);
        }
    }
}
