using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Content;

public class PaymentMethodDataEntrySettingsContent : UmbracoContent<PaymentMethodDataEntrySettingsContent> {
    public IEnumerable<FlowPaymentMethod> PaymentMethods => GetValue(x => x.PaymentMethods);
}