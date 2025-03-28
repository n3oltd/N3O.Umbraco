﻿using Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Crm.Engage;

namespace N3O.Umbraco.Elements;

public class CdnUrlAccessor {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ISubscriptionAccessor _subscriptionAccessor;

    public CdnUrlAccessor(IWebHostEnvironment webHostEnvironment, ISubscriptionAccessor subscriptionAccessor) {
        _webHostEnvironment = webHostEnvironment;
        _subscriptionAccessor = subscriptionAccessor;
    }

    public string GetUrl(string path) {
        var subscriptionCode = _subscriptionAccessor.GetSubscription().Id.Code;
        
        var url = new Url(GetBaseUrl());
        url.AppendPathSegment("elements");
        url.AppendPathSegment(subscriptionCode);
        url.AppendPathSegment(path);

        return url;
    }

    private string GetBaseUrl() {
        if (_webHostEnvironment.IsDevelopment()) {
            return "https://cdn-beta.n3o.cloud";
        } else {
            return "https://cdn.n3o.cloud";
        }
    }
}