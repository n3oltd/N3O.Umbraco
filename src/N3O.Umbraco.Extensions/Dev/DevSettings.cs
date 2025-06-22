using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Dev;

public static class DevSettings {
    private static readonly List<IDevProfile> DevProfiles = new();
    
    public static void Apply(IWebHostEnvironment webHostEnvironment, IConfiguration configuration) {
        foreach (var devProfile in DevProfiles) {
            if (devProfile.ShouldApply(webHostEnvironment, configuration)) {
                devProfile.Apply(webHostEnvironment, configuration);
            }   
        }
    }
    
    public static void Custom(Action<IWebHostEnvironment, IConfiguration, RuntimeDevProfile> apply,
                              Func<IWebHostEnvironment, IConfiguration, bool> shouldApply = null) {
        var devProfile = new RuntimeDevProfile(apply, shouldApply);

        UseProfile(devProfile);
    }

    public static void UseProfiles() {
        var types = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                t.ImplementsInterface<IDevProfile>() &&
                                                t.HasParameterlessConstructor())
                                 .ApplyAttributeOrdering();

        var devProfiles = types.Select(x => (IDevProfile) Activator.CreateInstance(x)).ToList();
        
        devProfiles.Do(UseProfile);
    }
    
    public static void UseProfile<T>() where T : IDevProfile, new() {
        var profile = new T();

        UseProfile(profile);
    }
    
    public static void UseProfile<T>(T devProfile) where T : IDevProfile {
        DevProfiles.Add(devProfile);
    }
}