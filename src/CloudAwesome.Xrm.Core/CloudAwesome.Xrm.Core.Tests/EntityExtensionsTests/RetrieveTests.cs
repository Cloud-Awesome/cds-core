using System;
using NUnit.Framework;
using FakeXrmEasy;
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
            var createdEntityId = testAccount.Create(orgService);

            var retrieveAccount = new Account {Id = createdEntityId};
            var retrievedAccount = retrieveAccount.Retrieve(orgService, new ColumnSet("name"));
            
            Assert.AreEqual(createdEntityId, retrievedAccount.Id);
            Assert.AreEqual(testAccount.Name, retrievedAccount["name"]);
            Assert.AreEqual(2, retrievedAccount.Attributes.Count);
        }

        [Test]
        public void RetrieveAllColumnsTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var testAccount = new Account { Name = "Test Account" };
            var createdEntityId = testAccount.Create(orgService);

            var retrieveAccount = new Account { Id = createdEntityId };
            var retrievedAccount = retrieveAccount.Retrieve(orgService, new ColumnSet(true));

            Assert.AreEqual(createdEntityId, retrievedAccount.Id);
            Assert.IsNotNull(retrievedAccount["createdon"]);
        }

        [Test]
        public void RetrievePassesValidation()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var retrieveAccount = new Account { };
            Assert.Throws(typeof(Exception), 
                () => retrieveAccount.Retrieve(orgService, new ColumnSet(true)));
        }
    }
}
