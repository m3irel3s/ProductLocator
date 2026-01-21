namespace ProductLocator.Api.Dtos;

public record CreateStoreProductRequest(
    Guid ProductId,
    decimal Price,
    string? Aisle,
    string? Shelf
);

public record StoreProductResponse(
    Guid StoreId,
    Guid ProductId,
    string StoreName,
    string ProductName,
    string Barcode,
    decimal Price,
    string? Aisle,
    string? Shelf,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record UpdateStoreProductRequest(
    decimal Price,
    string? Aisle,
    string? Shelf
);
