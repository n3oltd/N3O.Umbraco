using N3O.Umbraco.Extensions;
using System;
using System.Linq;

namespace N3O.Umbraco.Utilities;

public class HtmlIds {
    public string GenerateId(params object[] contextValues) {
        if (!contextValues.HasAny(x => x != null)) {
            throw new Exception("At least one non-null context value must be specified");
        }

        var context = string.Join("þ", contextValues.Select(x => x ?? "[null]"));

        return $"id_{context.Sha1().Substring(0, 6)}";
    }
}