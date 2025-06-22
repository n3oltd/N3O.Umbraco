using N3O.Umbraco.Cms;
using N3O.Umbraco.Utilities;

namespace DemoSite.Web;

public class Program {
    public static void Main(string[] args) {
        OurAssemblies.Configure("DemoSite.");
        
        UmbracoCms.Run<Startup>(args);
    }
}
