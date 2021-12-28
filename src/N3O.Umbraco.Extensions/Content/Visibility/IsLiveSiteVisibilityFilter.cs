using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content; 

public class IsLiveSiteVisibilityFilter : IContentVisibilityFilter {
    private readonly IWebHostEnvironment _webHostEnvironment;

    public IsLiveSiteVisibilityFilter(IWebHostEnvironment webHostEnvironment) {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public bool IsFilterFor(IPublishedContent content) => true;

    public bool IsVisible(IPublishedContent content) {
        return _webHostEnvironment.IsProduction();
    }
}