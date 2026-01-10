namespace N3O.Umbraco.Cloud.Zakat.Templates.Models;

public class NisabMergeModel {
    public NisabMergeModel(string gold, string silver) {
        Gold = gold;
        Silver = silver;
    }

    public string Gold { get; }
    public string Silver { get; }
}