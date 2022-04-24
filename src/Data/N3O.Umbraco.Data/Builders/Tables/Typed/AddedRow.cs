namespace N3O.Umbraco.Data.Builders {
    public class AddedRow<TRow> {
        public AddedRow(int number, TRow row) {
            Number = number;
            Row = row;
        }

        public int Number { get; }
        public TRow Row { get; }
    }
}