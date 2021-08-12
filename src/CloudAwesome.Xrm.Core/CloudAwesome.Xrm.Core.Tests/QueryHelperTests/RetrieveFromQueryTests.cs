using System;
using System.Collections.Generic;
using CloudAwesome.Xrm.Core.Exceptions;
using FakeXrmEasy;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using NUnit.Framework.Internal;

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

            var retrievedAccount = (Account) QueryExtensions.RetrieveRecordFromQuery<QueryByAttribute>(orgService, SampleAccountQueryByAttribute);

            retrievedAccount.Id.Should().Be(TestAccount1.Id);
            retrievedAccount.Name.Should().Be(TestAccount1.Name);
        }

        [Test(Description = "Happy path for RetrieveByQueryExpression. QE describes a single record which is retrieved")]
        public void BasicRetrieveByQueryExpression()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1
            });
            
            var retrievedAccount = (Account) QueryExtensions.RetrieveRecordFromQuery<QueryExpression>(orgService, SampleAccountQueryExpression);

            retrievedAccount.Id.Should().Be(TestAccount1.Id);
            retrievedAccount.Name.Should().Be(TestAccount1.Name);
        }

        [Test(Description = "QE describes no records so null is returned")]
        public void QueryWithResultsReturnsNull()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var retrievedAccount = (Account)QueryExtensions.RetrieveRecordFromQuery<QueryExpression>(orgService, SampleAccountQueryExpression);

            retrievedAccount.Should().BeNull();
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
            
            Action action = () =>
                QueryExtensions.RetrieveRecordFromQuery<QueryExpression>(orgService, SampleAccountQueryExpression);
            action.Should().Throw<QueryBaseException>();

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
            
            Action action = () =>
                QueryExtensions.RetrieveRecordFromQuery<QueryExpression>(orgService, SampleAccountQueryExpression,
                    false);
            action.Should().NotThrow();
        }

        [Test(Description = "Happy path for RetrieveByFetchExpression. FE describes a single record which is retrieved")]
        public void BasicRetrieveByFetchExpression()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1
            });

            var fetchExpression = new FetchExpression(SampleAccountFetchQuery);
            var retrievedAccount = (Account)QueryExtensions.RetrieveRecordFromQuery<FetchExpression>(orgService, fetchExpression);

            retrievedAccount.Id.Should().Be(TestAccount1.Id);
            retrievedAccount.Name.Should().Be(TestAccount1.Name);
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

            var fetchExpression = new FetchExpression(SampleAccountFetchQuery);

            Action action = () => 
                QueryExtensions.RetrieveRecordFromQuery<FetchExpression>(orgService, fetchExpression);
            action.Should().Throw<QueryBaseException>();
        }

    }
}
