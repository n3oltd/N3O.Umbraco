namespace N3O.Umbraco.Giving.Cart {
    public static class CartConstants {
        public const string ApiName = "Cart";
        public const string Cookie = "cartId";
        public const int MaxCartAllocations = 100;

        public static class BlockModuleKeys {
            public const string Cart = nameof(Cart);
        }
        
        public static class QueryString {
            public const string CheckoutView = "r";
        }
    }
}
