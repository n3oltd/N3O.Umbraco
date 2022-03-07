using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Checkout.Content;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Lookups;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Checkout.Modules {
    public class CheckoutRegularGivingBlockModule : CheckoutPaymentBlockModule {
        private static readonly string BlockAlias = AliasHelper<CheckoutRegularGivingBlockContent>.ContentTypeAlias();

        public CheckoutRegularGivingBlockModule(ICheckoutAccessor checkoutAccessor,
                                                IContentCache contentCache,
                                                ILookups lookups,
                                                IServiceProvider serviceProvider)
            : base(checkoutAccessor, contentCache, lookups, serviceProvider, BlockAlias) { }

        protected override IEnumerable<IPublishedContent> GetAllowedMethodSettings(GivingSettingsContent givingSettings) {
            return givingSettings.RegularGivingPaymentMethods;
        }

        protected override object GetModel(Entities.Checkout checkout,
                                           IReadOnlyDictionary<PaymentMethod, object> paymentMethods) {
            return new CheckoutRegularGivingModel(checkout, paymentMethods);
        }

        public override string Key => CheckoutConstants.BlockModuleKeys.CheckoutRegularGiving;
        protected override PaymentObjectType PaymentObjectType => PaymentObjectTypes.Credential;
    }
}