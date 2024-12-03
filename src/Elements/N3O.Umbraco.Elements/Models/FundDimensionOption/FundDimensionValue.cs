using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Elements.Models;

public interface IFundDimensionValue : INamedLookup {
    bool IsUnrestricted { get; }
}

public abstract class FundDimensionValue<T> : LookupContent<T>, IFundDimensionValue where T : FundDimensionValue<T> {
    public bool IsUnrestricted => GetValue(x => x.IsUnrestricted);
}

public class FundDimension1Value : FundDimensionValue<FundDimension1Value> { }
public class FundDimension2Value : FundDimensionValue<FundDimension2Value> { }
public class FundDimension3Value : FundDimensionValue<FundDimension3Value> { }
public class FundDimension4Value : FundDimensionValue<FundDimension4Value> { }
