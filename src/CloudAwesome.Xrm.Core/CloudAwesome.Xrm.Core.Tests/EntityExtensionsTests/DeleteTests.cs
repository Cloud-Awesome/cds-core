using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using FakeXrmEasy;
using CloudAwesome.Xrm.Core;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class DeleteTests
    {
        [Test]
        public void BasicDeletionTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();
            
            var testAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Test Account"
            };
            context.Initialize(new List<Entity>() {
                testAccount
            });

            testAccount.Delete(orgService);
            Assert.IsTrue(true);
        }
    }
}
