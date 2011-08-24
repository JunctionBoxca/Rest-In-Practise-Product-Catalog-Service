using System.IO;

namespace ProductCatalog.Notifications.Model
{
    public interface IEntityBodyTransformationStrategy
    {
        void WriteEntityBody(Stream destination);
    }
}