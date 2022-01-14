using N3O.Umbraco;

namespace DemoSite.Web {
    public class Program {
        public static void Main(string[] args) {
            Cms.Run<Startup>(args, "DemoSite.");
        }
    }
}