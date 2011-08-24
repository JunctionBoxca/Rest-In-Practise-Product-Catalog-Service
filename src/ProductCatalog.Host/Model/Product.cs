using System.Runtime.Serialization;

namespace ProductCatalog.Host.Model
{
    [DataContract(Name = "product", Namespace = "http://schemas.restbucks.com/product")]
    public class Product
    {
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "size")] public string Size;
        [DataMember(Name = "price")] public double Price;
    }
}