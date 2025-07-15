using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Linq;

namespace N3O.Umbraco.Giving.Allocations;

public class FundStructureAccessor : IFundStructureAccessor {
    private readonly ILookups _lookups;

    public FundStructureAccessor(ILookups lookups) {
        _lookups = lookups;
    }

    public FundStructure GetFundStructure() {
        var dimension1 = _lookups.GetAll<FundDimension1>().SingleOrDefault();
        var dimension2 = _lookups.GetAll<FundDimension2>().SingleOrDefault();
        var dimension3 = _lookups.GetAll<FundDimension3>().SingleOrDefault();
        var dimension4 = _lookups.GetAll<FundDimension4>().SingleOrDefault();
        
        return new FundStructure(dimension1, dimension2, dimension3, dimension4);
    }
}
