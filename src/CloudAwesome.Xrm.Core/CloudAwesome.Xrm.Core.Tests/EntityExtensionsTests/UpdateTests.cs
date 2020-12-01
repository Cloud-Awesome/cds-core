using System;

using NUnit.Framework;
using FakeXrmEasy;

using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class UpdateTests
    {
        [Test]
        public void EarlyBoundUpdateTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var testAccount = new Account { Name = "Test Account" };
            var createdEntityId = testAccount.Create(orgService);

            testAccount.Name = "Updated Account";
            testAccount.Update(orgService);

            Console.WriteLine($"ID of created entity: {createdEntityId}");
            Assert.IsNotNull(createdEntityId);
            Assert.AreEqual("Updated Account", testAccount.Name);
        }

        [Test]
        public void LateBoundCreateTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var testAccount = new Entity("account");
            testAccount["name"] = "Test Account";
            var createdEntityId = testAccount.Create(orgService);

            testAccount["name"] = "Updated Account";

            Console.WriteLine($"ID of created entity: {createdEntityId}");
            Assert.IsNotNull(createdEntityId);
            Assert.AreEqual("Updated Account", testAccount["name"]);
        }
    }
}
