# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All `dotnet` commands must be run from the `./src` directory.

```bash
dotnet restore
dotnet build --no-restore -warnaserror
dotnet test --no-build                        # run xUnit tests
dotnet format --verify-no-changes             # check code style (CI enforces this)
dotnet format && csharpier format .           # auto-fix code style
dotnet pack --configuration Release -p:PackageVersion=<version> --output .
```

## Architecture

This is a **single-class NuGet library** — one `sealed class`, one dependency, no abstractions.

`StorageDataDocumentTransformer` implements `IOpenApiDocumentTransformer` (from `Microsoft.AspNetCore.OpenApi`). It holds a static dictionary of four hardcoded `OpenApiSchema` entries keyed by component name (`ICell`, `IRow`, `IStoredTableDataSet`, `IStoredSchemaDataSet`). In `TransformAsync` it overwrites matching entries in `document.Components.Schemas` with those corrected schemas. The transformer is stateless and AOT-compatible.

**Multi-targeting:** net10.0 only.

**AOT:** `IsAotCompatible = true` — keep the class free of reflection-based patterns.

**Package validation:** `EnablePackageValidation = true` with `PackageValidationBaselineVersion = 0.1.0-preview.0.1.0`. Breaking API changes fail the build.

**Publishing:** triggered by pushing a semver tag (e.g. `1.0.0`). The tag becomes the `PackageVersion`. The workflow publishes to both NuGet.org and GitHub Packages.

## Code Style

Enforced via `.editorconfig` and `dotnet format --verify-no-changes` in CI:

- No `var` — always use explicit types
- No expression-bodied methods, constructors, operators, or local functions (expression bodies allowed only on properties, indexers, accessors, and lambdas)
- Private fields: `_camelCase` prefix required
- File-scoped namespaces required (`namespace Foo;` not `namespace Foo { }`)
- Max line length: 90 characters
- `using` directives must be placed outside the namespace

## Commit Messages

Do not mention Claude or AI assistance in commit messages.
