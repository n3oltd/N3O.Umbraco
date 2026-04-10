namespace N3O.Umbraco.Localization;

public class NullStringLocalizerContent : IStringLocalizerContent {
    public int CreateFolder(string name, int parentId) {
        return -1;
    }

    public int CreateTextContainer(string name, int folderId, int? containerId) {
        return -1;
    }
}