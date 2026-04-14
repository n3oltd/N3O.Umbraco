using System.Collections.Generic;

namespace N3O.Umbraco.Localization;

public class InvariantStringLocalizer : IStringLocalizer {
    public void Flush(IEnumerable<string> aliases) { }
    
    public string Get(string folderName, string name, string text) => text;
}
