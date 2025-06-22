using N3O.Umbraco.Cms;

namespace DemoSite.Web;

public class Program {
    public static void Main(string[] args) {
        UmbracoCms.Run<Startup>(args, "DemoSite.");
    }
}
