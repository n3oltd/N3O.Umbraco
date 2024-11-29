using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using System;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfundingNotifications {
    void Enqueue(FundraiserNotificationType notificationType, FundraiserNotificationViewModel model, Guid contentKey);
}