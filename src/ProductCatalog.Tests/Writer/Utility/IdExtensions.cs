using ProductCatalog.Writer.Feeds;

namespace ProductCatalog.Tests.Writer.Utility
{
    public static class IdExtensions
    {
        public static int GetValue(this Id id)
        {
            return PrivateField.GetValue<int>("value", id);
        }
    }
}