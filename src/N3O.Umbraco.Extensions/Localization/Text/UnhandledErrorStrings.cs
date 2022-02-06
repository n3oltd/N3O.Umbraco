using N3O.Umbraco.Constants;

namespace N3O.Umbraco.Localization {
    public class UnhandledErrorStrings : IStrings {
        public string Message_1 => "An unexpected error occurred. Reference: {0}";
        
        public string Folder => TextFolders.Code;
        public string Name => "UnhandledErrors";
    }
}