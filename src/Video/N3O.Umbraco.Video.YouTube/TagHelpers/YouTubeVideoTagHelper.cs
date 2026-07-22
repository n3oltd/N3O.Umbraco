using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Video.YouTube.Extensions;
using Umbraco.Extensions;

namespace N3O.Umbraco.Video.YouTube.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}youtube-video")]
public class YouTubeVideoTagHelper : TagHelper {
    [HtmlAttributeName("video-url")]
    public string VideoUrl { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        var videoId = VideoUrl.GetYouTubeVideoId();

        if (videoId == null) {
            output.SuppressOutput();

            return;
        }

        var host = VideoUrl.InvariantContains("youtube-nocookie.com")
                       ? "https://www.youtube-nocookie.com"
                       : "https://www.youtube.com";

        var iframeTag = new TagBuilder("iframe");

        foreach (var attribute in output.Attributes) {
            iframeTag.Attributes[attribute.Name] = attribute.Value?.ToString();
        }

        iframeTag.Attributes["src"] = $"{host}/embed/{videoId}?enablejsapi=1";

        if (!iframeTag.Attributes.ContainsKey("frameborder")) {
            iframeTag.Attributes["frameborder"] = "0";
        }

        if (!iframeTag.Attributes.ContainsKey("allowfullscreen")) {
            iframeTag.Attributes["allowfullscreen"] = "true";
        }

        if (!iframeTag.Attributes.ContainsKey("style")) {
            iframeTag.Attributes["style"] = "position: absolute; top: 0; left: 0; width: 100%; height: 100%; z-index: 1;";
        }

        output.TagName = "div";
        output.Attributes.Clear();
        output.Attributes.Add("style", "position: relative; width: 100%; height: 0; padding-bottom: 56.25%;");
        output.Content.SetHtmlContent(iframeTag.ToHtmlString());
    }
}