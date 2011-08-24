using System;
using ProductCatalog.Shared;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Writer.Tasks
{
    public interface ITask
    {
        bool IsLastTask { get; }
        ITask Execute(IFileSystem fileSystem, IEventBuffer buffer, FeedBuilder feedBuilder, Action<FeedMappingsChangedEventArgs> notifyMappingsChanged);
    }
}