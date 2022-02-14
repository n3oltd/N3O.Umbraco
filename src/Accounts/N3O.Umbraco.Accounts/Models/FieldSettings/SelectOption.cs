namespace N3O.Umbraco.Accounts.Models {
    public class SelectOption {
        public SelectOption(string value, string text) {
            Value = value;
            Text = text;
        }

        public string Value { get; }
        public string Text { get; }
    }
}