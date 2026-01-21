namespace ProductLocator.Api.Dtos.StoreProduct;

public record CreateStoreProductRequest(
    int ProductId,
    decimal Price,
    string? Aisle,
    string? Shelf
);