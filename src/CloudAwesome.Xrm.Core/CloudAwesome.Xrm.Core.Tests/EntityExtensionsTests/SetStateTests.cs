using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using FakeXrmEasy;
using CloudAwesome.Xrm.Core;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Core.Tests.EntityExtensionsTests
{
    [TestFixture]
    public class SetStateTests: BaseFakeXrmTest
    {
        [Test(Description = "This is not implemented as not required and possibly deprecated. For now, it should throw a not implemented exception")]
        public void TempSetStateShouldThrowNotImplementedException()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var testAccount = new Account()
            {
                Name = "Test SetState Account"
            };

            Assert.Throws<FeatureRequestException>(
                () => testAccount.SetState(orgService, 1, 1));
        }
    }
}
