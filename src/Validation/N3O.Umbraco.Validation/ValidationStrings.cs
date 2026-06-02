using N3O.Umbraco.Constants;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Types;

namespace N3O.Umbraco.Validation;

public class ValidationStrings : IStrings {
    public string Folder => TextFolders.Validation;
    public string Name => TypeResolver.PersistedName(GetType().FullName);
}
