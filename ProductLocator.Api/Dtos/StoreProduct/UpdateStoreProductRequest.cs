namespace ProductLocator.Api.Dtos.StoreProduct;

public record UpdateStoreProductRequest(
    decimal Price,
    string? Aisle,
    string? Shelf
);
