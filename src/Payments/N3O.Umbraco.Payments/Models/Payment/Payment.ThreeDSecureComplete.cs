using System;

namespace N3O.Umbraco.Payments.Models {
    public partial class Payment {
        public void ThreeDSecureComplete(string cRes) {
            if (Card?.ThreeDSecureRequired != true || Card?.ThreeDSecureCompleted == false) {
                throw new Exception("Cannot complete 3D secure in the current state");
            }
            
            Card = Card.ThreeDSecureComplete(cRes);
        }
    }
}