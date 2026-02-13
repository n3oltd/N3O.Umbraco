using Flurl;
using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsPageAccessor : IPlatformsPageAccessor {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IContentCache _contentCache;
    private readonly ICdnClient _cdnClient;
    private readonly IJsonProvider _jsonProvider;

    public PlatformsPageAccessor(IHttpContextAccessor httpContextAccessor,
                                 IContentCache contentCache,
                                 ICdnClient cdnClient,
                                 IJsonProvider jsonProvider) {
        _httpContextAccessor = httpContextAccessor;
        _contentCache = contentCache;
        _cdnClient = cdnClient;
        _jsonProvider = jsonProvider;
    }
    
    public async Task<GetPageResult> GetAsync(CancellationToken cancellationToken = default) {
        if (_httpContextAccessor.HttpContext == null) {
            return null;
        }

        var key = nameof(PlatformsPageAccessor);

        if (!_httpContextAccessor.HttpContext.Items.ContainsKey(key)) {
            var requestUri = _httpContextAccessor.HttpContext.Request.Uri();

            _httpContextAccessor.HttpContext.Items[key] = await GetAsync(requestUri, cancellationToken);
        }
        
        return (GetPageResult) _httpContextAccessor.HttpContext.Items[key];
    }
    
    private async Task<GetPageResult> GetAsync(Uri requestUri, CancellationToken cancellationToken) {
        var fallbacks = new List<SpecialContent>();
        
        foreach (var platformsPageRoute in PlatformsPageRoute.All) {
            var platformsPath = SpecialContentPathParser.ParseUri(_contentCache, platformsPageRoute.Parent, requestUri);

            if (!platformsPath.HasValue()) {
                continue;
            }
        
            var currentPath = platformsPath;

            do {
                var getPageResult = await _cdnClient.DownloadPlatformsPageAsync(_jsonProvider,
                                                                                platformsPageRoute.ContentKind,
                                                                                platformsPageRoute.Parent,
                                                                                currentPath,
                                                                                cancellationToken);

                if (getPageResult.HasValue()) {
                    if (getPageResult.IsRedirect) {
                        return getPageResult;
                    } else {
                        if (currentPath != platformsPath) {
                            var platformsPageUrl = GetPlatformsPageUrl(getPageResult.Page, platformsPageRoute.Parent);

                            return GetPageResult.ForRedirect(platformsPageUrl, false);
                        } else {
                            return getPageResult;
                        }
                    }
                }
            
                var lastPathSegment = currentPath.StripTrailingSlash().LastIndexOf('/');
            
                if (lastPathSegment > 0) {
                    currentPath = currentPath.Substring(0, lastPathSegment);
                } else {
                    break;
                }
            } while (currentPath.HasValue());
            
            fallbacks.Add(platformsPageRoute.Parent);
        }
        
        if (fallbacks.Any()) {
            return GetPageResult.ForRedirect(SpecialContentPathParser.GetPath(_contentCache, fallbacks.First()),
                                             false);
        } else {
            return null;   
        }
    }
    
    private string GetPlatformsPageUrl(PlatformsPage platformsPage, SpecialContent parent) {
        var pagePath = SpecialContentPathParser.GetPath(_contentCache, parent).Trim('/');
        var platformsPath = platformsPage.Path.Trim('/');

        var url = new Url();
        url.AppendPathSegment(pagePath);
        url.AppendPathSegment(platformsPath);
                    
        return url.ToString();
    }
}