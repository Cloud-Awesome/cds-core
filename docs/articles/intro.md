# Cds-Core

This is a core project for CDS, intended either to be used as an accelerator to new projects or to centralise base, common functionality for connecting to CDS.

This project is only intended for solutions _outside_ of CDS, e.g. for integrations, not for plugins. While there's nothing stopping it being consumed in a plugin, that would then require use of IlMerge/IlRepack and is not the primary target. There are much better repos with sample base plugins, use them instead :)  

## Feature implemented

There are currently three main areas implemented:

### Entity Extensions

Extension methods to standard Xrm objects, e.g. CRUD methods

### Query Helpers

Helper methods and extension methods to assist standard query methods, e.g. executing queries, deleting results

### Tracing Helpers

Core methods to log trace methods in various outputs, e.g. Console, Text file, Application Insights

## Features not yet implement

There are also placeholders for features which haven't yet been implemented. Other areas may be added in the future, and feature requests for functionality of core use are always welcome!

### FetchXml Helpers

Methods to help build and validate Fetch queries

### Metadata helpers

To interact with the CDS metadata service. 

Several metadata features have been implemented in the cds-customisation project, dealing with customisation of the system, but if any prove useful outside of this area, they can be promoted to the core project instead.