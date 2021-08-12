using System;
using System.Collections.Generic;
using System.Linq;
using FakeXrmEasy;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.QueryHelperTests
{
    [TestFixture]
    public class DeleteSingleRecordTests: BaseFakeXrmTest
    {
        [Test]
        public void DeleteSingleRecordTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>() {
                TestAccount1
            });

            var preAccounts = (from a in context.CreateQuery<Account>()
                select a).ToList();

            Action preAction = () => SampleAccountQueryExpression.DeleteSingleRecord(orgService);
            preAccounts.Count.Should().Be(1);
            preAction.Should().NotThrow();

            var postAccounts = (from a in context.CreateQuery<Account>()
                select a).ToList();

            postAccounts.Count.Should().Be(0);
        }

        [Test]
        public void DeleteWithNoQueryResultsThrowsNoError()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            Action action = () => SampleAccountQueryExpression.DeleteSingleRecord(orgService);
            action.Should().NotThrow();
        }
    }
}
