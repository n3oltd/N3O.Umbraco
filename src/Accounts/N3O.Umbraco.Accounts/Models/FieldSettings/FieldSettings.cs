namespace N3O.Umbraco.Accounts.Models {
    public abstract class FieldSettings : Value {
        protected FieldSettings(bool visible,
                                bool required,
                                string label,
                                string helpText,
                                int order = -1,
                                bool validate = false) {
            Required = required;
            Label = label;
            HelpText = helpText;
            Order = order;
            Visible = visible;
            Validate = validate;
        }
        
        public bool Required { get; }
        public string Label { get; }
        public string HelpText { get; }
        public int Order { get; }
        public bool Visible { get; }
        public bool Validate { get; }
        
        public abstract string Type { get; }
    }
}