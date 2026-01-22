namespace ProductLocator.Api.Dtos.StoreProduct;

public record CreateStoreProductRequest(
    int ProductId,
    decimal Price,
    int? AisleId,
    int? ShelfNumber
);