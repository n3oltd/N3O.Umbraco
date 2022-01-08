namespace N3O.Umbraco.Counters {
    public interface IReference {
        ReferenceType Type { get; }
        long Number { get; }
        string Text { get; }
    }
}