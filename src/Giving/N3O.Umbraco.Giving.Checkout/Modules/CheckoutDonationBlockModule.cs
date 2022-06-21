using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Checkout.Content;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Lookups;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Checkout.Modules;

public class CheckoutDonationBlockModule : CheckoutPaymentBlockModule {
    private static readonly string BlockAlias = AliasHelper<CheckoutDonationBlockContent>.ContentTypeAlias();

    public CheckoutDonationBlockModule(ICheckoutAccessor checkoutAccessor,
                                       IContentCache contentCache,
                                       ILookups lookups,
                                       IServiceProvider serviceProvider)
        : base(checkoutAccessor, contentCache, lookups, serviceProvider, BlockAlias) { }

    protected override IEnumerable<IPublishedContent> GetAllowedMethodSettings(GivingSettingsContent givingSettings) {
        return givingSettings.DonationPaymentMethods;
    }

    protected override object GetModel(Entities.Checkout checkout,
                                       IReadOnlyDictionary<PaymentMethod, object> paymentMethods) {
        return new CheckoutDonationModel(checkout, paymentMethods);
    }
    
    public override string Key => CheckoutConstants.BlockModuleKeys.CheckoutDonation;
    protected override PaymentObjectType PaymentObjectType => PaymentObjectTypes.Payment;
}
