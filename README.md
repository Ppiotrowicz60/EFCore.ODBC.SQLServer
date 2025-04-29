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
