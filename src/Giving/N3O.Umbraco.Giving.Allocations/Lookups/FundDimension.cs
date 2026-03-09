using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public interface IFundDimension : INamedLookup {
    bool IsActive { get; }
    IEnumerable<IFundDimensionValue> Options { get; }
    int Index { get; }
}

public abstract class FundDimension<T, TValue> : ContentOrPublishedLookup, IFundDimension
    where T : FundDimension<T, TValue>
    where TValue : IFundDimensionValue {
    protected FundDimension(string id,
                            string name,
                            Guid? contentId,
                            bool isActive,
                            IEnumerable<IFundDimensionValue> options,
                            int index)
        : base(id, name, contentId) {
        IsActive = isActive;
        Options = options;
        Index = index;
    }

    public bool IsActive { get; }
    public IEnumerable<IFundDimensionValue> Options { get; }
    public int Index { get; }
}

public class FundDimension1 : FundDimension<FundDimension1, FundDimension1Value> {
    public FundDimension1(string id,
                          string name,
                          Guid? contentId,
                          bool isActive,
                          IReadOnlyList<IFundDimensionValue> options) 
        : base(id, name, contentId, isActive, options, 1) { }
}

public class FundDimension2 : FundDimension<FundDimension2, FundDimension2Value> {
    public FundDimension2(string id,
                          string name,
                          Guid? contentId,
                          bool isActive,
                          IReadOnlyList<IFundDimensionValue> options) 
        : base(id, name, contentId, isActive, options, 2) { }
}

public class FundDimension3 : FundDimension<FundDimension3, FundDimension3Value> {
    public FundDimension3(string id,
                          string name,
                          Guid? contentId,
                          bool isActive,
                          IReadOnlyList<IFundDimensionValue> options) 
        : base(id, name, contentId, isActive, options, 3) { }
}

public class FundDimension4 : FundDimension<FundDimension4, FundDimension4Value> {
    public FundDimension4(string id,
                          string name,
                          Guid? contentId,
                          bool isActive,
                          IReadOnlyList<IFundDimensionValue> options) 
        : base(id, name, contentId, isActive, options, 4) { }
}