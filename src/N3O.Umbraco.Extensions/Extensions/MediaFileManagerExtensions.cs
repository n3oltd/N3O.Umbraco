using System.Collections.Concurrent;
using System.IO;
using System.Text;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.Extensions;

public static class MediaFileManagerExtensions {
    private static readonly ConcurrentDictionary<string, string> InlineSvgCache = new();

    public static string InlineSvg(this MediaFileManager mediaFileManager, string srcPath) {
        return InlineSvgCache.GetOrAdd(srcPath, () => {
            using (var stream = mediaFileManager.FileSystem.OpenFile(srcPath)) {
                using (var reader = new StreamReader(stream, Encoding.UTF8)) {
                    return reader.ReadToEnd();
                }
            }
        });
    }
}