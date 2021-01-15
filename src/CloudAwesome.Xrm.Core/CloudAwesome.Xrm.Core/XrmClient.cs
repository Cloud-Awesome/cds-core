using CloudAwesome.Xrm.Core.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;

namespace CloudAwesome.Xrm.Core
{
    /// <summary>
    /// Wrapper around the Xrm.Tooling connector
    /// </summary>
    public static class XrmClient
    {
        /// <summary>
        /// Creates a new CrmServiceClient given a valid connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>CrmServiceClient</returns>
        public static IOrganizationService GetCrmServiceClient(string connectionString)
        {
            return new CrmServiceClient(connectionString);
        }

        /// <summary>
        /// Creates a new CrmServiceClient given a valid username, password and target url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>CrmServiceClient</returns>
        public static IOrganizationService GetCrmServiceClientWithO365(string url, string username, string password)
        {
            var connectionString =
                "AuthType=Office365;" +
                $"Username={username};" +
                $"Password='{password}';" +
                $"Url={url}";
            return GetCrmServiceClient(connectionString);
        }

        /// <summary>
        /// Creates a new CrmServiceClient given valid AAD app registration details
        /// </summary>
        /// <param name="url"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns>CrmServiceClient</returns>
        public static IOrganizationService GetCrmServiceClientWithClientSecret(string url, string appId, string appSecret)
        {
            var connectionString =
                "AuthType=ClientSecret;" +
                $"ClientId={appId};" +
                $"ClientSecret='{appSecret}';" +
                $"Url={url}";
            return GetCrmServiceClient(connectionString);
        }

        /// <summary>
        /// Creates a new CrmServiceClient from configuration passed through an XML manifest configuration
        /// </summary>
        /// <param name="cdsConnection"></param>
        /// <returns>CrmServiceClient</returns>
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

        /// <summary>
        /// Gets a reference to the target environment's root business unit (usually the same name as the Org)
        /// </summary>
        /// <param name="organizationService"></param>
        /// <returns>EntityReference of the root BusinessUnit</returns>
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

        /// <summary>
        /// Gets a reference to the target environment's base currency 
        /// </summary>
        /// <param name="organizationService"></param>
        /// <returns>EntityReference of the BaseCurrency</returns>
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
