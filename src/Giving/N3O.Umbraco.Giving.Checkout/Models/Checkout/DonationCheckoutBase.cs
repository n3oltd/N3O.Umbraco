// using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
// using System;
//
// namespace N3O.Umbraco.Giving.Checkout.Models {
//     public abstract class DonationCheckoutBase {
//         public LiteCart Cart { get; }
//         public string Reference { get; }
//         public object PaymentData { get; }
//
//         [JsonIgnore]
//         public Money Value => Cart.Total;
//
//         [JsonIgnore]
//         public abstract Money TotalIncome { get; }
//
//         public void SetPaymentData<T>(Action<T> action) where T : new() {
//             if (PaymentData == null) {
//                 PaymentData = new T();
//             }
//
//             if (PaymentData is JObject jObject) {
//                 PaymentData = jObject.ToObject<T>();
//             }
//
//             action.Invoke((T) PaymentData);
//         }
//     }
// }