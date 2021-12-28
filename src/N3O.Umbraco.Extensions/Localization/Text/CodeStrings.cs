using N3O.Umbraco.Constants;

namespace N3O.Umbraco.Localization {
    public class CodeStrings : IStrings {
        public string Folder => TextFolders.Code;
        public string Name => GetType().FullName;
    }
}