using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;

namespace N3O.Umbraco.Dev;

public static class DevSettings {
    public static void Configure(Action<ConfiguredDevProfile> apply) {
        var profile = new ConfiguredDevProfile(apply);

        UseProfile(profile);
    }

    public static void UseProfiles() {
        var types = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                t.ImplementsInterface<IDevProfile>() &&
                                                t.HasParameterlessConstructor())
                                 .ApplyAttributeOrdering();

        var profiles = types.Select(x => (IDevProfile) Activator.CreateInstance(x)).ToList();
        
        profiles.Do(UseProfile);
    }
    
    public static void UseProfile<T>() where T : IDevProfile, new() {
        var profile = new T();

        UseProfile(profile);
    }
    
    public static void UseProfile<T>(T profile) where T : IDevProfile {
        if (profile.ShouldApply()) {
            profile.Apply();
        }
    }
}