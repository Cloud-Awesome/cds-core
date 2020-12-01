using System;
using System.Collections.Generic;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.QueryHelperTests
{
    [TestFixture]
    public class RetrieveFromQueryTests
    {
        [Test(Description = "Happy path for RetrieveByAttribute. Query describes a single record which is retrieved")]
        public void BasicRetrieveByQueryByAttribute()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                _testAccount1
            });

            var retrievedAccount = (Account) QueryHelpers.RetrieveRecordFromQuery<QueryByAttribute>(orgService, _queryByAttribute);

            Assert.AreEqual(_testAccount1.Id, retrievedAccount.Id);
            Assert.AreEqual(_testAccount1.Name, retrievedAccount.Name);
        }

        [Test(Description = "Happy path for RetrieveByQueryExpression. QE describes a single record which is retrieved")]
        public void BasicRetrieveByQueryExpression()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                _testAccount1
            });
            
            var retrievedAccount = (Account) QueryHelpers.RetrieveRecordFromQuery<QueryExpression>(orgService, _queryExpression);

            Assert.AreEqual(_testAccount1.Id, retrievedAccount.Id);
            Assert.AreEqual(_testAccount1.Name, retrievedAccount.Name);
        }

        [Test(Description = "If multiple results are returned in the QueryExpression, throw an exception as 'throwExceptionOnMultipleResults' defaults to true")]
        public void MultipleResultsThrowsExceptionByDefault()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                _testAccount1,
                _testAccount1Duplicate
            });
            
            Assert.Throws(typeof(Exception),
                () => QueryHelpers.RetrieveRecordFromQuery<QueryExpression>(orgService, _queryExpression));

        }

        [Test(Description = "Multiple results are returned in the QueryExpression, but only the first is returned. No exception as 'throwExceptionOnMultipleResults' is set to false")]
        public void MultipleResultsPicksFirstRecordIfToldTo()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                _testAccount1,
                _testAccount1Duplicate
            });
            
            Assert.DoesNotThrow(() => 
                QueryHelpers.RetrieveRecordFromQuery<QueryExpression>(orgService, _queryExpression, false));

        }

        [Test(Description = "Happy path for RetrieveByFetchExpression. FE describes a single record which is retrieved")]
        public void BasicRetrieveByFetchExpression()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                _testAccount1
            });

            var fetchExpression = new FetchExpression(_fetchQuery);
            var retrievedAccount = (Account)QueryHelpers.RetrieveRecordFromQuery<FetchExpression>(orgService, fetchExpression);

            Assert.AreEqual(_testAccount1.Id, retrievedAccount.Id);
            Assert.AreEqual(_testAccount1.Name, retrievedAccount.Name);
        }

        [Test(Description = "If multiple results are returned in the FetchExpression, throw an exception as 'throwExceptionOnMultipleResults' defaults to true")]
        public void RetrieveByFetchExpressionThrowsExceptionWithMultipleRecords()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                _testAccount1,
                _testAccount1Duplicate
            });

            var fetchExpression = new FetchExpression(_fetchQuery);

            Assert.Throws(typeof(Exception),
                () => QueryHelpers.RetrieveRecordFromQuery<FetchExpression>(orgService, fetchExpression));
        }

        [Test(Description = "Extension method to retrieve single record from QueryBase (QueryExpression test)")]
        public void QueryExpressionExtensionMethod()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                _testAccount1
            });

            var retrievedAccount = (Account)_queryExpression.RetrieveSingleRecord(orgService);

            Assert.AreEqual(_testAccount1.Id, retrievedAccount.Id);
            Assert.AreEqual(_testAccount1.Name, retrievedAccount.Name);
        }

        [Test(Description = "Extension method to retrieve single record from QueryBase (QueryByAttribute test)")]
        public void QueryByAttributeExtensionMethod()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                _testAccount1
            });

            var retrievedAccount = (Account)_queryByAttribute.RetrieveSingleRecord(orgService);

            Assert.AreEqual(_testAccount1.Id, retrievedAccount.Id);
            Assert.AreEqual(_testAccount1.Name, retrievedAccount.Name);
        }


        #region Query definitions and test data

        private static readonly Account _testAccount1 = new Account
        {
            Id = Guid.NewGuid(),
            Name = "Test Account 1",
            AccountNumber = "GB123456"
        };

        private static readonly Account _testAccount1Duplicate = new Account
        {
            Id = Guid.NewGuid(),
            Name = "Test Account 1 Duplicate",
            AccountNumber = "GB123456"
        };

        private readonly string _fetchQuery =
            @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                  <entity name=""account"">
                    <attribute name=""accountid"" />
                    <attribute name=""name"" />
                    <attribute name=""accountnumber"" />
                    <order attribute=""name"" descending=""false"" />
                    <filter type=""and"">
                      <condition attribute=""accountnumber"" operator=""eq"" value=""GB123456"" />
                    </filter>
                  </entity>
                </fetch>";

        private readonly QueryExpression _queryExpression = new QueryExpression()
        {
            EntityName = "account",
            ColumnSet = new ColumnSet("name", "accountnumber"),
            Criteria = new FilterExpression()
            {
                Conditions =
                {
                    new ConditionExpression("accountnumber", ConditionOperator.Equal, _testAccount1.AccountNumber)
                }
            }
        };

        private readonly QueryByAttribute _queryByAttribute = new QueryByAttribute()
        {
            EntityName = "account",
            ColumnSet = new ColumnSet("name", "accountnumber"),
            Attributes =
            {
                "accountnumber"
            },
            Values =
            {
                "GB123456"
            }
        };

        #endregion #region Query definitions and test data

    }
}
