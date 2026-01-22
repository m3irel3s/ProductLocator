namespace ProductLocator.Api.Dtos.StoreAisle;

public record StoreAisleResponse(
    int Id,
    int StoreId,
    string Name,
    int MaxShelf,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IEnumerable<StoreProductResponse> StoreProducts
);