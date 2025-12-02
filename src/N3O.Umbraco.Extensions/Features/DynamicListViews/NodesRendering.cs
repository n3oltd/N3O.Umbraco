using N3O.Umbraco.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Trees;

namespace N3O.Umbraco.Features.DynamicListViews;

public class NodesRendering : INotificationAsyncHandler<TreeNodesRenderingNotification> {
    public Task HandleAsync(TreeNodesRenderingNotification notification, CancellationToken cancellationToken) {
        var dynamicListViewsEnabledNodes = notification.Nodes.Where(DynamicListViewsEnabled).ToList();

        if (dynamicListViewsEnabledNodes.Any()) {
            foreach (var node in dynamicListViewsEnabledNodes) {
                node.AdditionalData["isContainer"] = true;
                node.CssClasses.Add("isContainer");
            }
        }

        return Task.CompletedTask;
    }

    private bool DynamicListViewsEnabled(TreeNode treeNode) {
        if (treeNode.Id?.ToString().TryParseAs<int>().HasValue() == true) {
            return ContentPathHelper.DynamicListViewsEnabled(treeNode.Path);
        } else {
            return false;
        }
    }
}