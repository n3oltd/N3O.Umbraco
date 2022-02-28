using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Blocks;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Content;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.TaxRelief.Content;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Checkout.Modules {
    public class CheckoutAccountBlockModule : IBlockModule {
        private static readonly string BlockAlias = AliasHelper<CheckoutAccountBlockContent>.ContentTypeAlias();
        
        private readonly IContentCache _contentCache;
        private readonly ILookups _lookups;

        public CheckoutAccountBlockModule(IContentCache contentCache, ILookups lookups) {
            _contentCache = contentCache;
            _lookups = lookups;
        }
        
        public bool ShouldExecute(IPublishedElement block) {
            return block.ContentType.Alias.EqualsInvariant(BlockAlias);
        }

        public Task<object> ExecuteAsync(IPublishedElement block, CancellationToken cancellationToken) {
            var dataEntrySettingsContent = _contentCache.Single<DataEntrySettingsContent>();
            var taxReliefSettingsContent = _contentCache.Single<TaxReliefSettingsContent>();
            var consentOptionsContent = _contentCache.All<ConsentOptionContent>();

            var dataEntrySettings = dataEntrySettingsContent.ToDataEntrySettings(_lookups, consentOptionsContent);

            return Task.FromResult<object>(new CheckoutAccountModel(dataEntrySettings, taxReliefSettingsContent.Scheme));
        }
        
        public string Key => CheckoutConstants.BlockModuleKeys.CheckoutAccount;
    }
}