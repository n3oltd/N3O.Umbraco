using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedCurrencies : Value {
    [JsonProperty(PropertyName = "currencies")]
    public Dictionary<string, PublishedCurrency> Items { get; set; }

    [JsonIgnore]
    public IEnumerable<PublishedCurrency> Currencies => Items.Values;

    public PublishedCurrency BaseCurrency => Currencies.Single(x => x.IsBaseCurrency);

    public PublishedCurrency FindByCode(string code, bool throwIfNotFound) {
        var publishedCurrency = Currencies.SingleOrDefault(x => x.Code.EqualsInvariant(code));
        
        if (throwIfNotFound && publishedCurrency == null) {
            throw new Exception($"Could not find published currency with code {code}");
        }

        return publishedCurrency;
    }
    
    public PublishedCurrency FindByCurrency(Currency currency, bool throwIfNotFound = false) {
        return FindByCode(currency.Code, throwIfNotFound);
    }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Items?.Keys;
        yield return Items?.Values;
    }
}