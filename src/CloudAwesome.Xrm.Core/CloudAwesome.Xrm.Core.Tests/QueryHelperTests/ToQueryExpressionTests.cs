using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.QueryHelperTests
{
    [TestFixture]
    public class ToQueryExpressionTests: BaseFakeXrmTest
    {
        [Test]
        public void Valid_Fetch_Query_Returns_QE()
        {
            var fetchQuery = 
                @"<fetch mapping='logical'>   
                   <entity name='account'>  
                      <attribute name='accountid'/>   
                      <attribute name='name'/>   
                   </entity>  
                </fetch>  ";

            var queryExpression = fetchQuery.ToQueryExpression(OrgService);

            queryExpression.ColumnSet.Columns.Count.Should().Be(2);
            queryExpression.EntityName.Should().Be("account");
        }

        [Test]
        public void Invalid_FetchQuery_Throws_Suitable_Exception()
        {
            var fetchQuery = "I'm an invalid query";
            Action action = () => fetchQuery.ToQueryExpression(OrgService);
            action.Should().Throw<Exception>();
        }
    }
}