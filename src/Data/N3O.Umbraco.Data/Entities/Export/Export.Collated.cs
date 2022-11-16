namespace N3O.Umbraco.Data.Entities;

public partial class Export {
    public void Collated(long processedCount) {
        Stage = DataConstants.Export.Stages.Collating;
        CollatedRecords = processedCount;
    }
}
