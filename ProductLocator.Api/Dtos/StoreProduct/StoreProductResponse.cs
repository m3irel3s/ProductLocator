namespace ProductLocator.Api.Dtos.StoreProduct;

public record StoreProductResponse(
    int StoreId,
    int ProductId,
    string StoreName,
    string ProductName,
    string ProductBarcode,
    decimal Price,
    int? AisleId,
    int? ShelfNumber,
    DateTime CreatedAt,
    DateTime UpdatedAt
);