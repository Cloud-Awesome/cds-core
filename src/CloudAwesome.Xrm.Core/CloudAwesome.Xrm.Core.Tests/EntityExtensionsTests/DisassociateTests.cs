using System.Collections.Generic;
using CloudAwesome.Xrm.Core.Exceptions;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class DisassociateTests: BaseFakeXrmTest
    {
        [Test(Description = "This is not completed due to a bug in execution. For now, it should throw a not implemented exception")]
        public void BasicDisassociateTest()
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

            contact.Associate(orgService, "account_primary_contact", accountCollection);

            Assert.Throws<FeatureRequestException>(() =>
                contact.Disassociate(orgService, "account_primary_contact", accountCollection));

        }

        [Test(Description = "This is not completed due to a bug in execution. For now, it should throw a not implemented exception")]
        public void BasicDisassociateTestWithEnumerableRecords()
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

            contact.Associate(orgService, "account_primary_contact", accountCollection);

            Assert.Throws<FeatureRequestException>(() =>
                contact.Disassociate(orgService, "account_primary_contact", accountCollection));

        }
    }
}
