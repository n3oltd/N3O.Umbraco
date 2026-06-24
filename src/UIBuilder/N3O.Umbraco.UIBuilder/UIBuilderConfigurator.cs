using System.Runtime.CompilerServices;
using Umbraco.UIBuilder.Configuration;
using Umbraco.UIBuilder.Configuration.Builders;

namespace N3O.Umbraco.UIBuilder;

public abstract class UIBuilderConfigurator : IConfigurator {
    // Keyed by builder instance so each UIBuilder config run gets exactly one "content"
    // section, correctly shared across all configurators in that run, without carrying
    // state across re-invocations of the factory lambda.
    private static readonly ConditionalWeakTable<UIBuilderConfigBuilder, WithSectionConfigBuilder> _sections = new();

    public abstract void Configure(UIBuilderConfigBuilder builder);

    protected WithSectionConfigBuilder GetContentSection(UIBuilderConfigBuilder builder) {
        return _sections.GetValue(builder, b => b.WithSection("content"));
    }
}
