namespace N3O.Umbraco.Dev;

public interface IDevProfile {
    void Apply();
    bool ShouldApply();
}