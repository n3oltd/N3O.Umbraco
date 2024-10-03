using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class AllocationStatisticsRes {
    public IEnumerable<AllocationStatisticsItemRes> TopItems { get; set; }
}