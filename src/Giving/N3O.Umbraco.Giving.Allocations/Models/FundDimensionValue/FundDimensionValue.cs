using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IFundDimensionValue : IContentOrPublishedLookup {
    bool IsUnrestricted { get; }
}

public abstract class FundDimensionValue<T> : ContentOrPublishedLookup, IFundDimensionValue where T : FundDimensionValue<T> {
    protected FundDimensionValue(string id, string name, Guid? contentId, bool isUnrestricted) 
        : base(id, name, contentId) {
        IsUnrestricted = isUnrestricted;
    }

    public bool IsUnrestricted { get; }
}

public class FundDimension1Value : FundDimensionValue<FundDimension1Value> {
    public FundDimension1Value(string id, string name, Guid? contentId, bool isUnrestricted) 
        : base(id, name, contentId, isUnrestricted) { }
}

public class FundDimension2Value : FundDimensionValue<FundDimension2Value> {
    public FundDimension2Value(string id, string name, Guid? contentId, bool isUnrestricted) 
        : base(id, name, contentId, isUnrestricted) { }
}

public class FundDimension3Value : FundDimensionValue<FundDimension3Value> {
    public FundDimension3Value(string id, string name, Guid? contentId, bool isUnrestricted) 
        : base(id, name, contentId, isUnrestricted) { }
}

public class FundDimension4Value : FundDimensionValue<FundDimension4Value> {
    public FundDimension4Value(string id, string name, Guid? contentId, bool isUnrestricted) 
        : base(id, name, contentId, isUnrestricted) { }
}