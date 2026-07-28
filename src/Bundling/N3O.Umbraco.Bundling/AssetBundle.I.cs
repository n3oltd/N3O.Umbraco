using Smidge;

namespace N3O.Umbraco.Bundling;

public interface IAssetBundle {
    void Require(ISmidgeRequire bundle);
}