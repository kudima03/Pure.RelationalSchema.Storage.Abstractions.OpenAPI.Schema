using Microsoft.OpenApi;

namespace Pure.RelationalSchema.Storage.Abstractions.OpenAPI.Schema.Tests;

public sealed record StorageDataDocumentTransformerTests
{
    private static OpenApiDocument BuildDocumentWithSchema(
        string name,
        JsonSchemaType type
    )
    {
        return new OpenApiDocument
        {
            Components = new OpenApiComponents
            {
                Schemas = new Dictionary<string, IOpenApiSchema>
                {
                    [name] = new OpenApiSchema { Type = type },
                },
            },
        };
    }

    private static IOpenApiSchema Schema(OpenApiDocument document, string name)
    {
        return document.Components!.Schemas![name];
    }

    [Fact]
    public async Task ICellIsReplacedWithObjectHavingStringValueProperty()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "ICell",
            JsonSchemaType.Object
        );

        await new StorageDataDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        IOpenApiSchema schema = Schema(document, "ICell");
        Assert.Equal(JsonSchemaType.Object, schema.Type);
        Assert.Equal(JsonSchemaType.String, schema.Properties!["Value"].Type);
    }

    [Fact]
    public async Task IRowIsReplacedWithArrayOfColumnCellPairs()
    {
        OpenApiDocument document = BuildDocumentWithSchema("IRow", JsonSchemaType.Object);

        await new StorageDataDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        IOpenApiSchema schema = Schema(document, "IRow");
        Assert.Equal(JsonSchemaType.Array, schema.Type);
        Assert.NotNull(schema.Items);
        Assert.Equal(JsonSchemaType.Object, schema.Items!.Type);
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Items!.Properties!["Column"]);
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Items!.Properties!["Cell"]);
    }

    [Fact]
    public async Task IStoredTableDataSetIsReplacedWithObjectHavingTableSchemaAndRowsProperties()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "IStoredTableDataSet",
            JsonSchemaType.Object
        );

        await new StorageDataDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        IOpenApiSchema schema = Schema(document, "IStoredTableDataSet");
        Assert.Equal(JsonSchemaType.Object, schema.Type);
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Properties!["TableSchema"]);
        Assert.Equal(JsonSchemaType.Array, schema.Properties!["Rows"].Type);
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Properties!["Rows"].Items!);
    }

    [Fact]
    public async Task IStoredSchemaDataSetIsReplacedWithObjectHavingSchemaAndDatasetsProperties()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "IStoredSchemaDataSet",
            JsonSchemaType.Object
        );

        await new StorageDataDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        IOpenApiSchema schema = Schema(document, "IStoredSchemaDataSet");
        Assert.Equal(JsonSchemaType.Object, schema.Type);
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Properties!["Schema"]);
        Assert.Equal(JsonSchemaType.Array, schema.Properties!["Datasets"].Type);
        _ = Assert.IsType<OpenApiSchemaReference>(schema.Properties!["Datasets"].Items!);
    }

    [Fact]
    public async Task UnrelatedSchemaIsNotModified()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "MyCustomType",
            JsonSchemaType.Object
        );

        await new StorageDataDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        Assert.Equal(JsonSchemaType.Object, Schema(document, "MyCustomType").Type);
    }

    [Fact]
    public async Task NullComponentsDoesNotThrow()
    {
        OpenApiDocument document = new();

        await new StorageDataDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );
    }
}
