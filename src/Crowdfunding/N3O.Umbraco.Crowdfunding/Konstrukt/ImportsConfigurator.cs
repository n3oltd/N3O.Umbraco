using Konstrukt.Configuration;
using Konstrukt.Configuration.Builders;

namespace N3O.Umbraco.CrowdFunding.Konstrukt;

public class ImportsConfigurator : IKonstruktConfigurator {
    public void Configure(KonstruktConfigBuilder builder) {
        var section = builder.AddSection("Donations");
        var dashboard = section.AddDashboard("List");
        /*section.SetVisibility(cfg => cfg.ShowForUserGroup(DataConstants.SecurityGroups.ImportUsers.Alias));*/

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
        collection.SetDateCreatedProperty(c => c.CreatedAt);
        collection.AddSearchableProperty(c => c.PageId);
        collection.AddSearchableProperty(c => c.Reference);
        collection.DisableCreate();
        /*collection.SetDataViewsBuilder<ImportActionDataViewsBuilder>();
        collection.SetDataViewsBuilder<ImportStatusDataViewsBuilder>();*/

        ConfigureListView(collection);

        ConfigureEditor(collection);
    }

    private void ConfigureListView(KonstruktDashboardCollectionConfigBuilder<CrowdfundingDonations> collection) {
        var listView = collection.ListView();
        listView.AddField(c => c.Reference);
        listView.AddField(c => c.CreatedAt);
        listView.AddField(c => c.PageId);
        listView.AddField(c => c.Reference);
    }

    private void ConfigureEditor(KonstruktDashboardCollectionConfigBuilder<CrowdfundingDonations> collection) {
        var editor = collection.Editor();
        var recordTab = editor.AddTab("Donation");
        
        var recordFieldset = recordTab.AddFieldset("Donation");
        /*recordFieldset.AddField(c => c.QueuedAt).MakeReadOnly();
        recordFieldset.AddField(c => c.QueuedBy).MakeReadOnly();
        recordFieldset.AddField(c => c.ContentTypeName).MakeReadOnly();
        recordFieldset.AddField(c => c.Reference).MakeReadOnly();
        recordFieldset.AddField(c => c.Filename).MakeReadOnly();
        recordFieldset.AddField(c => c.Row).MakeReadOnly();
        recordFieldset.AddField(c => c.Status).MakeReadOnly();
        recordFieldset.AddField(c => c.Data).SetDataType(ImportDataEditorDataEditor.DataEditorName);
        recordFieldset.AddField(c => c.Notices).SetDataType(ImportNoticesViewerDataEditor.DataEditorName);*/
    }
}
