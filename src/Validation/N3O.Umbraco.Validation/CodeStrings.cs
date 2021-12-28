using N3O.Umbraco.Constants;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Validation;

public class ValidationStrings : IStrings {
    public string Folder => TextFolders.Code;
    public string Name => GetType().FullName;
}