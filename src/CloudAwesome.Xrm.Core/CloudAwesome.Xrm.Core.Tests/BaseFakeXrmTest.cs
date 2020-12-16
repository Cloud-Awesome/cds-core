using System;
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

        public readonly string SampleAccountFetchQuery =
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

        public readonly QueryExpression SampleAccountQueryExpression = new QueryExpression()
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

        public readonly QueryByAttribute SampleAccountQueryByAttribute = new QueryByAttribute()
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

        public static readonly Contact JohnContact = new Contact()
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Contact",
            Address1_City = "London",
            EMailAddress1 = "john@contact.test"
        };

        public static readonly Contact JamieContact = new Contact()
        {
            Id = Guid.NewGuid(),
            FirstName = "Jamie",
            LastName = "Contact",
            Address1_City = "London",
            EMailAddress1 = "Jamie@contact.test"
        };

        public static readonly Contact JackContact = new Contact()
        {
            Id = Guid.NewGuid(),
            FirstName = "Jack",
            LastName = "Contact",
            Address1_City = "Edinburgh",
            EMailAddress1 = "Jack@contact.test"
        };

        public static readonly Contact JamesContact = new Contact()
        {
            Id = Guid.NewGuid(),
            FirstName = "James",
            LastName = "Contact",
            Address1_City = "Newcastle",
            EMailAddress1 = "James@contact.test"
        };

        public static readonly Contact JacobContact = new Contact()
        {
            Id = Guid.NewGuid(),
            FirstName = "Jacob",
            LastName = "Contact",
            Address1_City = "London",
            EMailAddress1 = "Jacob@contact.test"
        };

        public static readonly Contact JeremyContact = new Contact()
        {
            Id = Guid.NewGuid(),
            FirstName = "Jeremy",
            LastName = "Contact",
            Address1_City = "London",
            EMailAddress1 = "Jeremy@contact.test"
        };

        public readonly QueryByAttribute SampleLondonContactsQueryByAttribute = new QueryByAttribute()
        {
            EntityName = "contact",
            ColumnSet = new ColumnSet("firstname", "lastname", "emailaddress1", "address1_city"),
            Attributes =
            {
                "address1_city"
            },
            Values =
            {
                "London"
            }
        };

        #endregion Query definitions and test data

    }
}
