using N3O.Umbraco.Constants;
using N3O.Umbraco.Types;

namespace N3O.Umbraco.Localization;

public class CodeStrings : IStrings {
    public string Folder => TextFolders.Code;
    public string Name => TypeResolver.PersistedName(GetType().FullName);
}
