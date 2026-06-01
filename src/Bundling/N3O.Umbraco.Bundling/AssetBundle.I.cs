namespace N3O.Umbraco.Bundling;

// ISmidgeRequire (Smidge) was removed in Umbraco 14.
// TODO: Replace with ES module-based bundling registration.
public interface IAssetBundle {
    void Require(object bundle);
}
