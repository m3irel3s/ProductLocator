namespace ProductLocator.Api.Dtos.Product;

public record ProductResponse(
    int Id,
    string Name,
    string Barcode,
    DateTime CreatedAt,
    DateTime UpdatedAt
);