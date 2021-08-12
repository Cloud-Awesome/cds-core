using System;
using CloudAwesome.Xrm.Core.Exceptions;
using NUnit.Framework;
using FakeXrmEasy;
using FluentAssertions;
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
            var createdEntity = testAccount.Create(orgService);

            Console.WriteLine($"ID of created entity: {createdEntity.Id}");

            createdEntity.Id.Should().NotBeEmpty();
            testAccount.Name.Should().Be("Test Account");
        }

        [Test]
        public void FailCreateAndThrowValidExceptionIfLogicalNameNotProvided()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var testSomething = new Entity();
            testSomething["name"] = "Unknown Entity";

            Action action = () => testSomething.Create(orgService);
            action.Should().Throw<OperationPreventedException>();
        }

        [Test]
        public void LateBoundCreateTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var testAccount = new Entity("account");
            testAccount["name"] = "Test Account";

            var createdEntity = testAccount.Create(orgService);

            Console.WriteLine($"ID of created entity: {createdEntity.Id}");
            
            createdEntity.Id.Should().NotBeEmpty();
            testAccount["name"].Should().Be("Test Account");
        }

    }
}
