using N3O.Umbraco.Markup.Markdown.Utilities;
using System.Collections.Generic;

namespace N3O.Umbraco.Markup.Markdown.Helpers;

public abstract class KeyValueBlockHelper<T, TArgs> : BlockHelper<T, TArgs> where TArgs : HelperArgs, new() {
    protected KeyValueBlockHelper(IEnumerable<string> keywords, int args) : this(keywords, args, args) { }
    
    protected KeyValueBlockHelper(IEnumerable<string> keywords, int minArgs, int maxArgs)
        : base(keywords, minArgs * 2, maxArgs * 2) { }

    protected override IReadOnlyList<string> PreprocessArgs(IReadOnlyList<string> args) {
        args = KeyValueUtility.ProcessArgs(args);

        return base.PreprocessArgs(args);
    }

    protected override void ParseOpening(IReadOnlyList<string> args, TArgs helperArgs) {
        foreach (var (key, value) in KeyValueUtility.GetKeyValuePairs(args)) {
            ParseOpening(key, value, helperArgs);
        }
    }

    protected abstract void ParseOpening(string key, string value, TArgs helperArgs);
}
