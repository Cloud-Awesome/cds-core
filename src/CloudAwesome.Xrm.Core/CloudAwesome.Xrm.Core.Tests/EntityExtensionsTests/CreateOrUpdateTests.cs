using System;
using System.Collections.Generic;
using NUnit.Framework;
using FakeXrmEasy;
using FluentAssertions;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class CreateOrUpdateTests: BaseFakeXrmTest
    {
        [Test]
        public void CreateIfNoResultsReturned()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var account = new Account()
            {
                Name = "Create Account",
                AccountNumber = "GB123456"
            };

            account.CreateOrUpdate(orgService, SampleAccountQueryExpression);

            TestAccount1.Id.Should().NotBeEmpty();
        }

        [Test]
        public void UpdateIfResultsAreReturned()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                TestAccount1
            });

            TestAccount1.CreateOrUpdate(orgService, SampleAccountQueryExpression);

            TestAccount1.Id.Should().NotBeEmpty();
        }
        
    }
}
