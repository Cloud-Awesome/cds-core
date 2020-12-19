# Entity extensions

Extension methods on the `Entity` base class implementing common execute methods

### Implemented methods

| Method | Notes |
| - | - |
| Create | Create new record. Returns an `EntityReference` |
| Delete | Deletes single record |
| Update | Update single record |
| CreateOrUpdate | Executes a query, expecting a single record to be returned. If one record is returned, it will be updated, otherwise a new record is created |
| CloneFrom | Clones to a new entity instance including all attributes excluding primary ID attribute and attributes explicitly listed |
| Retrieve | Retrieves the record extended as long as the primary ID attribute is populated |
| Associate | Associates a record with a list or collection of child records |

### Not Implemented
| Method | Notes |
| - | - |
| ExecuteWorkflow | ... |
| Disassociate | ... |
| SetState | Has this method been deprecated in favour of an update message? |

## Extending with early bound classes

Using the CrmSrvUtil to generate classes which inherit from `Entity` so all are extendable

...