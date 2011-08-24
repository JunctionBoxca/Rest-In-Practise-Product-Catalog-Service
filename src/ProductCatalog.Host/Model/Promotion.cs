using System.Runtime.Serialization;

namespace ProductCatalog.Host.Model
{
    [DataContract(Name = "promotion", Namespace = "http://schemas.restbucks.com/promotion")]
    public class Promotion
    {
        [DataMember(Name="product")] public ProductReference ProductReference;
    }
}