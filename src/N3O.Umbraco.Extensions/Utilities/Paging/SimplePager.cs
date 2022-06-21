using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Utilities;

public class SimplePager<T> : DynamicPager<T> {
    public SimplePager(Uri currentUrl,
                       IEnumerable<T> results,
                       int pageSize,
                       int? firstPageSize = null) :
        base(currentUrl,
             (start, num) => results.Skip(start).Take(num),
             results.Count(),
             pageSize,
             firstPageSize) {
    }
}
