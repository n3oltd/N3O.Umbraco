using Markdig.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Markup.Markdown.Helpers;

public abstract class BlockHelper<T> : BlockHelper<T, EmptyHelperArgs> {
    protected BlockHelper(IEnumerable<string> keywords) : base(keywords, 0, 0) { }

    protected override void ParseOpening(IReadOnlyList<string> args, EmptyHelperArgs inline) { }
}

public abstract class BlockHelper<T, TArgs> : Helper<T, TArgs> where TArgs : HelperArgs, new() {
    private readonly int _minArgs;
    private readonly int _maxArgs;
    
    protected BlockHelper(IEnumerable<string> keywords, int args) : this(keywords, args, args) { }

    protected BlockHelper(IEnumerable<string> keywords, int minArgs, int maxArgs)
        : base(keywords.Concat(keywords.Select(x => $"/{x}")), 0, int.MaxValue) {
        _minArgs = minArgs;
        _maxArgs = maxArgs;
    }

    protected override void Parse(IReadOnlyList<string> args, TArgs inline) {
        if (!args[0].StartsWith("/")) {
            var argsCount = args.Count - 1;
            
            if (argsCount < _minArgs || argsCount > _maxArgs) {
                throw new Exception("Incorrect number of parameters passed");
            }
            
            ParseOpening(args, inline);
        }
    }

    protected abstract void ParseOpening(IReadOnlyList<string> args, TArgs inline);

    protected override void Render(HtmlRenderer renderer, TArgs inline) {
        if (inline.Keyword.StartsWith("/")) {
            RenderClosing(renderer);
        } else {
            RenderOpening(renderer, inline);
        }
    }

    protected abstract void RenderOpening(HtmlRenderer renderer, TArgs inline);
    protected abstract void RenderClosing(HtmlRenderer renderer);
}
