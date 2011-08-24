using System.IO;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Shared;

namespace ProductCatalog.Notifications.Persistence
{
    public class FileStore : IStore
    {
        private readonly FileSystemConfiguration fileSystemConfiguration;

        public FileStore(FileSystemConfiguration fileSystemConfiguration)
        {
            Check.IsNotNull(fileSystemConfiguration, "fileSystemConfiguration");

            this.fileSystemConfiguration = fileSystemConfiguration;
        }

        public IRepresentation GetArchiveFeed(IStoreId id)
        {
            string fileName = Path.Combine(fileSystemConfiguration.ArchiveDirectoryPath, ((IStoreId<string>) id).Value);
            return new FileBasedAtomDocument(fileName, new SendChunked(true));
        }

        public IRepresentation GetCurrentFeed(IStoreId id)
        {
            string fileName = Path.Combine(fileSystemConfiguration.CurrentDirectoryPath, ((IStoreId<string>) id).Value);
            return new FileBasedAtomDocument(fileName, new SendChunked(true));
        }
    }
}