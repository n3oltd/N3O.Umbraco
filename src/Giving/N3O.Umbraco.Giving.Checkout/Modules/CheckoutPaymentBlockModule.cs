using N3O.Umbraco.Blocks;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Content;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments;
using N3O.Umbraco.Payments.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Checkout.Modules {
    public abstract class CheckoutPaymentBlockModule : IBlockModule {
        private readonly ICheckoutAccessor _checkoutAccessor;
        private readonly IContentCache _contentCache;
        private readonly ILookups _lookups;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _blockAlias;

        protected CheckoutPaymentBlockModule(ICheckoutAccessor checkoutAccessor,
                                             IContentCache contentCache,
                                             ILookups lookups,
                                             IServiceProvider serviceProvider,
                                             string blockAlias) {
            _checkoutAccessor = checkoutAccessor;
            _contentCache = contentCache;
            _lookups = lookups;
            _serviceProvider = serviceProvider;
            _blockAlias = blockAlias;
        }
        
        public bool ShouldExecute(IPublishedElement block) {
            return block.ContentType.Alias.EqualsInvariant(_blockAlias);
        }

        public Task<object> ExecuteAsync(IPublishedElement block, CancellationToken cancellationToken) {
            var checkout = _checkoutAccessor.Get();
            var givingSettings = _contentCache.Single<GivingSettingsContent>();
            var allowedMethodSettings = GetAllowedMethodSettings(givingSettings);

            bool IsAllowed(PaymentMethod paymentMethod) {
                var settingsContentTypeAlias = paymentMethod.GetSettingsContentTypeAlias();

                return allowedMethodSettings.Any(x => x.ContentType.Alias.EqualsInvariant(settingsContentTypeAlias));
            }

            var paymentMethods = _lookups.GetAll<PaymentMethod>()
                                         .Where(x => x.IsAvailable(checkout.Account.Address.Country,
                                                                   checkout.Currency) &&
                                                     x.Supports(PaymentObjectType) &&
                                                     IsAllowed(x))
                                         .ToDictionary(x => x, GetViewModel);

            return Task.FromResult<object>(new CheckoutDonationModel(checkout, paymentMethods));
        }

        protected abstract IEnumerable<IPublishedContent> GetAllowedMethodSettings(GivingSettingsContent givingSettings);

        private object GetViewModel(PaymentMethod paymentMethod) {
            var viewModelType = typeof(IPaymentMethodViewModel<>).MakeGenericType(paymentMethod.GetType());

            return _serviceProvider.GetService(viewModelType);
        }

        public abstract string Key { get; }
        protected abstract PaymentObjectType PaymentObjectType { get; }
    }
}