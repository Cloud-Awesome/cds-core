using System;
using System.Collections.Generic;
using System.Linq;
using CloudAwesome.Xrm.Core.Exceptions;
using FakeXrmEasy;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.QueryHelperTests
{
    [TestFixture]
    public class DeleteAllResultsTests: BaseFakeXrmTest
    {
        [Test(Description = "All validation passes and all records returned in the query are deleted.")]
        public void DeleteAllHappyPath()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                JohnContact,
                JamieContact,
                JacobContact,
                JeremyContact,
                JamesContact // James isn't in London
            });

            var preContacts = (from a in context.CreateQuery<Contact>()
                               where a.Address1_City == "London"
                                select a).ToList();

            Assert.AreEqual(4, preContacts.Count);
            Assert.DoesNotThrow(() => SampleLondonContactsQueryByAttribute.DeleteAllResults(orgService));

            var postContacts = (from a in context.CreateQuery<Contact>()
                where a.Address1_City == "London"
                select a).ToList();
            
            postContacts.Count.Should().Be(0);
        }

        [Test(Description = "The threshold of how many records to delete is exceeded in the results returned by the query. Exception is thrown.")]
        public void RecordCountThresholdPassedExceptionThrown()
        {
            var expectedExceptionMessage = $"DeleteAllResults query returned too many results to proceed. " +
                                           $"Threshold was set to 2";

            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                JohnContact,
                JamieContact,
                JacobContact,
                JeremyContact
            });

            Assert.Throws(Is.TypeOf<QueryBaseException>().And.Message.EqualTo(expectedExceptionMessage),
                () => SampleLondonContactsQueryByAttribute.DeleteAllResults(orgService, 2)
            );

            // Assert that nothing's been deleted
            var postContacts = (from a in context.CreateQuery<Contact>()
                where a.Address1_City == "London"
                select a).ToList();
            
            postContacts.Count.Should().Be(4);
        }

        [Test(Description = "The expected number of records are returned in the query. All are deleted.")]
        public void ExpectedNumberOfResultsAreReturnedAllAreDeleted()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                JohnContact,
                JamieContact,
                JamesContact // James isn't in London
            });

            var preContacts = (from a in context.CreateQuery<Contact>()
                where a.Address1_City == "London"
                select a).ToList();

            preContacts.Count.Should().Be(2);
            Action preAction = () =>
                SampleLondonContactsQueryByAttribute.DeleteAllResults(orgService, expectedResultsToDelete: 2);
            preAction.Should().NotThrow();

            var postContacts = (from a in context.CreateQuery<Contact>()
                where a.Address1_City == "London"
                select a).ToList();
            
            postContacts.Count.Should().Be(0);
        }

        [Test(Description = "The expected number of records is not returned in the query. Nothing deleted and exception is thrown.")]
        public void ExpectedNumberOfResultsNotReturnedNoneAreDeleted()
        {
            var expectedExceptionMessage = $"Could not safely delete results of query. " +
                                           $"Expected 2 but actual was 3";

            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                JohnContact,
                JamieContact,
                JacobContact,
                JamesContact // James isn't in London
            });

            Action action = () =>
                SampleLondonContactsQueryByAttribute.DeleteAllResults(orgService, expectedResultsToDelete: 2);
            action.Should().Throw<OperationPreventedException>().WithMessage(expectedExceptionMessage);

            // Assert that nothing's been deleted
            var postContacts = (from a in context.CreateQuery<Contact>()
                where a.Address1_City == "London"
                select a).ToList();

            postContacts.Count.Should().Be(3);
        }

        [Test(Description = "No records are returned by the query. Nothing happens.")]
        public void NoRecordsReturnedInQueryNothingHappens()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            Action action = () => SampleLondonContactsQueryByAttribute.DeleteAllResults(orgService);
            action.Should().NotThrow();
        }

    }
}
