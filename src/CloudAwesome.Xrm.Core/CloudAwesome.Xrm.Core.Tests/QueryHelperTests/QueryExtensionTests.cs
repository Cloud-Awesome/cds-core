using System.Collections.Generic;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.QueryHelperTests
{
    public class QueryExtensionTests: BaseFakeXrmTest
    {
        [Test(Description = "If no results are found for the expected single record, NULL is returned instead. No exception is thrown")]
        public void IfNoResultsAreFoundReturnsNull()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();
            
            var retrievedAccount = (Account)SampleAccountQueryExpression.RetrieveSingleRecord(orgService);
            
            Assert.IsNull(retrievedAccount);
        }

        [Test(Description = "Extension method to retrieve single record from QueryBase (QueryExpression test)")]
        public void QueryExpressionSingleRecordExtensionMethod()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1
            });

            var retrievedAccount = (Account)SampleAccountQueryExpression.RetrieveSingleRecord(orgService);

            Assert.AreEqual(TestAccount1.Id, retrievedAccount.Id);
            Assert.AreEqual(TestAccount1.Name, retrievedAccount.Name);
        }

        [Test(Description = "Extension method to retrieve single record from QueryBase (QueryByAttribute test)")]
        public void QueryByAttributeSingleRecordExtensionMethod()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1
            });

            var retrievedAccount = (Account)SampleAccountQueryByAttribute.RetrieveSingleRecord(orgService);

            Assert.AreEqual(TestAccount1.Id, retrievedAccount.Id);
            Assert.AreEqual(TestAccount1.Name, retrievedAccount.Name);
        }

        [Test(Description = "Extension method to retrieve multiple records from Query Base (QueryExpression test)")]
        public void QueryExpressionEntityCollectionExtensionMethod()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1,
                TestAccount1Duplicate
            });

            var retrievedAccounts = SampleAccountQueryExpression.RetrieveMultiple(orgService);

            Assert.AreEqual(2, retrievedAccounts.Entities.Count);
        }

        [Test(Description = "Extension method to retrieve multiple records from Query Base (QueryByAttribute test)")]
        public void QueryByAttributeEntityCollectionExtensionMethod()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1,
                TestAccount1Duplicate
            });

            var retrievedAccounts = SampleAccountQueryByAttribute.RetrieveMultiple(orgService);

            Assert.AreEqual(2, retrievedAccounts.Entities.Count);
        }
    }
}
