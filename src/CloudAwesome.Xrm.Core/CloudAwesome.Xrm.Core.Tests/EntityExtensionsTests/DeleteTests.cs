using System;
using System.Collections.Generic;
using NUnit.Framework;
using CloudAwesome.Xrm.Core.Exceptions;
using FluentAssertions;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class DeleteTests: BaseFakeXrmTest
    {
        [Test]
        public void BasicDeletionTest()
        {
            var testAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Test Account"
            };
            XrmContext.Initialize(new List<Entity>() {
                testAccount
            });

            testAccount.Delete(OrgService);
            Assert.IsTrue(true);
            
        }

        [Test]
        public void FailDeleteAndThrowValidExceptionIfPrimaryGuidIsEmpty()
        {
            var testAccount = new Account(){ Name = "Test Account"};

            Action action = () => testAccount.Delete(OrgService);
            action.Should().Throw<OperationPreventedException>();
        }
    }
}
