using System.Collections.Generic;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class AssociateTests: BaseFakeXrmTest
    {
        [Test]
        public void BasicAssociateTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.AddRelationship("account_primary_contact", new XrmFakedRelationship()
            {
                RelationshipType = XrmFakedRelationship.enmFakeRelationshipType.OneToMany,
                Entity1LogicalName = "contact",
                Entity1Attribute = "contactid",
                Entity2LogicalName = "account",
                Entity2Attribute = "contactid"
            });

            context.Initialize(new List<Entity>() {
                TestAccount1,
                TestAccount1Duplicate
            });

            var contact = new Contact()
            {
                FirstName = "Jaroslaw",
                LastName = "Czyz"
            };
            contact.Create(orgService);

            var accountCollection = new EntityReferenceCollection()
            {
                TestAccount1.ToEntityReference(),
                TestAccount1Duplicate.ToEntityReference()
            };

            Assert.DoesNotThrow(() =>
                contact.Associate(orgService, "account_primary_contact", accountCollection));

        }

        [Test]
        public void BasicAssociateTestWithEnumerableRecords()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();
            context.AddRelationship("account_primary_contact", new XrmFakedRelationship()
            {
                RelationshipType = XrmFakedRelationship.enmFakeRelationshipType.OneToMany,
                Entity1LogicalName = "contact",
                Entity1Attribute = "contactid",
                Entity2LogicalName = "account",
                Entity2Attribute = "contactid"
            });

            context.Initialize(new List<Entity>() {
                TestAccount1,
                TestAccount1Duplicate
            });

            var contact = new Contact()
            {
                FirstName = "Jaroslaw",
                LastName = "Czyz"
            };
            contact.Create(orgService);

            IEnumerable<Account> accountCollection = new List<Account>()
            {
                TestAccount1,
                TestAccount1Duplicate
            };

            Assert.DoesNotThrow(() =>
                contact.Associate(orgService, "account_primary_contact", accountCollection));

        }
    }
}
