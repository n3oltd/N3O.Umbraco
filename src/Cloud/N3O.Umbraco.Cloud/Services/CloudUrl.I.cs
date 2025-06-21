using N3O.Umbraco.Cloud.Lookups;

namespace N3O.Umbraco.Cloud;

public interface ICloudUrl {
    string ForApi(string servicePath);
    string ForCdn(CdnRoot root, string path);
}