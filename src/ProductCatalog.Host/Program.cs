using System;
using System.Reflection;
using log4net;
using log4net.Config;
using ProductCatalog.Host.Model;
using ProductCatalog.Host.Setup;
using ProductCatalog.Shared;
using ProductCatalog.Writer;
using ProductCatalog.Writer.Model;

namespace ProductCatalog.Host
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            UriConfiguration uriConfiguration = new UriConfiguration(
                new Uri("http://localhost/product-catalog/notifications/"),
                new UriTemplate("/recent"),
                new UriTemplate("/?page={id}"),
                new UriTemplate("/notification/{id}"));

            FileSystemConfiguration fileSystemConfiguration = new FileSystemConfiguration(@"c:\");

            Container container = new Container(uriConfiguration, fileSystemConfiguration);
            container.Timer.Start();

            ITimer publishTimer = StartPublishingTo(container.EventBuffer);


            try
            {
                Console.WriteLine("Starting server...");
                container.ServiceHost.StartHost();
                Console.WriteLine("Server started. Press any key to terminate.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.ReadKey();

                publishTimer.Stop();
                publishTimer.Dispose();

                container.Dispose();
            }
        }

        private static ITimer StartPublishingTo(EventBuffer eventBuffer)
        {
            ITimer publishTimer = new ScheduledTimer(2000);
            int id = 0;

            publishTimer.TimerFired += ((o, a) =>
                                            {
                                                publishTimer.Stop();
                                                eventBuffer.Add(new Event(id++, "event: " + id, DateTime.Now, new EventBody("application/vnd.restbucks+xml", new Uri("http://product/" + id), new Product {Name = "product", Price = 10.0, Size = "1kg"})));
                                                publishTimer.Start();
                                            });
            publishTimer.Start();
            return publishTimer;
        }
    }
}