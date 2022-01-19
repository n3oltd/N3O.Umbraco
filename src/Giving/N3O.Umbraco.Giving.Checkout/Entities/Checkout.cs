using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Counters;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Payments.Entities;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout : Entity, IPaymentsFlow {
        public Checkout() {
            RequiredStages = new List<CheckoutStage>();
            CompletedStages = new List<CheckoutStage>();
        }

        public Guid CartId { get; private set; }
        public Reference Reference { get; private set; }
        public CheckoutStage CurrentStage { get; private set; }
        public List<CheckoutStage> RequiredStages { get; private set; }
        public List<CheckoutStage> CompletedStages { get; private set; }
        public Currency Currency { get; private set; }
        public Account Account { get; private set; }
        public CartContents Single { get; private set; }
        public CartContents Regular { get; private set; }
    }
}