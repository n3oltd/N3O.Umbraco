using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedCurrency : Value {
    public string Code { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }
    public Uri Icon { get; set; }
    public int DecimalDigits { get; set; }
    public decimal Rate { get; set; }
    public bool IsBaseCurrency { get; set; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Code;
        yield return Symbol;
        yield return Name;
        yield return Icon;
        yield return DecimalDigits;
        yield return Rate;
        yield return IsBaseCurrency;
    }
}