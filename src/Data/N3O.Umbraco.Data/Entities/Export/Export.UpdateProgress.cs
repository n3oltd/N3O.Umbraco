namespace N3O.Umbraco.Data.Entities;

public partial class Export {
    public void UpdateProgress(long processed) {
        Processed = processed;
    }
}
