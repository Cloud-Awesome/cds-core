using System;
using CloudAwesome.Xrm.Core.Exceptions;
using NUnit.Framework;
using FakeXrmEasy;
using FluentAssertions;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class UpdateTests: BaseFakeXrmTest
    {
        [Test]
        public void EarlyBoundUpdateTest()
        {
            var testAccount = new Account { Name = "Test Account" };
            var createdEntity = testAccount.Create(OrgService);

            testAccount.Name = "Updated Account";
            testAccount.Update(OrgService);

            Console.WriteLine($"ID of created entity: {createdEntity.Id}");
            
            createdEntity.Id.Should().NotBeEmpty();
            testAccount.Name.Should().Be("Updated Account");
        }

        [Test]
        public void FailUpdateAndThrowValidExceptionIfPrimaryGuidIsEmpty()
        {   
            var testAccount = new Account { Name = "Test Account" };

            Action action = () => testAccount.Update(OrgService);
            action.Should().Throw<OperationPreventedException>();
        }

        [Test]
        public void LateBoundCreateTest()
        {
            var testAccount = new Entity("account");
            testAccount["name"] = "Test Account";
            var createdEntity = testAccount.Create(OrgService);

            testAccount["name"] = "Updated Account";

            Console.WriteLine($"ID of created entity: {createdEntity.Id}");
            
            createdEntity.Id.Should().NotBeEmpty();
            testAccount["name"].Should().Be("Updated Account");
        }
    }
}
