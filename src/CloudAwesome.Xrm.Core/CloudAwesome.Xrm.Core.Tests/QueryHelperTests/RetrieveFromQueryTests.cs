using System;
using System.Collections.Generic;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.QueryHelperTests
{
    [TestFixture]
    public class RetrieveFromQueryTests: BaseFakeXrmTest
    {
        [Test(Description = "Happy path for RetrieveByAttribute. Query describes a single record which is retrieved")]
        public void BasicRetrieveByQueryByAttribute()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1
            });

            var retrievedAccount = (Account) QueryExtensions.RetrieveRecordFromQuery<QueryByAttribute>(orgService, SampleQueryByAttribute);

            Assert.AreEqual(TestAccount1.Id, retrievedAccount.Id);
            Assert.AreEqual(TestAccount1.Name, retrievedAccount.Name);
        }

        [Test(Description = "Happy path for RetrieveByQueryExpression. QE describes a single record which is retrieved")]
        public void BasicRetrieveByQueryExpression()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1
            });
            
            var retrievedAccount = (Account) QueryExtensions.RetrieveRecordFromQuery<QueryExpression>(orgService, SampleQueryExpression);

            Assert.AreEqual(TestAccount1.Id, retrievedAccount.Id);
            Assert.AreEqual(TestAccount1.Name, retrievedAccount.Name);
        }

        [Test(Description = "If multiple results are returned in the QueryExpression, throw an exception as 'throwExceptionOnMultipleResults' defaults to true")]
        public void MultipleResultsThrowsExceptionByDefault()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1,
                TestAccount1Duplicate
            });
            
            Assert.Throws(typeof(Exception),
                () => QueryExtensions.RetrieveRecordFromQuery<QueryExpression>(orgService, SampleQueryExpression));

        }

        [Test(Description = "Multiple results are returned in the QueryExpression, but only the first is returned. No exception as 'throwExceptionOnMultipleResults' is set to false")]
        public void MultipleResultsPicksFirstRecordIfToldTo()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1,
                TestAccount1Duplicate
            });
            
            Assert.DoesNotThrow(() => 
                QueryExtensions.RetrieveRecordFromQuery<QueryExpression>(orgService, SampleQueryExpression, false));

        }

        [Test(Description = "Happy path for RetrieveByFetchExpression. FE describes a single record which is retrieved")]
        public void BasicRetrieveByFetchExpression()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1
            });

            var fetchExpression = new FetchExpression(SampleFetchQuery);
            var retrievedAccount = (Account)QueryExtensions.RetrieveRecordFromQuery<FetchExpression>(orgService, fetchExpression);

            Assert.AreEqual(TestAccount1.Id, retrievedAccount.Id);
            Assert.AreEqual(TestAccount1.Name, retrievedAccount.Name);
        }

        [Test(Description = "If multiple results are returned in the FetchExpression, throw an exception as 'throwExceptionOnMultipleResults' defaults to true")]
        public void RetrieveByFetchExpressionThrowsExceptionWithMultipleRecords()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1,
                TestAccount1Duplicate
            });

            var fetchExpression = new FetchExpression(SampleFetchQuery);

            Assert.Throws(typeof(Exception),
                () => QueryExtensions.RetrieveRecordFromQuery<FetchExpression>(orgService, fetchExpression));
        }

    }
}
