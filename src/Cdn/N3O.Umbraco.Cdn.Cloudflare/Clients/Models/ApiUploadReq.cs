namespace N3O.Umbraco.Cdn.Cloudflare.Clients;

public class ApiUploadReq {
    public string Url { get; set; }
    public ApiMetadataReq Meta { get; set; }
}