using System.Threading.Tasks;

namespace N3O.Umbraco.Extensions;

public static class TaskExtensions {
    public static T GetResult<T>(this Task task) {
        return (T) GetResult(task);
    }

    public static object GetResult(this Task task) {
        var resultProperty = task.GetType().GetProperty(nameof(Task<object>.Result));

        return resultProperty.GetValue(task);
    }
}
