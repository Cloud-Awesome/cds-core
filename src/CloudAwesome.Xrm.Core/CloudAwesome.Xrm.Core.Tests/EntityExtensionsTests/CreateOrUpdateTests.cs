using System.Collections.Generic;
using NUnit.Framework;
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
            var account = new Account()
            {
                Name = "Create Account",
                AccountNumber = "GB123456"
            };

            account.CreateOrUpdate(OrgService, SampleAccountQueryExpression);

            TestAccount1.Id.Should().NotBeEmpty();
        }

        [Test]
        public void UpdateIfResultsAreReturned()
        {
            XrmContext.Initialize(new List<Entity>()
            {
                TestAccount1
            });

            TestAccount1.CreateOrUpdate(OrgService, SampleAccountQueryExpression);

            TestAccount1.Id.Should().NotBeEmpty();
        }
        
    }
}
