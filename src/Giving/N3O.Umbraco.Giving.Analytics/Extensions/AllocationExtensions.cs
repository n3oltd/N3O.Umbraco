using Humanizer;
using N3O.Umbraco.Giving.Analytics.Models;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Giving.Analytics.Extensions {
    public static class AllocationExtensions {
        public static Item ToItem(this Allocation allocation, GivingType givingType, int index) {
            var item = new Item();
            item.Id = allocation.Summary.Camelize();
            item.Name = allocation.Summary;
            item.Affiliation = "Website";
            item.Coupon = null;
            item.Currency = allocation.Value.Currency.Code.ToUpper();
            item.Discount = 0;
            item.Index = index;
            item.Brand = givingType.Name;
            item.Category = allocation.FundDimensions.Dimension1?.Name;
            item.Category2 = allocation.FundDimensions.Dimension2?.Name;
            item.Category3 = allocation.FundDimensions.Dimension3?.Name;
            item.Category4 = allocation.FundDimensions.Dimension4?.Name;
            item.Category5 = allocation.Type.Name;
            item.ListId = null;
            item.ListName = null;
            item.Variant = null;
            item.LocationId = null;
            item.Price = allocation.Value.Amount * givingType.ValueMultiplier;
            item.Quantity = 1;
            
            return item;
        }
    }
}