using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout {
    public interface ICheckoutReceipt {
        Task SendAsync();
    }
}