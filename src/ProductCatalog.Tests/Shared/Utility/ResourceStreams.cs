using System.IO;
using System.Reflection;

namespace ProductCatalog.Tests.Shared.Utility
{
    public static class ResourceStreams
    {
        public static Stream CurrentFeed()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("ProductCatalog.Tests.Data.productcatalog.current.atom");
        }

        public static Stream ArchiveFeed()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("ProductCatalog.Tests.Data.productcatalog.archive.archive.atom");
        }
    }
}