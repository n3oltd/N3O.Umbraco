// using N3O.Umbraco.Giving.Checkout.Database;
// using N3O.Umbraco.Giving.Checkout.Models;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Umbraco.Cms.Core.Scoping;
// using ComposeEmail = FluentEmail.Core.Email;
//
// namespace N3O.Umbraco.Giving.Checkout {
//     public class Checkout : ICheckout {
//         private readonly ICart _cart;
//         private readonly ILocalClock _localClock;
//         private readonly IFormatter _formatter;
//         private readonly ILookups _lookups;
//         private readonly IScopeProvider _scopeProvider;
//         private readonly IHttpRequestAccessor _httpRequestAccessor;
//         private readonly IJsonProvider _jsonProvider;
//         private readonly ISender _emailSender;
//         private readonly IContentLocator _contentLocator;
//         private readonly CheckoutState _state = new CheckoutState();
//         private readonly CheckoutSchema _checkoutSchema;
//
//         public Checkout(ICart cart,
//                         ILocalClock localClock,
//                         IFormatter formatter,
//                         ILookups lookups,
//                         IScopeProvider scopeProvider,
//                         IHttpRequestAccessor httpRequestAccessor,
//                         IJsonProvider jsonProvider,
//                         ISender emailSender,
//                         IContentLocator contentLocator) {
//             _cart = cart;
//             _localClock = localClock;
//             _formatter = formatter;
//             _lookups = lookups;
//             _scopeProvider = scopeProvider;
//             _httpRequestAccessor = httpRequestAccessor;
//             _jsonProvider = jsonProvider;
//             _emailSender = emailSender;
//             _contentLocator = contentLocator;
//
//             using (var scope = scopeProvider.CreateScope()) {
//                 _checkoutSchema = scope.Database
//                                        .Query<CheckoutSchema>($@"SELECT * FROM {Constants.Database.Tables.Checkouts}
//                                                              WHERE {nameof(CheckoutSchema.CartId)} = @0",
//                                                               _cart.Id)
//                                        .SingleOrDefault();
//             }
//
//             if (_checkoutSchema != null) {
//                 _state = _jsonProvider.DeserializeObject<CheckoutState>(_checkoutSchema.State);
//             } else {
//                 _checkoutSchema = new CheckoutSchema();
//
//                 Start();
//             }
//         }
//
//         public Guid CartId {
//             get => _state.CartId;
//
//             private set => _state.CartId = value;
//         }
//
//         public Account Account {
//             get => _state.Account;
//
//             set => _state.Account = value;
//         }
//
//         public string Reference {
//             get => _state.Reference;
//
//             set => _state.Reference = value;
//         }
//
//         public CheckoutStage CurrentStage {
//             get => _state.CurrentStage;
//
//             private set => _state.CurrentStage = value;
//         }
//
//         public IReadOnlyList<CheckoutStage> RequiredStages {
//             get => _state.RequiredStages;
//
//             private set => _state.RequiredStages = value.ToList();
//         }
//
//         public IReadOnlyList<CheckoutStage> CompletedStages {
//             get => _state.CompletedStages;
//
//             private set => _state.CompletedStages = value.ToList();
//         }
//
//         public Currency Currency {
//             get => _lookups.FindById<ICurrency>(_state.CurrencyId);
//
//             private set => _state.CurrencyId = value.LookupId;
//         }
//
//         public RegularDonationCheckout RegularDonation {
//             get => _state.RegularDonation;
//
//             private set => _state.RegularDonation = value;
//         }
//
//         public SingleDonationCheckout SingleDonation {
//             get => _state.SingleDonation;
//
//             private set => _state.SingleDonation = value;
//         }
//
//         public bool IsRequired(CheckoutStage stage) => _state.RequiredStages.Contains(stage);
//
//         public bool IsCompleted(CheckoutStage stage) => _state.CompletedStages.Contains(stage);
//
//         public CheckoutStage MoveToNextStage() {
//             var nextRequiredIndex = Math.Min(RequiredStages.ToList()
//                                                            .IndexOf(CurrentStage) + 1,
//                                              RequiredStages.Count - 1);
//
//             if (!CompletedStages.Contains(CurrentStage)) {
//                 var newList = CompletedStages.ToList();
//                 newList.Add(CurrentStage);
//
//                 CompletedStages = newList;
//             }
//
//             CurrentStage = RequiredStages[nextRequiredIndex];
//
//             if (CurrentStage == CheckoutStages.Complete) {
//                 CompleteCheckout();
//             }
//
//             Save();
//
//             return CurrentStage;
//         }
//
//         public string TotalText {
//             get {
//                 var str = "";
//
//                 if (HasSingleDonation()) {
//                     str = SingleDonation.Cart.TotalText;
//                 }
//
//                 if (HasSingleDonation() && HasRegularDonation()) {
//                     str += " + ";
//                 }
//
//                 if (HasRegularDonation()) {
//                     str += RegularDonation.Cart.TotalText;
//                 }
//
//                 return str;
//             }
//         }
//
//         public Money TotalIncome => new Money((SingleDonation?.TotalIncome.Amount ?? 0) +
//                                               (RegularDonation?.TotalIncome.Amount ?? 0),
//                                               Currency);
//
//         public void Save() {
//             using (var scope = _scopeProvider.CreateScope()) {
//                 _checkoutSchema.CartId = CartId;
//                 _checkoutSchema.Datestamp = _localClock.GetUtcNow()
//                                                        .Date;
//
//                 _checkoutSchema.State = _jsonProvider.SerializeObject(_state);
//                 _checkoutSchema.Complete = CurrentStage == CheckoutStages.Complete;
//
//                 scope.Database.Save(_checkoutSchema);
//
//                 scope.Complete();
//             }
//         }
//
//         public void Delete() {
//             using (var scope = _scopeProvider.CreateScope()) {
//                 scope.Database.Delete(_checkoutSchema);
//
//                 scope.Complete();
//             }
//         }
//
//         public void Start() {
//             var requiredStages = new List<CheckoutStage>();
//
//             CartId = _cart.Id;
//             CurrentStage = CheckoutStages.Account;
//             CompletedStages = new List<CheckoutStage>();
//
//             Currency = _cart.Currency;
//
//             requiredStages.Add(CheckoutStages.Account);
//
//             if (_cart.HasRegularDonationItems()) {
//                 RegularDonation = new RegularDonationCheckout();
//                 RegularDonation.Cart = _cart.ToLiteCart(DonationTypes.Regular);
//
//                 requiredStages.Add(CheckoutStages.RegularPayment);
//             } else {
//                 RegularDonation = null;
//             }
//
//             if (_cart.HasSingleDonationItems()) {
//                 SingleDonation = new SingleDonationCheckout();
//                 SingleDonation.Cart = _cart.ToLiteCart(DonationTypes.Single);
//
//                 if (SingleDonation.Cart.Total.IsZero()) {
//                     SingleDonation.PaymentMethod = SinglePaymentMethod.Free;
//                 } else {
//                     requiredStages.Add(CheckoutStages.SinglePayment);
//                 }
//             } else {
//                 SingleDonation = null;
//             }
//
//             requiredStages.Add(CheckoutStages.Complete);
//
//             RequiredStages = requiredStages;
//
//             Save();
//         }
//
//         public bool HasSingleDonation() => SingleDonation != null;
//
//         public bool HasRegularDonation() => RegularDonation != null;
//
//         private void CompleteCheckout() {
//             if (HasRegularDonation()) {
//                 SubmitRegularDonation();
//             }
//
//             if (HasSingleDonation()) {
//                 SubmitSingleDonation();
//             }
//
//             SendReceiptEmail();
//
//             _cart.Clear();
//         }
//
//         private void SendReceiptEmail() {
//             var checkoutTemplateEmail = _contentLocator.Single<ICheckoutTemplateEmail>();
//
//             var viewModel = new CheckoutReceiptEmailModel(_localClock, _formatter, this);
//
//             var email = ComposeEmail.From(checkoutTemplateEmail.FromEmail, checkoutTemplateEmail.FromName)
//                                     .UsingTemplateEngine(new HandlebarsTemplateRenderer())
//                                     .To(Account.Email.Address)
//                                     .Subject(checkoutTemplateEmail.Subject)
//                                     .UsingTemplate(checkoutTemplateEmail.Body, viewModel);
//
//             _emailSender.Send(email);
//         }
//
//         private Basket GetBasket(DonationCheckoutBase checkout) {
//             return new Basket {
//                 Id = _cart.Id,
//                 Currency = (Currency) Enum.Parse(typeof(Currency), Currency.Name),
//                 Notes = checkout.Reference,
//                 Items = checkout.Cart.Allocations.Select(GetBasketItem)
//                                 .ToArray()
//             };
//         }
//
//         private BasketItem GetBasketItem(LiteCartAllocation allocation) {
//             var basketItem = new BasketItem();
//
//             basketItem.Amount = allocation.Value.Amount;
//             basketItem.LocationCode = allocation.FundDimension1;
//             basketItem.StipulationCode = allocation.FundDimension3;
//
//             if (allocation.AllocationType == AllocationTypes.Fund) {
//                 basketItem.DonationItemCode = allocation.DonationItem;
//             } else {
//                 basketItem.DonationItemCode = allocation.DonationType == DonationTypes.Single ? "OSY" : "OS";
//             }
//
//             return basketItem;
//         }
//
//         private AnalyticsDetailData[] GetAnalyticsData() {
//             return new AnalyticsDetailData[] { };
//         }
//
//         private void SubmitRegularDonation() {
//             var transaction = new RecurringPaymentTransaction();
//
//             transaction.TransactionId = Guid.NewGuid()
//                                             .ToString();
//
//             // TODO Move to clock
//             transaction.TransactionTimeUtc = DateTime.UtcNow;
//             transaction.Supporter = Account.ToCrmSupporter();
//             transaction.BasketCollectionId = CartId;
//             transaction.Basket = GetBasket(RegularDonation);
//             transaction.UserIp = _httpRequestAccessor.Request?.ClientIp();
//             transaction.SiteCode = "Site";
//             transaction.AnalyticsData = GetAnalyticsData();
//             transaction.PaymentMethod = RegularDonation.PaymentMethod;
//             transaction.PaymentDayOfMonth = RegularDonation.PaymentDayOfMonth;
//             transaction.BankAccountDetails = RegularDonation.BankDetails;
//             transaction.PaymentCardDetails = RegularDonation.CardTokenDetails;
//
//             _checkoutClient.CheckoutRecurringPaymentTransactionAsync(transaction)
//                            .ConfigureAwait(false)
//                            .GetAwaiter()
//                            .GetResult();
//         }
//
//         private bool SubmitSingleDonation() {
//             var transaction = new SinglePaymentTransaction();
//             transaction.TransactionId = Guid.NewGuid()
//                                             .ToString();
//
//             // TODO Move to clock
//             transaction.TransactionTimeUtc = DateTime.UtcNow;
//             transaction.Supporter = Account.ToCrmSupporter();
//             transaction.BasketCollectionId = CartId;
//             transaction.Basket = GetBasket(SingleDonation);
//             transaction.UserIp = _httpRequestAccessor.Request?.ClientIp();
//             transaction.SiteCode = "Site";
//             transaction.AnalyticsData = GetAnalyticsData();
//             transaction.PaymentMethod = SingleDonation.PaymentMethod;
//             transaction.PaypalDetails = SingleDonation.PayPalDetails;
//             transaction.WebPaymentCardDetails = SingleDonation.CardDetails;
//
//             var result = _checkoutClient.CheckoutSinglePaymentTransactionAsync(transaction)
//                                         .ConfigureAwait(false)
//                                         .GetAwaiter()
//                                         .GetResult();
//
//             return result.Body.CheckoutSinglePaymentTransactionResult;
//         }
//     }
// }