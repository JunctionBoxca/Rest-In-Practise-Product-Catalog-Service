using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Tests.Writer.Utility
{
    public static class FileNameExtensions
    {
        public static string GetValue(this FileName fileName)
        {
            return PrivateField.GetValue<string>("value", fileName);
        }
    }
}