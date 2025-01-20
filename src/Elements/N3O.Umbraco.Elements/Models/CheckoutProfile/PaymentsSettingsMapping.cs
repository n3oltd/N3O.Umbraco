using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Elements.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class PaymentsSettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PaymentMethodDataEntrySettingsContent, PaymentsSettings>((_, _) => new PaymentsSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PaymentMethodDataEntrySettingsContent src, PaymentsSettings dest, MapperContext ctx) {
        var paymentsSettings = new List<PaymentMethodSettings>();

        foreach (var (paymentMethod, index) in src.PaymentMethods.SelectWithIndex()) {
            var method = new PaymentMethodSettings();

            method.Name = paymentMethod.Name;
            method.Id = paymentMethod.Id.ToString();
            method.ProcessorId = paymentMethod.ProcessorId;
            method.Order = index + 1;
            method.AllowedCurrencies = paymentMethod.AllowedCurrencies.Select(x => ToCrmCurrency(x.Code)).ToList();
            method.SupportsRealtimePayments = paymentMethod.SupportsGooglePay;
            method.SupportsApplePay = paymentMethod.SupportsGooglePay;
            method.SupportsGooglePay = paymentMethod.SupportsGooglePay;
            
            paymentsSettings.Add(method);
        }
        
        dest.PaymentMethods = paymentsSettings;
    }

    private Currency ToCrmCurrency(string code) {
        return (Currency) Enum.Parse(typeof(Currency), code, true);
    }
}