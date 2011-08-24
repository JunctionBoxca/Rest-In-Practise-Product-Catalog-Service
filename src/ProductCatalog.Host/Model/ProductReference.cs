using System.Runtime.Serialization;

namespace ProductCatalog.Host.Model
{
    [DataContract(Name = "product", Namespace = "http://schemas.restbucks.com/promotion")]
    public class ProductReference
    {
        [DataMember(Name = "type")] public const string Type = "application/vnd.restbucks+xml";
        [DataMember(Name = "href")] public string Href;
    }
}