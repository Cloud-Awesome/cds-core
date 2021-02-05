using System;
using CloudAwesome.Xrm.Core.Exceptions;
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
        public void FailCreateAndThrowValidExceptionIfLogicalNameNotProvided()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var testSomething = new Entity();
            testSomething["name"] = "Unknown Entity";

            Assert.Throws<OperationPreventedException>(() => testSomething.Create(orgService));
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

    }
}
