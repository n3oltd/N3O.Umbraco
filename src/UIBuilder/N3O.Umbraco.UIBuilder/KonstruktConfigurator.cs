using Konstrukt.Configuration;
using Konstrukt.Configuration.Builders;

namespace N3O.Umbraco.UIBuilder;

public abstract class KonstruktConfigurator : IKonstruktConfigurator {
    private static KonstruktWithSectionConfigBuilder _contentSection;
    
    public abstract void Configure(KonstruktConfigBuilder builder);

    protected KonstruktWithSectionConfigBuilder GetContentSection(KonstruktConfigBuilder builder) {
        if (_contentSection == null) {
            _contentSection = builder.WithSection("content");
        }

        return _contentSection;
    }
}