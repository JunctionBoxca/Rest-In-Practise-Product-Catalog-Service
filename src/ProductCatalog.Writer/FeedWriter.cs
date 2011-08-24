using System;
using System.Reflection;
using log4net;
using ProductCatalog.Shared;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Persistence;
using ProductCatalog.Writer.Tasks;

namespace ProductCatalog.Writer
{
    public class FeedWriter : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public event EventHandler<FeedMappingsChangedEventArgs> FeedMappingsChanged;

        private readonly ITimer timer;
        private readonly IFileSystem fileSystem;
        private readonly EventBuffer buffer;
        private readonly FeedBuilder feedBuilder;
        private readonly Reaper reaper;
 
        public FeedWriter(ITimer timer, EventBuffer buffer, IFileSystem fileSystem, FeedBuilder feedBuilder)
        {
            Check.IsNotNull(timer, "timer");
            Check.IsNotNull(buffer, "buffer");
            Check.IsNotNull(fileSystem, "fileSystem");
            Check.IsNotNull(feedBuilder, "feedBuilder");

            this.timer = timer;
            this.fileSystem = fileSystem;
            this.buffer = buffer;
            this.feedBuilder = feedBuilder;

            this.timer.TimerFired += TimerFiredHandler;

            reaper = new Reaper(fileSystem);
            FeedMappingsChanged += reaper.OnFeedMappingsChanged;
        }

        private void TimerFiredHandler(object sender, EventArgs e)
        {
            WriteFeed();
        }

        private void WriteFeed()
        {
            timer.Stop();

            ITask task = new QueryingEvents();
            while (!task.IsLastTask)
            {
                task = task.Execute(fileSystem, buffer, feedBuilder, NotifyMappingsChanged);
            }

            timer.Start();
        }

        private void NotifyMappingsChanged(FeedMappingsChangedEventArgs args)
        {
            EventHandler<FeedMappingsChangedEventArgs> handler = FeedMappingsChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void Dispose()
        {
            Log.Debug("Disposing FeedWriter...");
            timer.TimerFired -= TimerFiredHandler;
            FeedMappingsChanged -= reaper.OnFeedMappingsChanged;
        }
    }
}