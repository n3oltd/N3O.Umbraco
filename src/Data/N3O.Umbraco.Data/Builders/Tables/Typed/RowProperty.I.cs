namespace N3O.Umbraco.Data.Builders {
    public interface IRowProperty<in TRow> {
        void AddValues(TRow record);
        void CreateColumnRange();
    }
}