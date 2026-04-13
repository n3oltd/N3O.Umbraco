namespace N3O.Umbraco.Localization;

public interface IStringLocalizerContent {
    int CreateFolder(string name, int parentId);
    int CreateTextContainer(string name, int folderId, int? containerId);
}
