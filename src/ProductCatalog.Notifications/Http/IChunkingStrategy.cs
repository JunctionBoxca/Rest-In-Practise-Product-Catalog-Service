namespace ProductCatalog.Notifications.Http
{
    public interface IChunkingStrategy
    {
        IHeader CreateHeader(long contentLength);
    }
}