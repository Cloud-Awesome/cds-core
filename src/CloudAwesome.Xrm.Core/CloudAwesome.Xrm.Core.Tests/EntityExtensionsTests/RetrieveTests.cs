using System;
using CloudAwesome.Xrm.Core.Exceptions;
using NUnit.Framework;
using FakeXrmEasy;
using FluentAssertions;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class RetrieveTests
    {
        [Test]
        public void RetrieveEntitySetColumnsTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var testAccount = new Account { Name = "Test Account" };
            var createdEntity = testAccount.Create(orgService);

            var retrieveAccount = new Account {Id = createdEntity.Id};
            var retrievedAccount = retrieveAccount.Retrieve(orgService, new ColumnSet("name"));
            
            retrievedAccount.Id.Should().Be(createdEntity.Id);
            retrievedAccount["name"].Should().Be(testAccount.Name);
            retrievedAccount.Attributes.Count.Should().Be(2);
        }

        [Test]
        public void RetrieveAllColumnsTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var testAccount = new Account { Name = "Test Account" };
            var createdEntity = testAccount.Create(orgService);

            var retrieveAccount = new Account { Id = createdEntity.Id };
            var retrievedAccount = retrieveAccount.Retrieve(orgService, new ColumnSet(true));

            retrieveAccount.Id.Should().Be(createdEntity.Id);
            retrievedAccount["name"].Should().NotBeNull();
        }

        [Test]
        public void RetrievePassesValidation()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var retrieveAccount = new Account { };

            Action action = () => retrieveAccount.Retrieve(orgService, new ColumnSet(true));
            action.Should().Throw<OperationPreventedException>();
        }
    }
}
