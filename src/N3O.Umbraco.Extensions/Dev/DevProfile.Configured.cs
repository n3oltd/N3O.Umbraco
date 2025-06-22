using System;

namespace N3O.Umbraco.Dev;

public class ConfiguredDevProfile : DevProfile {
    private readonly Action<ConfiguredDevProfile> _apply;

    public ConfiguredDevProfile(Action<ConfiguredDevProfile> apply) {
        _apply = apply;
    }
    
    public override void Apply() {
        _apply(this);
    }

    public override bool ShouldApply() => true;
}