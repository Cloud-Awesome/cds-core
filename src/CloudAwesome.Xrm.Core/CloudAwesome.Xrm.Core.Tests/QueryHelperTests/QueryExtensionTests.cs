using System.Collections.Generic;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.QueryHelperTests
{
    public class QueryExtensionTests: BaseFakeXrmTest
    {

        [Test(Description = "Extension method to retrieve single record from QueryBase (QueryExpression test)")]
        public void QueryExpressionSingleRecordExtensionMethod()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1
            });

            var retrievedAccount = (Account)SampleQueryExpression.RetrieveSingleRecord(orgService);

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

            var retrievedAccount = (Account)SampleQueryByAttribute.RetrieveSingleRecord(orgService);

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

            var retrievedAccounts = SampleQueryExpression.RetrieveMultiple(orgService);

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

            var retrievedAccounts = SampleQueryByAttribute.RetrieveMultiple(orgService);

            Assert.AreEqual(2, retrievedAccounts.Entities.Count);
        }
    }
}
