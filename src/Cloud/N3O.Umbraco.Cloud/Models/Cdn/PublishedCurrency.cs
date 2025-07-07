using System;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedCurrency {
    public string Code { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }
    public Uri Icon { get; set; }
    public int DecimalDigits { get; set; }
    public decimal Rate { get; set; }
    public bool IsBaseCurrency { get; set; }
}