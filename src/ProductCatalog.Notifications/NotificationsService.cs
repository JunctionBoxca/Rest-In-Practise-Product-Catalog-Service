using System;
using System.Reflection;
using log4net;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Notifications.Persistence;
using ProductCatalog.Notifications.Service;
using ProductCatalog.Shared;

namespace ProductCatalog.Notifications
{
    public class NotificationsService
    {
        private readonly Routes routes;
        private readonly IRepository repository;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public NotificationsService(Routes routes, IRepository repository)
        {
            Check.IsNotNull(routes, "routes");
            Check.IsNotNull(repository, "repository");

            this.routes = routes;
            this.repository = repository;

            Log.InfoFormat("Service initialized. Repository: [{0}].", repository.GetType());
        }

        public Uri BaseAddress
        {
            get { return routes.BaseAddress; }
        }

        public IResponse GetResponse(IRequestWrapper request)
        {
            Log.DebugFormat("Received request. Uri: [{0}].", request.Uri.AbsoluteUri);

            try
            {
                IRepositoryCommand command = routes.CreateCommand(request.Uri);
                IRepresentation representation = command.Execute(repository);
                return request.Condition.CreateResponse(representation);
            }
            catch (ServerException ex)
            {
                Log.ErrorFormat("Server exception. {0}", ex.Message);
                return Response.InternalServerError();
            }
            catch (InvalidUriException ex)
            {
                Log.WarnFormat("Invalid request. {0}", ex.Message);
                return Response.NotFound();
            }
            catch (NotFoundException ex)
            {
                Log.WarnFormat("Invalid request. {0}", ex.Message);
                return Response.NotFound();
            }
        }
    }
}