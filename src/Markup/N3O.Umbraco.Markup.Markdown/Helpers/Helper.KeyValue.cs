using N3O.Umbraco.Markup.Markdown.Utilities;
using System.Collections.Generic;

namespace N3O.Umbraco.Markup.Markdown.Helpers;

public abstract class KeyValueHelper<T, TArgs> : Helper<T, TArgs> where TArgs : HelperArgs, new() {
    protected KeyValueHelper(IEnumerable<string> keywords, int args) : this(keywords, args, args) { }

    protected KeyValueHelper(IEnumerable<string> keywords, int minArgs, int maxArgs)
        : base(keywords, minArgs * 2, maxArgs * 2) { }

    protected override IReadOnlyList<string> PreprocessArgs(IReadOnlyList<string> args) {
        args = KeyValueUtility.ProcessArgs(args);

        return base.PreprocessArgs(args);
    }

    protected override void Parse(IReadOnlyList<string> args, TArgs inline) {
        foreach (var (key, value) in KeyValueUtility.GetKeyValuePairs(args)) {
            Parse(key, value, inline);
        }
    }

    protected abstract void Parse(string key, string value, TArgs inline);
}
