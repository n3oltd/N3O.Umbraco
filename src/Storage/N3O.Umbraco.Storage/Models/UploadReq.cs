using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Storage.Models {
    public class UploadReq {
        [Name("File")]
        public IFormFile File { get; set; }
    }
}