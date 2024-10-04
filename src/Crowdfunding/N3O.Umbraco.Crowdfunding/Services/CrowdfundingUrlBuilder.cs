using Flurl;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Configuration.Models;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingUrlBuilder : ICrowdfundingUrlBuilder {
    private readonly RequestHandlerSettings _requestHandlerSettings;

    public CrowdfundingUrlBuilder(IContentLocator contentLocator,
                                  IOptionsMonitor<RequestHandlerSettings> requestHandlerSettings) {
        ContentLocator = contentLocator;
        _requestHandlerSettings = requestHandlerSettings.CurrentValue;
    }
    
    public string GenerateUrl(string path, Action<Url> addQueryParameters = null) {
        var home = ContentLocator.Single<HomePageContent>();

        var url = new Url(home.Content().AbsoluteUrl());

        if (path.HasValue()) {
            url.AppendPathSegment(path);
        }

        addQueryParameters?.Invoke(url);

        var urlStr = url.ToString();

        if (_requestHandlerSettings.AddTrailingSlash && !urlStr.EndsWith("/")) {
            urlStr += "/";
        }

        return urlStr;
    }

    public IContentLocator ContentLocator { get; }
}