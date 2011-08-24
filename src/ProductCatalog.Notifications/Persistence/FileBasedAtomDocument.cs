using System.IO;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;

namespace ProductCatalog.Notifications.Persistence
{
    public class FileBasedAtomDocument : IRepresentation
    {
        private readonly FileInfo fileInfo;
        private readonly IHeader chunking;
        private readonly ETag eTag;
        private readonly LastModified lastModified;

        public FileBasedAtomDocument(string fileName, IChunkingStrategy chunkingStrategy)
        {
            fileInfo = new FileInfo(fileName);

            if (!fileInfo.Exists)
            {
                throw new NotFoundException(string.Format("File does not exist. File: [{0}].", fileInfo.FullName));
            }

            eTag = new ETag(string.Format(@"""{0}#{1}""", fileInfo.Name, fileInfo.LastWriteTimeUtc.Ticks));
            chunking = chunkingStrategy.CreateHeader(fileInfo.Length);
            lastModified = new LastModified(fileInfo.LastWriteTimeUtc);
        }

        public void UpdateContext(IResponseContext context)
        {
            context.AddHeader(ContentType.Atom).AddHeader(chunking).AddHeader(eTag).AddHeader(lastModified);
            context.AddEntityBodyStreamAccessMethod(() => fileInfo.OpenRead());
        }
    }
}