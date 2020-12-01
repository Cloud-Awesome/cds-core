using System;
using NUnit.Framework;
using FakeXrmEasy;
using CloudAwesome.Xrm.Core;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Core.Tests
{
    public class BaseFakeXrmTest
    {
        // TODO - * setup and tear down this for each test, and extract out of each existing test
        //var context = new XrmFakedContext();
        //var orgService = context.GetOrganizationService();


        // Any reporting set up and outputs..?

        #region Query definitions and test data

        public static readonly Account TestAccount1 = new Account
        {
            Id = Guid.NewGuid(),
            Name = "Test Account 1",
            AccountNumber = "GB123456"
        };

        public static readonly Account TestAccount1Duplicate = new Account
        {
            Id = Guid.NewGuid(),
            Name = "Test Account 1 Duplicate",
            AccountNumber = "GB123456"
        };

        public readonly string SampleFetchQuery =
            @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                  <entity name=""account"">
                    <attribute name=""accountid"" />
                    <attribute name=""name"" />
                    <attribute name=""accountnumber"" />
                    <order attribute=""name"" descending=""false"" />
                    <filter type=""and"">
                      <condition attribute=""accountnumber"" operator=""eq"" value=""GB123456"" />
                    </filter>
                  </entity>
                </fetch>";

        public readonly QueryExpression SampleQueryExpression = new QueryExpression()
        {
            EntityName = "account",
            ColumnSet = new ColumnSet("name", "accountnumber"),
            Criteria = new FilterExpression()
            {
                Conditions =
                {
                    new ConditionExpression("accountnumber", ConditionOperator.Equal, TestAccount1.AccountNumber)
                }
            }
        };

        public readonly QueryByAttribute SampleQueryByAttribute = new QueryByAttribute()
        {
            EntityName = "account",
            ColumnSet = new ColumnSet("name", "accountnumber"),
            Attributes =
            {
                "accountnumber"
            },
            Values =
            {
                "GB123456"
            }
        };

        #endregion #region Query definitions and test data

    }
}
