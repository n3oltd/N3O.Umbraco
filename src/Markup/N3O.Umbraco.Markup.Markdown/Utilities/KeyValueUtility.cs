using Flurl.Util;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Markup.Markdown.Utilities;

public static class KeyValueUtility {
    public static IEnumerable<(string, string)> GetKeyValuePairs(IReadOnlyList<string> args) {
        foreach (var batch in args.Skip(1).Batch(2)) {
            if (batch.Count() != 3 || batch.ElementAt(1) != "=") {
                yield return (batch.First(), batch.Last());
            }
        }
    }
    
    public static IReadOnlyList<string> ProcessArgs(IReadOnlyList<string> args) {
        var newArgs = new List<string>();

        newArgs.Add(args[0]);

        for (var i = 1; i < args.Count; i++) {
            var currArg = args.ElementAt(i);
            var nextArg = args.ElementAtOrDefault(i + 1);

            if (nextArg == "=") {
                newArgs.Add(currArg);
                newArgs.Add(args.ElementAtOrDefault(i + 2));
                i += 2;
            } else {
                newArgs.AddRange(currArg.SplitOnFirstOccurence("=").Select(x => x.Replace("\"", ""))); ;
            }
        }

        return newArgs;
    }
}
