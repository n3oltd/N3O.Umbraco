using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Donations.Models {
    public class FixedOrDefaultFundDimensionOptionRes {
        public FundDimensionOptionRes Fixed { get; set; }
        public FundDimensionOptionRes Default { get; set; }
        
        public static FixedOrDefaultFundDimensionOptionRes For(FundDimensionOptionRes @fixed,
                                                               IEnumerable<FundDimensionOptionRes> options) {
            var res = new FixedOrDefaultFundDimensionOptionRes();
            res.Fixed = @fixed;
            res.Default  = @fixed.HasValue() ? null : options.FirstOrDefault(x => x.IsUnrestricted);

            return res;
        }
    }
}
