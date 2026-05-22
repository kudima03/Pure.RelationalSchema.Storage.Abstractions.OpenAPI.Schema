# Pure.RelationalSchema.Storage.Abstractions.OpenAPI.Schema

OpenAPI document transformer for the **Pure** ecosystem — corrects auto-generated schemas for `Pure.RelationalSchema.Storage.Abstractions` types.

[![.NET build & test](https://github.com/kudima03/Pure.RelationalSchema.Storage.Abstractions.OpenAPI.Schema/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Storage.Abstractions.OpenAPI.Schema/actions/workflows/build-and-test.yml)
[![NuGet](https://img.shields.io/nuget/v/Pure.RelationalSchema.Storage.Abstractions.OpenAPI.Schema)](https://www.nuget.org/packages/Pure.RelationalSchema.Storage.Abstractions.OpenAPI.Schema)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Overview

`Pure.RelationalSchema.Storage.Abstractions.OpenAPI.Schema` provides an `IOpenApiDocumentTransformer` that replaces the ASP.NET Core–generated component schemas for `ICell`, `IRow`, `IStoredTableDataSet`, and `IStoredSchemaDataSet` with schemas that match how those types actually serialize to JSON.

ASP.NET Core's built-in OpenAPI reflection cannot resolve the true JSON shape of these interface-based types. This transformer runs as a post-processing step and overwrites the relevant entries in `document.Components.Schemas`.

## API

| Type | Kind | Description |
|------|------|-------------|
| `StorageDataDocumentTransformer` | `sealed class` | Implements `IOpenApiDocumentTransformer`. Replaces generated schemas for `ICell`, `IRow`, `IStoredTableDataSet`, and `IStoredSchemaDataSet` with their actual JSON representation. |

### Schema corrections applied

| Component | Corrected JSON shape |
|-----------|----------------------|
| `ICell` | `{ "Value": "<string>" }` |
| `IRow` | `[{ "Column": IColumn, "Cell": ICell }, ...]` |
| `IStoredTableDataSet` | `{ "TableSchema": ITable, "Rows": IRow[] }` |
| `IStoredSchemaDataSet` | `{ "Schema": ISchema, "Datasets": IStoredTableDataSet[] }` |

## Target Frameworks

- .NET 10

## Installation

```bash
dotnet add package Pure.RelationalSchema.Storage.Abstractions.OpenAPI.Schema
```

## Usage

Register the transformer when configuring OpenAPI in your ASP.NET Core app:

```csharp
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<StorageDataDocumentTransformer>();
});
```
