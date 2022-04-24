namespace N3O.Umbraco.Data.Builders {
    public interface IRowProperty<in TRow> {
        void AddCells(TRow record);
        void CreateColumnRange();
    }
}