namespace N3O.Umbraco.References;

public interface IReferenceStartProvider {
    bool CanProvideReference(ReferenceType type);
    
    long GetStartNumber(ReferenceType type);
}