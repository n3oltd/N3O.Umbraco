namespace N3O.Umbraco.Content {
    public abstract class PropertyBuilder : IPropertyBuilder {
        public virtual object Build() {
            return Value;
        }
        
        protected object Value { get; set; }
    }
}