using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
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

        public static EntityReference GetRootBusinessUnit(IOrganizationService organizationService)
        {
            var rootBusinessUnit = new QueryExpression()
            {
                EntityName = "businessunit",
                ColumnSet = new ColumnSet("name"),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression("parentbusinessunitid", ConditionOperator.Null)
                    }
                }
            }.RetrieveSingleRecord(organizationService);

            return rootBusinessUnit.ToEntityReference();
        }

        public static EntityReference GetBaseCurrency(IOrganizationService organizationService)
        {
            var baseCurrency = new QueryExpression()
            {
                EntityName = "organization",
                ColumnSet = new ColumnSet("basecurrencyid")
            }.RetrieveSingleRecord(organizationService).GetAttributeValue<EntityReference>("basecurrencyid");

            return baseCurrency;
        }
    }
}
