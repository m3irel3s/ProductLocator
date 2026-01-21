namespace ProductLocator.Api.Dtos.Store;

public record StoreResponse(
    int Id,
    string Name,
    string Location,
    DateTime CreatedAt,
    DateTime UpdatedAt
);