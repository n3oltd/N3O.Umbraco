using Newtonsoft.Json;
using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookNewCustomField : Value {
    [JsonConstructor]
    public WebhookNewCustomField(string alias, bool? @bool, LocalDate? date, string text) {
        Alias = alias;
        Bool = @bool;
        Date = date;
        Text = text;
    }

    public string Alias { get; }
    public bool? Bool { get; }
    public LocalDate? Date { get; }
    public string Text { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Alias;
        yield return Bool;
        yield return Date;
        yield return Text;
    }
}