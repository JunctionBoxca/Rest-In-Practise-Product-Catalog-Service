using Castle.MicroKernel.Registration;
using Castle.Windsor;
using ProductCatalog.Notifications;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Notifications.Persistence;
using ProductCatalog.Notifications.Service;
using ProductCatalog.Shared;
using ProductCatalog.Writer;
using ProductCatalog.Writer.Feeds;
using ProductCatalog.Writer.Model;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Host.Setup
{
    public class Container
    {
        private readonly IWindsorContainer container;

        public Container(UriConfiguration uriConfiguration, FileSystemConfiguration fileSystemConfiguration)
        {
            container = new WindsorContainer();

            AddConfiguration(uriConfiguration, fileSystemConfiguration);
            AddCommonComponents();
            AddNotificationsServiceComponents();
            AddWriterComponents();
            WireUpEvents();
        }

        public ServiceHost ServiceHost
        {
            get { return container.Resolve<ServiceHost>(); }
        }

        public ITimer Timer
        {
            get { return container.Resolve<ITimer>(); }
        }

        public EventBuffer EventBuffer
        {
            get { return container.Resolve<EventBuffer>(); }
        }

        private void AddConfiguration(UriConfiguration uriConfiguration, FileSystemConfiguration fileSystemConfiguration)
        {
            container.Register(Component.For(typeof (UriConfiguration)).Instance(uriConfiguration).LifeStyle.Singleton);
            container.Register(Component.For(typeof (FileSystemConfiguration)).Instance(fileSystemConfiguration).LifeStyle.Singleton);
        }

        private void AddCommonComponents()
        {
            container.Register(Component.For(typeof (Links)).LifeStyle.Singleton);
        }

        private void AddNotificationsServiceComponents()
        {
            container.Register(Component.For(typeof (ResourceIdToStoreIdConverter)).Instance(ResourceIdToStoreIdConverter.Default).LifeStyle.Singleton);
            container.Register(Component.For(typeof (IRepository)).ImplementedBy(typeof (Repository)).LifeStyle.Singleton);
            container.Register(Component.For(typeof (IStore)).ImplementedBy(typeof (FileStore)));
            container.Register(Component.For(typeof (Routes)));
            container.Register(Component.For(typeof (NotificationsService)));
            container.Register(Component.For(typeof (ServiceHost)).LifeStyle.Singleton);
        }

        private void AddWriterComponents()
        {
            container.Register(Component.For(typeof (ITimer)).Instance(new ScheduledTimer(10000)).LifeStyle.Singleton);
            container.Register(Component.For(typeof (EventBuffer)).LifeStyle.Singleton);
            container.Register(Component.For(typeof (FeedWriter)).LifeStyle.Singleton);
            container.Register(Component.For(typeof (FeedBuilder)).LifeStyle.Singleton);
            container.Register(Component.For(typeof (IFileSystem)).ImplementedBy(typeof (FileSystem)));
        }

        private void WireUpEvents()
        {
            container.Resolve<FeedWriter>().FeedMappingsChanged += ((Repository) container.Resolve<IRepository>()).OnFeedMappingsChanged;
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}