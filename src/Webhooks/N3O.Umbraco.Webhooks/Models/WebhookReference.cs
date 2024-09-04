using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.References;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Webhooks.Models;

public class WebhookReference : Value {
    public WebhookReference(string prefix, long number, string text) {
        Prefix = prefix;
        Number = number;
        Text = text;
    }

    public string Prefix { get; }
    public long Number { get; }
    public string Text { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Prefix;
        yield return Number;
        yield return Text;
    }

    public Reference ToReference() {
        var type = StaticLookups.GetAll<ReferenceType>().Single(x => x.Prefix.EqualsInvariant(Prefix));

        return type.ToReference(Text);
    }
}