using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.UIBuilder.Configuration;
using Umbraco.UIBuilder.Extensions;

namespace N3O.Umbraco.UIBuilder;

public class UIBuilderComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.AddUIBuilder(cfg => {
            var configurators = OurAssemblies
                .GetTypes(t => t.IsConcreteClass() &&
                               t.HasParameterlessConstructor() &&
                               t.ImplementsInterface<IConfigurator>())
                .ApplyAttributeOrdering()
                .Select(t => (IConfigurator) Activator.CreateInstance(t))
                .ToList();

            foreach (var configurator in configurators) {
                configurator.Configure(cfg);
            }
        });
    }
}
