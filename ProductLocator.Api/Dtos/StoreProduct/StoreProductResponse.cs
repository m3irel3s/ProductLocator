namespace ProductLocator.Api.Dtos.StoreProduct;

public record StoreProductResponse(
    int StoreId,
    int ProductId,
    string StoreName,
    string ProductName,
    string ProductBarcode,
    decimal Price,
    string? Aisle,
    string? Shelf,
    DateTime CreatedAt,
    DateTime UpdatedAt
);