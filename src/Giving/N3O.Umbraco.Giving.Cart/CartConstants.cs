namespace N3O.Umbraco.Giving.Cart;

public static class CartConstants {
    public const string ApiName = "Cart";

    public static class BlockModuleKeys {
        public static readonly string Cart = nameof(Cart);
    }
    
    public static class QueryString {
        public static readonly string CheckoutView = "r";
    }
}
