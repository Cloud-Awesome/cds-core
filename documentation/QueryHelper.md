# Query Helper

Helper classes for querying Dynamics/CDS

| Method | Notes |
| - | - |
| RetrieveSingleRecord | `QueryBase` extension method retrieves the FirstOrDefault from query results |
| RetrieveMultiple | `QueryBase` extension method returning an entity collection |
| RetrieveRecordFromQuery | Retrieves the FirstOrDefault record from a `QueryBase` and optionally throws an exception if multiple records are unexpectedly returned |
| DeleteSingleRecord | `QueryBase` extension method which deletes the FirstOrDefault record returned |
| DeleteAllRecords | Deletes all results returned from a query. Limits the number of records are deleted (defaults to 50) to avoid extensive data loss/processing time and optionally throws an exception if an unexoected number of records are returned for deletion |



