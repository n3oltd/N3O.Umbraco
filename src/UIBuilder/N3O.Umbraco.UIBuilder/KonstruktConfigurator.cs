using Umbraco.UIBuilder.Configuration;
using Umbraco.UIBuilder.Configuration.Builders;

namespace N3O.Umbraco.UIBuilder;

public abstract class KonstruktConfigurator : IConfigurator {
    private static WithSectionConfigBuilder _contentSection;

    public abstract void Configure(UIBuilderConfigBuilder builder);

    protected WithSectionConfigBuilder GetContentSection(UIBuilderConfigBuilder builder) {
        if (_contentSection == null) {
            _contentSection = builder.WithSection("content");
        }

        return _contentSection;
    }
}
