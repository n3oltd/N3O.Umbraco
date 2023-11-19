using Konstrukt.Configuration;
using Konstrukt.Configuration.Builders;

namespace N3O.Umbraco.CrowdFunding.Konstrukt;

public class CrowdfundingDonationsConfigurator : IKonstruktConfigurator {
    public void Configure(KonstruktConfigBuilder builder) {
        var section = builder.AddSectionAfter("content","Donations");
        var dashboard = section.AddDashboard("List");
        
        ConfigureCollection(dashboard);
    }

    private void ConfigureCollection(KonstruktDashboardConfigBuilder dashboard) {
        var collection = dashboard.SetCollection<CrowdfundingDonations>(x => x.Id,
                                                                        "CrowdfundingDonation",
                                                                        "CrowdfundingDonations",
                                                                        "Crowdfunding Donations",
                                                                        "icon-arrow-up",
                                                                        "icon-arrow-up");

        collection.SetAlias("crowdfundingDonations");
        collection.SetNameProperty(c => c.Reference);
        collection.AddFilterableProperty(c => c.Reference);
        collection.SetDateCreatedProperty(c => c.CreatedAt);
        collection.AddSearchableProperty(c => c.PageId);
        collection.AddSearchableProperty(c => c.Comment);
        collection.DisableCreate();
        collection.DisableDelete();

        ConfigureListView(collection);

        ConfigureEditor(collection);
    }

    private void ConfigureListView(KonstruktDashboardCollectionConfigBuilder<CrowdfundingDonations> collection) {
        var listView = collection.ListView();
        listView.AddField(c => c.Reference);
        listView.AddField(c => c.CreatedAt);
        listView.AddField(c => c.PageId);
        listView.AddField(c => c.Comment);
    }

    private void ConfigureEditor(KonstruktDashboardCollectionConfigBuilder<CrowdfundingDonations> collection) {
        var editor = collection.Editor();
        var recordTab = editor.AddTab("Donation");
        
        var recordFieldset = recordTab.AddFieldset("Donation");
        recordFieldset.AddField(c => c.Reference).MakeReadOnly();
        recordFieldset.AddField(c => c.CreatedAt).MakeReadOnly();
        recordFieldset.AddField(c => c.PageId).MakeReadOnly();
        recordFieldset.AddField(c => c.Amount).MakeReadOnly();
        recordFieldset.AddField(c => c.Currency).MakeReadOnly();
        recordFieldset.AddField(c => c.FirstName).MakeReadOnly();
        recordFieldset.AddField(c => c.LastName).MakeReadOnly();
        recordFieldset.AddField(c => c.Email).MakeReadOnly();;
        recordFieldset.AddField(c => c.Comment);
    }
}
