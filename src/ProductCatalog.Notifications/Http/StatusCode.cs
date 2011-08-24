namespace ProductCatalog.Notifications.Http
{
    public class StatusCode : IHeader
    {
        public static readonly StatusCode OK = new StatusCode(200, "OK");
        public static readonly StatusCode NotModified = new StatusCode(304, "Not Modified");
        public static readonly StatusCode NotFound = new StatusCode(404, "Not Found");
        public static readonly StatusCode InternalServerError = new StatusCode(500, "Internal Server Error");

        private readonly int code;
        private readonly string description;

        public StatusCode(int code, string description)
        {
            this.code = code;
            this.description = description;
        }

        public void AddToResponse(IResponseWrapper response)
        {
            response.WriteStatusCode(code);
            response.WriteStatusDescription(description);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", code, description);
        }
    }
}