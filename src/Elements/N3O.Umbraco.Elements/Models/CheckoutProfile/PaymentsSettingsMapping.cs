﻿using N3O.Umbraco.Elements.Clients;
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

        foreach (var (paymentMethod, index) in src.PaymentMethods.OrEmpty().SelectWithIndex()) {
            var method = new PaymentMethodSettings();

            method.Name = paymentMethod.Name;
            method.DisplayName = paymentMethod.DisplayName;
            method.Description = paymentMethod.Description;
            method.IconId = paymentMethod.IconId;
            method.Id = paymentMethod.Id.ToString();
            method.ProcessorId = paymentMethod.ProcessorId;
            method.Order = index + 1;
            method.AllowedCurrencies = paymentMethod.AllowedCurrencies.Select(x => GetCurrency(x.Code)).ToList();
            method.SupportsApplePay = paymentMethod.SupportsApplePay;
            method.SupportsGooglePay = paymentMethod.SupportsGooglePay;
            method.SupportsRealtimePayments = paymentMethod.SupportsRealtimePayments;
            method.CollectionDaysOfMonth = paymentMethod.CollectionDaysOfMonth.Select(ctx.Map<FlowPaymentMethodCollectionDayOfMonth, PaymentMethodCollectionDayOfMonth>).ToList();
            method.CollectionDaysOfWeek = paymentMethod.CollectionDaysOfWeek.Select(ctx.Map<FlowPaymentMethodCollectionDayOfWeek, PaymentMethodCollectionDayOfWeek>).ToList();
            method.AdditionalData = paymentMethod.AdditionalData;
            
            paymentsSettings.Add(method);
        }
        
        dest.PaymentMethods = paymentsSettings;
    }

    private Currency GetCurrency(string code) {
        return (Currency) Enum.Parse(typeof(Currency), code, true);
    }
}