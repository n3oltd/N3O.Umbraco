using N3O.Umbraco.Markup.Markdown.Utilities;
using System.Collections.Generic;

namespace N3O.Umbraco.Markup.Markdown.Helpers {
    public abstract class KeyValueBlockHelper<T> : BlockHelper<T> where T : HelperArgs, new() {
        protected KeyValueBlockHelper(IEnumerable<string> keywords, int args) : this(keywords, args, args) { }
        
        protected KeyValueBlockHelper(IEnumerable<string> keywords, int minArgs, int maxArgs)
            : base(keywords, minArgs * 2, maxArgs * 2) { }

        protected override IReadOnlyList<string> PreprocessArgs(IReadOnlyList<string> args) {
            args = KeyValueUtility.ProcessArgs(args);

            return base.PreprocessArgs(args);
        }

        protected override void ParseOpening(IReadOnlyList<string> args, T inline) {
            foreach (var (key, value) in KeyValueUtility.GetKeyValuePairs(args)) {
                ParseOpening(key, value, inline);
            }
        }

        protected abstract void ParseOpening(string key, string value, T inline);
    }
}