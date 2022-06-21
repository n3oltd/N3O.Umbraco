using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Data.Konstrukt;

public partial class Import {
    public string GetStorageFolderName(IJsonProvider jsonProvider) {
        return jsonProvider.DeserializeObject<ParserSettings>(ParserSettings).StorageFolderName;
    }
}
