using System;
using System.Collections.Generic;
using System.Linq;
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
            XrmContext.Initialize(new List<Entity>() {
                TestAccount1
            });

            var preAccounts = (from a in XrmContext.CreateQuery<Account>()
                select a).ToList();

            Action preAction = () => SampleAccountQueryExpression.DeleteSingleRecord(OrgService);
            preAccounts.Count.Should().Be(1);
            preAction.Should().NotThrow();

            var postAccounts = (from a in XrmContext.CreateQuery<Account>()
                select a).ToList();

            postAccounts.Count.Should().Be(0);
        }

        [Test]
        public void DeleteWithNoQueryResultsThrowsNoError()
        {
            Action action = () => SampleAccountQueryExpression.DeleteSingleRecord(OrgService);
            action.Should().NotThrow();
        }
    }
}
