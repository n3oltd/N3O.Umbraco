using Microsoft.AspNetCore.Html;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Media;

public interface IMediaUrl {
    string GetCropUrl(MediaWithCrops mediaWithCrops,
                      string cropAlias,
                      string furtherOptions = null,
                      UrlMode urlMode = UrlMode.Default);

    string GetCropUrl(MediaWithCrops mediaWithCrops,
                      int width,
                      string furtherOptions = null,
                      UrlMode urlMode = UrlMode.Default);

    string GetCropUrl(MediaWithCrops mediaWithCrops,
                      int width,
                      int height,
                      string furtherOptions = null,
                      UrlMode urlMode = UrlMode.Default);

    string GetMediaUrl(MediaWithCrops mediaWithCrops,
                       string furtherOptions = null,
                       UrlMode urlMode = UrlMode.Default);

    HtmlString InlineSvg(MediaWithCrops mediaWithCrops);
}