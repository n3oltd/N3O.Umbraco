using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Content;

public class MultipleTextPropertyBuilder : PropertyBuilder {
    public void Set(IEnumerable<string> values) {
        Set(values.OrEmpty().ToArray());
    }

    public void Set(params string[] values) {
        Value = string.Join(Environment.NewLine, values.OrEmpty().Where(x => x.HasValue()));
    }
}
