using System.Runtime.Serialization;

namespace ProductCatalog.Host.Model
{
    [DataContract(Name = "region", Namespace = "http://schemas.restbucks.com/promotion")]
    public class RegionReference
    {
        [DataMember(Name = "type")] public const string Type = "application/vnd.restbucks+xml";
        [DataMember(Name = "href")] public string Href;
    }
}