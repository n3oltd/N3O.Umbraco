namespace N3O.Umbraco.Lookups {
    public class CapitalisationDataSource : LookupsDataSource<Capitalisation> {
        public CapitalisationDataSource(ILookups lookups) : base(lookups) { }
        
        public override string Name => "Capitalisations";
        public override string Description => "Data source for capitalisations";
        public override string Icon => "icon-autofill";

        protected override string GetIcon(Capitalisation capitalisation) => "icon-autofill";
    }
}