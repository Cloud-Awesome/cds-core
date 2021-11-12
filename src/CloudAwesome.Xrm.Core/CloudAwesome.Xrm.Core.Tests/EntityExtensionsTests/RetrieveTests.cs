using System;
using CloudAwesome.Xrm.Core.Exceptions;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class RetrieveTests: BaseFakeXrmTest
    {
        [Test]
        public void RetrieveEntitySetColumnsTest()
        {
            var testAccount = new Account { Name = "Test Account" };
            var createdEntity = testAccount.Create(OrgService);

            var retrieveAccount = new Account {Id = createdEntity.Id};
            var retrievedAccount = retrieveAccount.Retrieve(OrgService, new ColumnSet("name"));
            
            retrievedAccount.Id.Should().Be(createdEntity.Id);
            retrievedAccount["name"].Should().Be(testAccount.Name);
            retrievedAccount.Attributes.Count.Should().Be(2);
        }

        [Test]
        public void RetrieveAllColumnsTest()
        {
            var testAccount = new Account { Name = "Test Account" };
            var createdEntity = testAccount.Create(OrgService);

            var retrieveAccount = new Account { Id = createdEntity.Id };
            var retrievedAccount = retrieveAccount.Retrieve(OrgService, new ColumnSet(true));

            retrieveAccount.Id.Should().Be(createdEntity.Id);
            retrievedAccount["name"].Should().NotBeNull();
        }

        [Test]
        public void RetrievePassesValidation()
        {
            var retrieveAccount = new Account { };

            Action action = () => retrieveAccount.Retrieve(OrgService, new ColumnSet(true));
            action.Should().Throw<OperationPreventedException>();
        }
    }
}
