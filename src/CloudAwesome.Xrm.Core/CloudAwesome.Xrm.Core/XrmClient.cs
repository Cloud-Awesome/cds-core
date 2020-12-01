using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace CloudAwesome.Xrm.Core
{
    public static class XrmClient
    {
        // TODO - * Tracing

        public static CrmServiceClient GetCrmServiceClient(string connectionString)
        {
            return new CrmServiceClient(connectionString);
        }

        public static CrmServiceClient GetCrmServiceClientWithO365(string url, string username, string password)
        {
            var connectionString =
                "AuthType=Office365;" +
                $"Username={username};" +
                $"Password='{password}';" +
                $"Url={url}";
            return GetCrmServiceClient(connectionString);
        }

        public static CrmServiceClient GetCrmServiceClientWithClientSecret(string url, string appId, string appSecret)
        {
            var connectionString =
                "AuthType=ClientSecret;" +
                $"ClientId={appId};" +
                $"ClientSecret='{appSecret}';" +
                $"Url={url}";
            return GetCrmServiceClient(connectionString);
        }

    }
}
