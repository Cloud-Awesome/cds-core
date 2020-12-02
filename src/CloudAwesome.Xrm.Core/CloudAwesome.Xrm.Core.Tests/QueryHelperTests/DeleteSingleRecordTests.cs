using System.Collections.Generic;
using System.Linq;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.QueryHelperTests
{
    [TestFixture]
    public class DeleteSingleRecordTests: BaseFakeXrmTest
    {
        // TODO - What about if the query returns no results?

        [Test]
        public void DeleteSingleRecordTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1
            });

            var preAccounts = (from a in context.CreateQuery<Account>()
                select a).ToList();

            Assert.AreEqual(1, preAccounts.Count);
            Assert.DoesNotThrow(
                () => SampleQueryExpression.DeleteSingleRecord(orgService));

            var postAccounts = (from a in context.CreateQuery<Account>()
                select a).ToList();

            Assert.AreEqual(0, postAccounts.Count);
        }
    }
}
