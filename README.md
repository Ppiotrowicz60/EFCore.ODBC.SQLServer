# EFCore.ODBC.SqlServer

A custom EF Core provider that enables ODBC-based access to Microsoft SQL Server.  
Useful for cloud environments, DSN-only infrastructure, or platform-specific drivers.

## Features

- ✅ Compatible with Entity Framework Core 8 ONLY
- ✅ Uses `System.Data.Odbc`
- ✅ Reuses EF Core's SQL Server translation pipeline
- ✅ Supports LINQ, parameters, and raw SQL

## Installation

```bash
dotnet add package EFCore.ODBC.SqlServer
```

## Notes
OPENJSON is not handled well via odbc, so in some cases it might be causing specific queries to fail.
Recommended solution is to change compatibility level to 120 to prevent EF Core from using OPENJSON.
