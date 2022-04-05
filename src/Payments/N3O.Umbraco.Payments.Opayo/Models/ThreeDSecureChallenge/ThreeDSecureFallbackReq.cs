using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class ThreeDSecureFallbackReq {
        [Name("PaRes")]
        public string PaRes { get; set; }

        [Name("MD")]
        public string Md { get; set; }
        
        [Name("MDX")]
        public string Mdx { get; set; }
    }
}