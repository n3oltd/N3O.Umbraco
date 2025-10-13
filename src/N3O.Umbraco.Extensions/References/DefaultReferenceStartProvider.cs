namespace N3O.Umbraco.References;

public class DefaultReferenceStartProvider : IReferenceStartProvider{
    public bool CanProvideReference(ReferenceType type) {
        return true;
    }
    public long GetStartNumber(ReferenceType type) {
        return type.StartFrom;
    }
}