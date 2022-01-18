using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.RegularExpressions;
using Umbraco.Extensions;

namespace N3O.Umbraco.Video.YouTube.TagHelpers {
    [HtmlTargetElement("n3o-youtube-video")]
    public class YouTubeVideoTagHelper : TagHelper {
        [HtmlAttributeName("video-url")]
        public string VideoUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            var videoId = GetVideoId();

            if (videoId == null) {
                output.SuppressOutput();
            } else {
                output.TagName = "div";
                output.Attributes.Add("style", "position: relative; width: 100%; height: 0; padding-bottom: 56.25%;");

                var iframeTag = new TagBuilder("iframe");
                iframeTag.Attributes.Add("src", $"https://www.youtube.com/embed/{videoId}");
                iframeTag.Attributes.Add("frameborder", "0");
                iframeTag.Attributes.Add("allowfullscreen", "true");
                iframeTag.Attributes.Add("style", "position: absolute; top: 0; left: 0; width: 100%; height: 100%;");

                output.Content.SetHtmlContent(iframeTag.ToHtmlString());
            }
        }

        private string GetVideoId() {
            try {
                var match = Regex.Match(VideoUrl, @"((?<=(v|V)/)|(?<=be/)|(?<=(\?|\&)v=)|(?<=embed/))([\w-]+)");

                if (match.Success) {
                    return match.Groups[0].Value;
                }
            } catch { }

            return null;
        }
    }
}