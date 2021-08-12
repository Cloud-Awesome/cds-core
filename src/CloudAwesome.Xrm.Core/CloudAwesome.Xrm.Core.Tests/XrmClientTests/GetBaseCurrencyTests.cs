using System;
using System.Collections.Generic;
using FakeXrmEasy;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.XrmClientTests
{
    [TestFixture]
    public class GetBaseCurrencyTests
    {
        [Test]
        public void GetBaseCurrencyTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var baseCurrencyId = Guid.NewGuid();
            var baseCurrency = new Entity("organization")
            {
                Id = Guid.NewGuid(),
                ["name"] = "crm-organisation",
                ["basecurrencyid"] = new EntityReference("transactioncurrency", baseCurrencyId)
            };

            context.Initialize(new List<Entity>()
            {
                baseCurrency
            });

            var retrievedBaseCurrency = XrmClient.GetBaseCurrency(orgService);
            
            retrievedBaseCurrency.Id.Should().Be(baseCurrencyId);
        }
    }
}
