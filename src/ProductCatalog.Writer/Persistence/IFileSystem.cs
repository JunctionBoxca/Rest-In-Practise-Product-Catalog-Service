namespace ProductCatalog.Writer.Persistence
{
    public interface IFileSystem
    {
        IDirectory CurrentDirectory { get; }
        IDirectory ArchiveDirectory { get; }
        IDirectory EntryDirectory { get; }
    }
}