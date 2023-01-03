using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.QueryHelperTests
{
    [TestFixture]
    [Category("LongRunningTests")]
    public class QueryOverMultiplePagesTests: BaseFakeXrmTest
    {
        [Test(Description = "By default a retrieve multiple will return a max of 5,000 records unless specifically told not to")]
        public void Results_Are_More_Than_5000_But_Only_Returns_First_Page_By_Default()
        {
            const int recordSize = 6000;
            
            var contacts = new Contact[recordSize];
            for (var i = 0; i < recordSize; i++)
            {
                contacts[i] = new Contact
                {
                    Id = Guid.NewGuid(),
                    LastName = $"Contact {i}"
                };
            }

            var entities = new List<Contact>(12000);
                entities.AddRange(contacts);
                
            XrmContext.Initialize(entities);

            var query = new QueryExpression
            {
                EntityName = Contact.EntityLogicalName
            };

            var results = QueryExtensions.RetrieveMultipleFromQuery(OrgService, query);
            results.Entities.Count.Should().Be(5000);
        }
        
        [Test(Description = "When instructed to, a QueryExpression will return more than the 5,000 paged default")]
        public void Query_Expression_Results_Are_More_Than_5000_Are_Returned_If_Instructed()
        {
            // Arrange
            const int recordSize = 6000;
            
            var contacts = new Contact[recordSize];
            for (var i = 0; i < recordSize; i++)
            {
                contacts[i] = new Contact
                {
                    Id = Guid.NewGuid(),
                    LastName = $"Contact {i}"
                };
            }

            var entities = new List<Contact>(recordSize);
            entities.AddRange(contacts);
                
            XrmContext.Initialize(entities);

            var query = new QueryExpression
            {
                EntityName = Contact.EntityLogicalName
            };

            // Act
            var results = 
                QueryExtensions.RetrieveMultipleFromQuery(OrgService, query, true);
            
            // Assert
            results.Entities.Count.Should().Be(6000);
        }
        
        [Test(Description = "When instructed to, a FetchExpression will return more than the 5,000 paged default")]
        public void Fetch_Expression_Results_Are_More_Than_5000_Are_Returned_If_Instructed()
        {
            // Arrange
            const int recordSize = 6000;
            
            var contacts = new Contact[recordSize];
            for (var i = 0; i < recordSize; i++)
            {
                contacts[i] = new Contact
                {
                    Id = Guid.NewGuid(),
                    LastName = $"Contact {i}"
                };
            }

            var entities = new List<Contact>(recordSize);
            entities.AddRange(contacts);
                
            XrmContext.Initialize(entities);

            var queryString = @"<fetch version='1.0' mapping='logical' output-format='xml-platform'>
                                    <entity name='contact'>
                                        <attribute name='lastname' />
                                        <order attribute='name' descending='false'/>
                                    </entity>
                                </fetch>";
            var query = new FetchExpression(queryString);

            // Act
            var results = 
                QueryExtensions.RetrieveMultipleFromQuery(OrgService, query, true);
            
            // Assert
            results.Entities.Count.Should().Be(6000);
        }
    }
}