using System.Collections.Generic;
using FakeXrmEasy;
using FluentAssertions;
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

            retrievedAccount.Should().BeNull();
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

            retrievedAccount.Id.Should().Be(TestAccount1.Id);
            retrievedAccount.Name.Should().Be(TestAccount1.Name);
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

            retrievedAccount.Id.Should().Be(TestAccount1.Id);
            retrievedAccount.Name.Should().Be(TestAccount1.Name);
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

            retrievedAccounts.Entities.Count.Should().Be(2);
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

            retrievedAccounts.Entities.Count.Should().Be(2);
        }
    }
}
