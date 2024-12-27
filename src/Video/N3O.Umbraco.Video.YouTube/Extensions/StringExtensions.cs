using System.Text.RegularExpressions;

namespace N3O.Umbraco.Video.YouTube.Extensions;

public static class StringExtensions {
    public static string GetYouTubeVideoId(this string videoUrl) {
        try {
            var match = Regex.Match(videoUrl, @"((?<=(v|V)/)|(?<=be/)|(?<=(\?|\&)v=)|(?<=embed/))([\w-]+)");

            if (match.Success) {
                return match.Groups[0].Value;
            }
        } catch { }

        return null;
    }
}