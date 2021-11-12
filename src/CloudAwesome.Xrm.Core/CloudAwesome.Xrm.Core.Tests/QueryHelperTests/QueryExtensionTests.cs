using System.Collections.Generic;
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
            var retrievedAccount = (Account)SampleAccountQueryExpression.RetrieveSingleRecord(OrgService);
            retrievedAccount.Should().BeNull();
        }

        [Test(Description = "Extension method to retrieve single record from QueryBase (QueryExpression test)")]
        public void QueryExpressionSingleRecordExtensionMethod()
        {
            XrmContext.Initialize(new List<Entity>() {
                TestAccount1
            });

            var retrievedAccount = (Account)SampleAccountQueryExpression.RetrieveSingleRecord(OrgService);

            retrievedAccount.Id.Should().Be(TestAccount1.Id);
            retrievedAccount.Name.Should().Be(TestAccount1.Name);
        }

        [Test(Description = "Extension method to retrieve single record from QueryBase (QueryByAttribute test)")]
        public void QueryByAttributeSingleRecordExtensionMethod()
        {
            XrmContext.Initialize(new List<Entity>() {
                TestAccount1
            });

            var retrievedAccount = (Account)SampleAccountQueryByAttribute.RetrieveSingleRecord(OrgService);

            retrievedAccount.Id.Should().Be(TestAccount1.Id);
            retrievedAccount.Name.Should().Be(TestAccount1.Name);
        }

        [Test(Description = "Extension method to retrieve multiple records from Query Base (QueryExpression test)")]
        public void QueryExpressionEntityCollectionExtensionMethod()
        {
            XrmContext.Initialize(new List<Entity>() {
                TestAccount1,
                TestAccount1Duplicate
            });

            var retrievedAccounts = SampleAccountQueryExpression.RetrieveMultiple(OrgService);

            retrievedAccounts.Entities.Count.Should().Be(2);
        }

        [Test(Description = "Extension method to retrieve multiple records from Query Base (QueryByAttribute test)")]
        public void QueryByAttributeEntityCollectionExtensionMethod()
        {
            XrmContext.Initialize(new List<Entity>() {
                TestAccount1,
                TestAccount1Duplicate
            });

            var retrievedAccounts = SampleAccountQueryByAttribute.RetrieveMultiple(OrgService);

            retrievedAccounts.Entities.Count.Should().Be(2);
        }
    }
}
