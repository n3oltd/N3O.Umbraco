namespace N3O.Umbraco.Data.Entities;

public partial class Export {
    public void Complete(string storageFolderName, string filename) {
        StorageFolderName = storageFolderName;
        Filename = filename;
        Stage = DataConstants.Export.Stages.Complete;
        IsComplete = true;
    }
}
