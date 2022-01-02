using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Plugins.Models {
    public class ImageUploadReq {
        [Name("Min Height")]
        public int? MinHeight { get; set; }
    
        [Name("Min Width")]
        public int? MinWidth { get; set; }
    
        [Name("File")]
        public IFormFile File { get; set; }
    }
}