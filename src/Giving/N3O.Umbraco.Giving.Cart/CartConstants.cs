namespace N3O.Umbraco.Giving.Cart {
    public static class CartConstants {
        public const string ApiName = "Cart";
        public const string Cookie = "cartId";
        public const int MaxCartAllocations = 100;

        public static class QueryString {
            public const string CheckoutView = "r";
        }

        public static class Tables {
            public const string Carts = "N3O_Carts";
        }
    }
}
