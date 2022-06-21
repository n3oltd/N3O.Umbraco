using Konstrukt.Configuration;
using Konstrukt.Configuration.Builders;
using N3O.Umbraco.Data.DataTypes;

namespace N3O.Umbraco.Data.Konstrukt;

public class ImportsConfigurator : IKonstruktConfigurator {
    public void Configure(KonstruktConfigBuilder builder) {
        var dashboard = builder.AddDashboard("Imports");
        dashboard.SetVisibility(cfg => cfg.ShowInSection("content"));

        ConfigureCollection(dashboard);
    }

    private void ConfigureCollection(KonstruktDashboardConfigBuilder dashboard) {
        var collection = dashboard.SetCollection<Import>(x => x.Id,
                                                            "Import",
                                                            "Imports",
                                                            "Data imports",
                                                            "icon-arrow-up",
                                                            "icon-arrow-up");

        collection.SetAlias("pendingImports");
        collection.SetNameProperty(c => c.Reference);
        collection.SetDateCreatedProperty(c => c.QueuedAt);
        collection.AddSearchableProperty(c => c.QueuedByName);
        collection.AddSearchableProperty(c => c.Reference);
        collection.AddSearchableProperty(c => c.ContentTypeName);
        collection.AddSearchableProperty(c => c.ImportedContentSummary);
        collection.AddSearchableProperty(c => c.BatchFilename);
        collection.DisableCreate();
        collection.SetDataViewsBuilder<ImportActionDataViewsBuilder>();
        collection.SetDataViewsBuilder<ImportStatusDataViewsBuilder>();

        ConfigureListView(collection);

        ConfigureEditor(collection);
    }

    private void ConfigureListView(KonstruktDashboardCollectionConfigBuilder<Import> collection) {
        var listView = collection.ListView();
        listView.AddField(c => c.QueuedAt);
        listView.AddField(c => c.QueuedByName);
        listView.AddField(c => c.ContentTypeName);
        listView.AddField(c => c.Reference);
        listView.AddField(c => c.ImportedContentSummary);
        listView.AddField(c => c.Status);
    }

    private void ConfigureEditor(KonstruktDashboardCollectionConfigBuilder<Import> collection) {
        var editor = collection.Editor();
        var recordTab = editor.AddTab("Record");
        
        var recordFieldset = recordTab.AddFieldset("Record");
        recordFieldset.AddField(c => c.Data).SetDataType(ImportDataEditorDataEditor.DataEditorName);
        recordFieldset.AddField(c => c.Notices).SetDataType(ImportNoticesViewerDataEditor.DataEditorName);
    }
}
