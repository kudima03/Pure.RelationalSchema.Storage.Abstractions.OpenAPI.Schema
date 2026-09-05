# Changelog

All notable changes to Pure.RelationalSchema.Storage.Abstractions.OpenAPI.Schema are documented here.

Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

## [0.1.0-preview.0.2.1] — 2026-08-22

- Maintenance release: dependency and build updates.

## [0.1.0-preview.0.2.0] — 2026-08-14

- Maintenance release: dependency and build updates.

## [0.1.0-preview.0.1.2] — 2026-08-13

### Fixed

- Pinned `Microsoft.OpenApi` to 2.11.0 to resolve the NU1903 security advisory.

## [0.1.0-preview.0.1.1] — 2026-06-14

- Maintenance release: dependency and build updates.

## [0.1.0-preview.0.1.0] — 2026-05-14

### Added

- **`StorageDataDocumentTransformer`** — an `IOpenApiDocumentTransformer` that
  replaces the auto-generated OpenAPI schemas for `ICell`, `IRow`,
  `IStoredTableDataSet` and `IStoredSchemaDataSet` with schemas matching their
  actual JSON serialization output.
