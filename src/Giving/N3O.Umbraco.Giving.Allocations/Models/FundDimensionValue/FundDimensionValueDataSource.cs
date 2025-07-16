using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models;

public abstract class FundDimensionValueDataSource<T> : LookupsDataSource<T> where T : ContentOrPublishedLookup {
    public FundDimensionValueDataSource(ILookups lookups, int index) : base(lookups) {
        Index = index;
    }

    private int Index { get; }
    
    public override string Name => $"Fund Dimension {Index} Values";
    public override string Description => $"Data source for fund dimension {Index} values";
    public override string Icon => "icon-checkbox";

    protected override string GetIcon(T lookup) => "icon-checkbox";
}

public class FundDimensionValue1DataSource : FundDimensionValueDataSource<FundDimension1Value> {
    public FundDimensionValue1DataSource(ILookups lookups) : base(lookups, 1) { }
}

public class FundDimensionValue2DataSource : FundDimensionValueDataSource<FundDimension2Value> {
    public FundDimensionValue2DataSource(ILookups lookups) : base(lookups, 2) { }
}

public class FundDimensionValue3DataSource : FundDimensionValueDataSource<FundDimension3Value> {
    public FundDimensionValue3DataSource(ILookups lookups) : base(lookups, 3) { }
}

public class FundDimensionValue4DataSource : FundDimensionValueDataSource<FundDimension4Value> {
    public FundDimensionValue4DataSource(ILookups lookups) : base(lookups, 4) { }
}