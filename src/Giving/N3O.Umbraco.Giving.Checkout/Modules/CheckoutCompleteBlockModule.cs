using N3O.Umbraco.Blocks;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Content;
using N3O.Umbraco.Giving.Checkout.Models;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Checkout.Modules {
    public class CheckoutCompleteBlockModule : IBlockModule {
        private static readonly string BlockAlias = AliasHelper<CheckoutCompleteBlockContent>.ContentTypeAlias();
        
        private readonly ICheckoutAccessor _checkoutAccessor;

        public CheckoutCompleteBlockModule(ICheckoutAccessor checkoutAccessor) {
            _checkoutAccessor = checkoutAccessor;
        }
        
        public bool ShouldExecute(IPublishedElement block) {
            return block.ContentType.Alias.EqualsInvariant(BlockAlias);
        }

        public async Task<object> ExecuteAsync(IPublishedElement block, CancellationToken cancellationToken) {
            var checkout = await _checkoutAccessor.GetAsync(cancellationToken);

            return Task.FromResult<object>(new CheckoutCompleteModel(checkout));
        }
        
        public string Key => CheckoutConstants.BlockModuleKeys.CheckoutComplete;
    }
}