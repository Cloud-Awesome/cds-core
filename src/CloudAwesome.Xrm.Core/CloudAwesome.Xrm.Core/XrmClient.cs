using System;
using CloudAwesome.Xrm.Core.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace CloudAwesome.Xrm.Core
{
    public static class XrmClient
    {
        public static CrmServiceClient GetCrmServiceClient(string connectionString)
        {
            return new CrmServiceClient(connectionString);
        }

        public static IOrganizationService GetCrmServiceClientWithO365(string url, string username, string password)
        {
            var connectionString =
                "AuthType=Office365;" +
                $"Username={username};" +
                $"Password='{password}';" +
                $"Url={url}";
            return GetCrmServiceClient(connectionString);
        }

        public static IOrganizationService GetCrmServiceClientWithClientSecret(string url, string appId, string appSecret)
        {
            var connectionString =
                "AuthType=ClientSecret;" +
                $"ClientId={appId};" +
                $"ClientSecret='{appSecret}';" +
                $"Url={url}";
            return GetCrmServiceClient(connectionString);
        }

        public static IOrganizationService GetCrmServiceClientFromManifestConfiguration(CdsConnection cdsConnection)
        {
            switch (cdsConnection.ConnectionType)
            {
                case CdsConnectionType.AppRegistration:
                    return GetCrmServiceClientWithClientSecret(cdsConnection.CdsUrl, cdsConnection.CdsAppId,
                        cdsConnection.CdsAppSecret);
                case CdsConnectionType.ConnectionString:
                    return GetCrmServiceClient(cdsConnection.CdsConnectionString);
                case CdsConnectionType.UserNameAndPassword:
                    return GetCrmServiceClientWithO365(cdsConnection.CdsUrl, cdsConnection.CdsUserName,
                        cdsConnection.CdsPassword);
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
