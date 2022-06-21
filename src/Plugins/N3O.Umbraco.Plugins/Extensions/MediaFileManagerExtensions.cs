using System.Linq;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.Plugins.Extensions;

public static class MediaFileManagerExtensions {
    public static string GetSourceFile(this MediaFileManager mediaFileManager, string mediaId) {
        return mediaFileManager.FileSystem
                               .GetFiles(mediaId, "*.*")
                               .OrderBy(x => mediaFileManager.FileSystem.GetLastModified(x))
                               .FirstOrDefault();
    }
}
