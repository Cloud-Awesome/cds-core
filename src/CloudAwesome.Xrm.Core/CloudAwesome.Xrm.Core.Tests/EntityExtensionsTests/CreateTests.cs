using System;

using NUnit.Framework;
using FakeXrmEasy;

using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class CreateTests
    {
        [Test]
        public void EarlyBoundCreateTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var testAccount = new Account {Name = "Test Account"};
            var createdEntityId = testAccount.Create(orgService);

            Console.WriteLine($"ID of created entity: {createdEntityId}");
            Assert.IsNotNull(createdEntityId);
            Assert.AreEqual("Test Account", testAccount.Name);

        }

        [Test]
        public void LateBoundCreateTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var testAccount = new Entity("account");
            testAccount["name"] = "Test Account";

            var createdEntityId = testAccount.Create(orgService);

            Console.WriteLine($"ID of created entity: {createdEntityId}");
            Assert.IsNotNull(createdEntityId);
            Assert.AreEqual("Test Account", testAccount["name"]);
        }

        [Ignore("Reminder TODO")]
        [Test]
        public void CreateTestWithTracing()
        {
            // TODO - test standard tracing output in Create extension method

            // TODO - if implemented, test AppInsights tracing?
        }
    }
}
