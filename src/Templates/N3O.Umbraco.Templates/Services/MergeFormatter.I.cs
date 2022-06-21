using System;

namespace N3O.Umbraco.Templates;

public interface IMergeFormatter {
    bool CanFormat(Type type);
    string Format(object value);
}
