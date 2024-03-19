using HandlebarsDotNet;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Templates.Handlebars;

public class HandlebarsCompiler : IHandlebarsCompiler {
    private readonly IMemoryCache _cache;
    private readonly IHandlebarsFactory _handlebarsFactory;

    public HandlebarsCompiler(IMemoryCache cache, IHandlebarsFactory handlebarsFactory) {
        _cache = cache;
        _handlebarsFactory = handlebarsFactory;
    }

    public bool IsSyntaxValid(string content) {
        try {
            var handlebars = GetHandlebars(null);
        
            handlebars.Compile(content);

            return true;
        } catch {
            return false;
        }
    }

    public HandlebarsTemplate<object, object> Compile(string markup,
                                                      IReadOnlyDictionary<string, string> partials = null,
                                                      string cacheKey = null) {
        HandlebarsTemplate<object, object> compiled;

        if (cacheKey != null) {
            compiled = _cache.GetOrCreate(cacheKey, c => {
                var result = DoCompile(markup, partials);

                c.SlidingExpiration = TimeSpan.FromHours(1);

                return result;
            });
        } else {
            compiled = DoCompile(markup, partials);
        }

        return compiled;
    }

    private HandlebarsTemplate<object, object> DoCompile(string markup, IReadOnlyDictionary<string, string> partials) {
        var handlebars = GetHandlebars(partials);
        
        var compiledTemplate = handlebars.Compile(markup);

        return compiledTemplate;
    }

    private IHandlebars GetHandlebars(IReadOnlyDictionary<string, string> partials) {
        var handlebars = _handlebarsFactory.Create(partials);

        return handlebars;
    }
}
