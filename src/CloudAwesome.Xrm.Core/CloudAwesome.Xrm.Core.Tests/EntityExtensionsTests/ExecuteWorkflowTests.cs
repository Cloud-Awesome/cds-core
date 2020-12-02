using System;

using NUnit.Framework;
using FakeXrmEasy;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class ExecuteWorkflowTests
    {
        [Test(Description = "This is not completed due to a bug in unit test. For now, it should throw a partially implemented exception")]
        public void BasicExecuteWorkflowTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var workflowId = Guid.NewGuid();

            var testAccount = new Account { Name = "Test Account" };
            var createdEntityId = testAccount.Create(orgService);

            Assert.Throws<FeatureRequestException>(
                () => testAccount.ExecuteWorkflow(orgService, workflowId));
        }
    }
}
