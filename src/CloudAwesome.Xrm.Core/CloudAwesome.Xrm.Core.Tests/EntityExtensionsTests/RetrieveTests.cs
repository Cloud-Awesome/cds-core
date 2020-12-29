using System;
using CloudAwesome.Xrm.Core.Exceptions;
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
            var createdEntity = testAccount.Create(orgService);

            var retrieveAccount = new Account {Id = createdEntity.Id};
            var retrievedAccount = retrieveAccount.Retrieve(orgService, new ColumnSet("name"));
            
            Assert.AreEqual(createdEntity.Id, retrievedAccount.Id);
            Assert.AreEqual(testAccount.Name, retrievedAccount["name"]);
            Assert.AreEqual(2, retrievedAccount.Attributes.Count);
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

            Assert.AreEqual(createdEntity.Id, retrievedAccount.Id);
            Assert.IsNotNull(retrievedAccount["createdon"]);
        }

        [Test]
        public void RetrievePassesValidation()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var retrieveAccount = new Account { };
            Assert.Throws(typeof(OperationPreventedException), 
                () => retrieveAccount.Retrieve(orgService, new ColumnSet(true)));
        }
    }
}
