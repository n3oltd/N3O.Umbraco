using HandlebarsDotNet;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace N3O.Umbraco.Templates.Handlebars;

public class HandlebarsCompiler : IHandlebarsCompiler {
    private readonly IMemoryCache _cache;
    private readonly IHandlebarsFactory _handlebarsFactory;
    private IHandlebars _handlebars;

    public HandlebarsCompiler(IMemoryCache cache, IHandlebarsFactory handlebarsFactory) {
        _cache = cache;
        _handlebarsFactory = handlebarsFactory;
    }

    public bool IsSyntaxValid(string content) {
        try {
            var handlebars = GetHandlebars();
        
            handlebars.Compile(content);

            return true;
        } catch {
            return false;
        }
    }

    public HandlebarsTemplate<object, object> Compile(string markup, string cacheKey = null) {
        HandlebarsTemplate<object, object> compiled;

        if (cacheKey != null) {
            compiled = _cache.GetOrCreate(cacheKey, c => {
                var result = DoCompile(markup);

                c.SlidingExpiration = TimeSpan.FromHours(1);

                return result;
            });
        } else {
            compiled = DoCompile(markup);
        }

        return compiled;
    }

    private HandlebarsTemplate<object, object> DoCompile(string markup) {
        var handlebars = GetHandlebars();
    
        var compiledTemplate = handlebars.Compile(markup);

        return compiledTemplate;
    }

    private IHandlebars GetHandlebars() {
        if (_handlebars == null) {
            _handlebars = _handlebarsFactory.Create();
        }

        return _handlebars;
    }
}
