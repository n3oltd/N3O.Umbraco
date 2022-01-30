namespace N3O.Umbraco.References {
    public interface IReference {
        ReferenceType Type { get; }
        long Number { get; }
        string Text { get; }
    }
}