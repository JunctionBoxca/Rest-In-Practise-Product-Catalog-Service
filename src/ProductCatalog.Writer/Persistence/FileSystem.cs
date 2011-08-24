using ProductCatalog.Shared;

namespace ProductCatalog.Writer.Persistence
{
    public class FileSystem : IFileSystem
    {
        private readonly IDirectory currentDirectory;
        private readonly IDirectory archiveDirectory;
        private readonly IDirectory entryDirectory;

        public FileSystem(FileSystemConfiguration fileSystemConfiguration)
        {
            currentDirectory = new Directory(fileSystemConfiguration.CurrentDirectoryPath);
            archiveDirectory = new Directory(fileSystemConfiguration.ArchiveDirectoryPath);
            entryDirectory = new Directory(fileSystemConfiguration.EntryDirectoryPath);
        }

        public IDirectory CurrentDirectory
        {
            get { return currentDirectory; }
        }

        public IDirectory ArchiveDirectory
        {
            get { return archiveDirectory; }
        }

        public IDirectory EntryDirectory
        {
            get { return entryDirectory; }
        }
    }
}