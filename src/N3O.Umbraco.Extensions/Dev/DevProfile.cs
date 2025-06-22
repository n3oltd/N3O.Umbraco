using N3O.Umbraco.Hosting;

namespace N3O.Umbraco.Dev;

public abstract class DevProfile : IDevProfile {
    public abstract void Apply();
    public abstract bool ShouldApply();
    
    public void SetDevFlag(string flag) {
        DevFlags.Set(flag);
    }
    
    public void SetEnvironmentData(string key, string value) {
        EnvironmentData.Override(key, value);
    }
    
    public void SetOurEnvironmentData(string key, string value) {
        EnvironmentData.OverrideOur(key, value);
    }
}