# CloudAwesome.Xrm.Core

[![Build Status](https://dev.azure.com/cloud-awesome/CloudAwesome.Xrm/_apis/build/status/Cloud-Awesome.cds-core?branchName=master)](https://dev.azure.com/cloud-awesome/CloudAwesome.Xrm/_build/latest?definitionId=1&branchName=master)
[![SonarCloud Coverage](https://sonarcloud.io/api/project_badges/measure?project=cds-core&metric=coverage)](https://sonarcloud.io/component_measures/metric/coverage/list?id=cds-core)

Core project for Dynamics 365/CDS extensions

- Entity Extensions
- Query Helpers
- Tracing Helpers
- FetchXML Helpers
- Metadata Helpers

## Installing and using Xrm.Core

Install the most recent version from NuGet here: https://www.nuget.org/packages/CloudAwesome.Xrm.Core/

## Example usage

See [Unit tests on GitHub](https://github.com/Cloud-Awesome/cds-core/tree/master/src/CloudAwesome.Xrm.Core/CloudAwesome.Xrm.Core.Tests) for more comprehensive examples.

### Entity Extensions

```csharp
var contact = new Contact()
{
    Id = Guid.NewGuid(),
    FirstName = "John",
    LastName = "Contact",
    Address1_City = "London",
    EMailAddress1 = "john@contact.test"
};

// Upsert contact record
contact.Id = contact.CreateOrUpdate(orgService).Id;

contact.LastName = "UpdateContact";
contact.Update(orgService);

contact.Delete(orgService);
```

### Query Helpers

```csharp
var sampleAccountQuery = new QueryExpression()
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
}

// Get all accounts
var allAccounts = sampleAccountQuery.RetrieveMultiple(orgService);

// Get just one record
// Throw QueryBaseException if multiple records are unexpectedly returned
var account = sampleAccountQuery.RetrieveSingleRecord(orgService);

// Delete the record
// Throw QueryBaseException if multiple records are unexpectedly returned
sampleAccountQuery.DeleteSingleRecord(orgService);
```

```csharp
var expectedRecordsReturned = 2;
var sampleLondonContactsQuery = new QueryByAttribute()
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

// Delete 2 results 
// Throw OperationPreventedException if results != 2
var success = sampleLondonContactsQuery.DeleteAllResults(orgService, expectedRecordsReturned);
```

## Feedback

I'd love to get any feedback (well, constructive feedback!) or feature requests that could be of use.

Feel free to raise a [GitHub issue](https://github.com/Cloud-Awesome/cds-core/issues) 

Or you can get me on [Twitter @Arthur82](https://twitter.com/Arthur82)