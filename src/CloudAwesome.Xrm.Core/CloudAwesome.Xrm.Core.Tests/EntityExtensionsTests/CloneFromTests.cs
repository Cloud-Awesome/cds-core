using System;
using System.Collections.Generic;
using CloudAwesome.Xrm.Core.Exceptions;
using NUnit.Framework;
using FakeXrmEasy;
using FluentAssertions;
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
            var createdSourceAccount = sourceAccount.Create(orgService);

            var targetAccount = new Account();
            targetAccount.CloneFrom(sourceAccount);
            var createdTargetAccount = targetAccount.Create(orgService);

            createdTargetAccount.Id.Should().NotBeEmpty();
            createdSourceAccount.Id.Should().NotBe(createdTargetAccount.Id);

            targetAccount.Name.Should().Be(sourceAccount.Name);
            targetAccount.AccountCategoryCode.Should().Be(sourceAccount.AccountCategoryCode);
            targetAccount.LastOnHoldTime.Should().Be(sourceAccount.LastOnHoldTime);
            targetAccount.NumberOfEmployees.Should().Be(sourceAccount.NumberOfEmployees);
        }

        [Test]
        public void CannotCloneDifferenceEntitiesTest()
        {
            var sourceAccount = new Account()
            {
                Name = "Source Account"
            };
            var targetContact = new Contact();

            Action action = () => targetContact.CloneFrom(sourceAccount);
            action.Should().Throw<OperationPreventedException>();
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

            targetAccount.Name.Should().Be(sourceAccount.Name);

            targetAccount.AccountNumber.Should().NotBe(sourceAccount.AccountNumber);
            targetAccount.ParentAccountId.Should().NotBe(sourceAccount.ParentAccountId);
            targetAccount.LastOnHoldTime.HasValue.Should().Be(false);
            targetAccount.NumberOfEmployees.Should().NotBe(sourceAccount.NumberOfEmployees);

        }
    }
}
