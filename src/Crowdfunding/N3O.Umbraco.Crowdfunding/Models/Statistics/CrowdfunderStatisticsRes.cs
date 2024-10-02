using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public abstract class CrowdfunderStatisticsRes {
    public int Count { get; set; }
    public decimal AveragePercentageComplete { get; set; }
    public IEnumerable<CrowdfunderStatisticsItemRes> TopItems { get; set; }
}