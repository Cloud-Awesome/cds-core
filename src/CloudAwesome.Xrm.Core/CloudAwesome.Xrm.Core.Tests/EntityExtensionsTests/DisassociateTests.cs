using System;
using System.Collections.Generic;
using CloudAwesome.Xrm.Core.Exceptions;
using FakeXrmEasy;
using FluentAssertions;
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
            XrmContext.AddRelationship("account_primary_contact", new XrmFakedRelationship()
            {
                RelationshipType = XrmFakedRelationship.enmFakeRelationshipType.OneToMany,
                Entity1LogicalName = "contact",
                Entity1Attribute = "contactid",
                Entity2LogicalName = "account",
                Entity2Attribute = "contactid"
            });

            XrmContext.Initialize(new List<Entity>() {
                TestAccount1,
                TestAccount1Duplicate
            });

            var contact = new Contact()
            {
                FirstName = "Jaroslaw",
                LastName = "Czyz"
            };
            contact.Create(OrgService);

            var accountCollection = new EntityReferenceCollection()
            {
                TestAccount1.ToEntityReference(),
                TestAccount1Duplicate.ToEntityReference()
            };

            contact.Associate(OrgService, "account_primary_contact", accountCollection);

            Action action = () =>
                contact.Disassociate(OrgService, "account_primary_contact", accountCollection);
            action.Should().Throw<FeatureRequestException>();

        }

        [Test(Description = "This is not completed due to a bug in execution. For now, it should throw a not implemented exception")]
        public void BasicDisassociateTestWithEnumerableRecords()
        {
            XrmContext.AddRelationship("account_primary_contact", new XrmFakedRelationship()
            {
                RelationshipType = XrmFakedRelationship.enmFakeRelationshipType.OneToMany,
                Entity1LogicalName = "contact",
                Entity1Attribute = "contactid",
                Entity2LogicalName = "account",
                Entity2Attribute = "contactid"
            });

            XrmContext.Initialize(new List<Entity>() {
                TestAccount1,
                TestAccount1Duplicate
            });

            var contact = new Contact()
            {
                FirstName = "Jaroslaw",
                LastName = "Czyz"
            };
            contact.Create(OrgService);

            IEnumerable<Account> accountCollection = new List<Account>()
            {
                TestAccount1,
                TestAccount1Duplicate
            };

            contact.Associate(OrgService, "account_primary_contact", accountCollection);

            Action action = () =>
                contact.Disassociate(OrgService, "account_primary_contact", accountCollection);
            action.Should().Throw<FeatureRequestException>();
        }
    }
}
