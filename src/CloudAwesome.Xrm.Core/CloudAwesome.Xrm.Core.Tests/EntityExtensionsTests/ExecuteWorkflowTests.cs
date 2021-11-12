using System;
using CloudAwesome.Xrm.Core.Exceptions;
using NUnit.Framework;
using FluentAssertions;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class ExecuteWorkflowTests: BaseFakeXrmTest
    {
        [Test(Description = "This is not completed due to a bug in unit test. For now, it should throw a partially implemented exception")]
        public void BasicExecuteWorkflowTest()
        {
            var workflowId = Guid.NewGuid();

            var testAccount = new Account { Name = "Test Account" };
            var createdEntity = testAccount.Create(OrgService);

            Action action = () => testAccount.ExecuteWorkflow(OrgService, workflowId);
            action.Should().Throw<FeatureRequestException>();
        }
    }
}
