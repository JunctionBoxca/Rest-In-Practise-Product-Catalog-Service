using ProductCatalog.Shared;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Writer.Feeds
{
    public class FeedMapping
    {
        private readonly Id id;
        private readonly FileName fileName;

        public FeedMapping(Id id) : this(id, FileName.TempFileName())
        {
        }

        private FeedMapping(Id id, FileName fileName)
        {
            this.id = id;
            this.fileName = fileName;
        }

        public FeedMapping WithArchiveFileName()
        {
            return new FeedMapping(id.Clone(), id.CreateFileName());
        }

        public FeedMapping WithNextId()
        {
            return new FeedMapping(id.Increment());
        }

        public FileName FileName
        {
            get { return fileName; }
        }

        public Id Id
        {
            get { return id; }
        }

        public FeedMappingsChangedEventArgs CreateFeedMappingsChangedEventArgs()
        {
            return new FeedMappingsChangedEventArgs(id.ToString(), fileName.ToString());
        }
    }
}