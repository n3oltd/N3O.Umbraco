using N3O.Umbraco.Extensions;
using N3O.Umbraco.StructuredData;
using DemoSite.Core.Content;
using Umbraco.Extensions;

namespace DemoSite.Core.StructuredData {
    public class HomePageStructuredData : StructuredDataProvider<HomePage> {
        protected override void AddStructuredData(JsonLd jsonLd, HomePage page) {
            jsonLd.OfType("NGO")
                  .Name("Demo NGO")
                  .Url(page.Url())
                  .Email("info@example.com")
                  .Telephone("123456789");

            jsonLd.Nest("location")
                  .OfType("PostalAddress")
                  .Address("123 Street, City")
                  .PostalCode("ABC 123")
                  .Country("GBR");
        }
    }
}