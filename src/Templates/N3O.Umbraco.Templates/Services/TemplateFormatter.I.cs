using System;

namespace N3O.Umbraco.Templates;

public interface ITemplateFormatter {
    bool CanFormat(Type type);
    string Format(object value);
}
