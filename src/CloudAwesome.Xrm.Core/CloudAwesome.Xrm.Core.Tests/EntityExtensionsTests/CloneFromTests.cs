using System;
using System.Collections.Generic;
using CloudAwesome.Xrm.Core.Exceptions;
using NUnit.Framework;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class CloneFromTests
    {
        [Test]
        public void BasicCloneFromTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();


            var sourceAccount = new Account()
            {
                Name = "Source Account",
                AccountCategoryCode = new OptionSetValue(1),
                AccountNumber = "GB123456",
                ParentAccountId = new EntityReference("account", Guid.NewGuid()),
                LastOnHoldTime = DateTime.Now,
                NumberOfEmployees = 1999
            };
            var createdSourceAccountId = sourceAccount.Create(orgService);

            var targetAccount = new Account();
            targetAccount.CloneFrom(sourceAccount);
            var createdTargetAccountId = targetAccount.Create(orgService);

            Assert.That(createdTargetAccountId, Is.Not.Null);
            Assert.That(createdSourceAccountId, Is.Not.EqualTo(createdTargetAccountId));

            Assert.That(sourceAccount.Name, Is.EqualTo(targetAccount.Name));
            Assert.That(sourceAccount.AccountCategoryCode, Is.EqualTo(targetAccount.AccountCategoryCode));
            Assert.That(sourceAccount.LastOnHoldTime, Is.EqualTo(targetAccount.LastOnHoldTime));
            Assert.That(sourceAccount.NumberOfEmployees, Is.EqualTo(targetAccount.NumberOfEmployees));
        }

        [Test]
        public void CannotCloneDifferenceEntitiesTest()
        {
            var sourceAccount = new Account()
            {
                Name = "Source Account"
            };
            var targetContact = new Contact();

            Assert.Throws(typeof(OperationPreventedException), () => targetContact.CloneFrom(sourceAccount));
        }

        [Test]
        public void EnsureExcludedAttributesFromCloneTest()
        {
            var sourceAccount = new Account()
            {
                Name = "Source Account",
                AccountCategoryCode = new OptionSetValue(1),
                AccountNumber = "GB123456",
                ParentAccountId = new EntityReference("account", Guid.NewGuid()),
                LastOnHoldTime = DateTime.Now,
                NumberOfEmployees = 1999
            };

            var targetAccount = new Account();
            var excludedAttributes = new List<string>()
            {
                "accountnumber",
                "parentaccountid",
                "lastonholdtime",
                "numberofemployees"
            };

            targetAccount.CloneFrom(sourceAccount, excludedAttributes);

            Assert.AreEqual(sourceAccount.Name, targetAccount.Name);

            Assert.AreNotEqual(sourceAccount.AccountNumber, targetAccount.AccountNumber);
            Assert.AreNotEqual(sourceAccount.ParentAccountId, targetAccount.ParentAccountId);
            Assert.AreNotEqual(sourceAccount.LastOnHoldTime, targetAccount.LastOnHoldTime);
            Assert.AreNotEqual(sourceAccount.NumberOfEmployees, targetAccount.NumberOfEmployees);
        }
    }
}
