using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace N3O.Umbraco.Dev;

public class RuntimeDevProfile : DevProfile {
    private readonly Action<IWebHostEnvironment, IConfiguration, RuntimeDevProfile> _apply;
    private readonly Func<IWebHostEnvironment, IConfiguration, bool> _shouldApply;

    public RuntimeDevProfile(Action<IWebHostEnvironment, IConfiguration, RuntimeDevProfile> apply,
                             Func<IWebHostEnvironment, IConfiguration, bool> shouldApply) {
        _apply = apply;
        _shouldApply = shouldApply;
    }
    
    public override void Apply(IWebHostEnvironment webHostEnvironment, IConfiguration configuration) {
        _apply(webHostEnvironment, configuration, this);
    }

    public override bool ShouldApply(IWebHostEnvironment webHostEnvironment, IConfiguration configuration) {
        return _shouldApply?.Invoke(webHostEnvironment, configuration) ?? true;
    }
}