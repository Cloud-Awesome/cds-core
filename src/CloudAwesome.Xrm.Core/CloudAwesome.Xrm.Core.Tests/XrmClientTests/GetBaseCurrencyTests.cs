using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.XrmClientTests
{
    [TestFixture]
    public class GetBaseCurrencyTests: BaseFakeXrmTest
    {
        [Test]
        public void GetBaseCurrencyTest()
        {
            var baseCurrencyId = Guid.NewGuid();
            var baseCurrency = new Entity("organization")
            {
                Id = Guid.NewGuid(),
                ["name"] = "crm-organisation",
                ["basecurrencyid"] = new EntityReference("transactioncurrency", baseCurrencyId)
            };

            XrmContext.Initialize(new List<Entity>()
            {
                baseCurrency
            });

            var retrievedBaseCurrency = XrmClient.GetBaseCurrency(OrgService);
            
            retrievedBaseCurrency.Id.Should().Be(baseCurrencyId);
        }
    }
}
