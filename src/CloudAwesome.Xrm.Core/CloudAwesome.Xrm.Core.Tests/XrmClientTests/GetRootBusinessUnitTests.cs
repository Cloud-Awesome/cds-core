﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.XrmClientTests
{
    [TestFixture]
    public class GetRootBusinessUnitTests: BaseFakeXrmTest
    {
        [Test]
        public void GetRootBusinessUnitTest()
        {
            var rootBusinessUnit = new Entity("businessunit")
            {
                Id = Guid.NewGuid(),
                ["name"] = "root", 
                ["parentbusinessunitid"] = null
            };

            var childBusinessUnit1 = new Entity("businessunit")
            {
                Id = Guid.NewGuid(),
                ["name"] = "child 1",
                ["parentbusinessunitid"] = rootBusinessUnit.Id
            };

            var childBusinessUnit2 = new Entity("businessunit")
            {
                Id = Guid.NewGuid(),
                ["name"] = "child 2",
                ["parentbusinessunitid"] = rootBusinessUnit.Id
            };

            XrmContext.Initialize(new List<Entity>()
            {
                rootBusinessUnit,
                childBusinessUnit1,
                childBusinessUnit2
            });

            var retrievedRootBusinessUnit = XrmClient.GetRootBusinessUnit(OrgService);

            retrievedRootBusinessUnit.Id.Should().Be(rootBusinessUnit.Id);

        }
    }
}
