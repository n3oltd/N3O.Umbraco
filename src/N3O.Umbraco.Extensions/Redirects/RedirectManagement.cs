using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using NodaTime;
using System;
using System.Linq;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Redirects;

public class RedirectManagement : IRedirectManagement {
    private static readonly string HitCountAlias = AliasHelper<RedirectContent>.PropertyAlias(x => x.HitCount);
    private static readonly string LastHitDateAlias = AliasHelper<RedirectContent>.PropertyAlias(x => x.LastHitDate);
    
    private readonly IContentService _contentService;
    private readonly IClock _clock;
    private readonly IContentCache _contentCache;

    public RedirectManagement(IContentService contentService, IClock clock, IContentCache contentCache) {
        _contentService = contentService;
        _clock = clock;
        _contentCache = contentCache;
    }

    public Redirect FindRedirect(string requestPath) {
        if (!requestPath.HasValue()) {
            return null;
        }

        requestPath = Normalise(requestPath);

        var searchPaths = new[] {
            requestPath,
            requestPath + "/",
            "/" + requestPath,
            "/" + requestPath + "/"
        };

        var redirects = _contentCache.All<RedirectContent>();
        var redirect = redirects.FirstOrDefault(x => searchPaths.Contains(x.Content().Name, true));

        return redirect.IfNotNull(x => new Redirect(x.Content().Key,
                                                    x.HitCount,
                                                    x.LastHitDate,
                                                    x.Temporary,
                                                    x.Content().AbsoluteUrl()));
    }

    public void LogHit(Guid redirectId) {
        var content = _contentService.GetById(redirectId);
        var now = _clock.GetCurrentInstant().ToDateTimeUtc();

        var currentHitCount = content.GetValue<int>(HitCountAlias);

        content.SetValue(HitCountAlias, currentHitCount + 1);
        content.SetValue(LastHitDateAlias, now);

        _contentService.SaveAndPublish(content);
    }

    private string Normalise(string requestPath) {
        requestPath = requestPath.ToLowerInvariant();

        if (requestPath.StartsWith("/")) {
            requestPath = requestPath.Substring(1);
        }

        if (requestPath.EndsWith("/")) {
            requestPath = requestPath.Substring(0, requestPath.Length - 1);
        }

        return requestPath;
    }
}
