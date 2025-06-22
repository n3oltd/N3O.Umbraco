using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace N3O.Umbraco.Dev;

public class ConfiguredDevProfile : DevProfile {
    private readonly Action<IWebHostEnvironment, IConfiguration, ConfiguredDevProfile> _apply;

    public ConfiguredDevProfile(Action<IWebHostEnvironment, IConfiguration, ConfiguredDevProfile> apply) {
        _apply = apply;
    }
    
    public override void Apply(IWebHostEnvironment webHostEnvironment, IConfiguration configuration) {
        _apply(webHostEnvironment, configuration, this);
    }

    public override bool ShouldApply() => true;
}