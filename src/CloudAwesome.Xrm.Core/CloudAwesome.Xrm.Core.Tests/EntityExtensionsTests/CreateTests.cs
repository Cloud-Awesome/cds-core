using System;
using NUnit.Framework;
using FakeXrmEasy;
using CloudAwesome.Xrm.Core;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class CreateTests
    {
        [Test]
        public void BasicCreateTest()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var testAccount = new Account {Name = "Test Account"};
            var createdEntity = testAccount.Create(orgService);

            Console.WriteLine($"ID of created entity: {createdEntity}");
            Assert.IsNotNull(createdEntity);

        }
    }
}
