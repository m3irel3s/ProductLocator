namespace ProductLocator.Api.Dtos;

public record CreateProductRequest(
    string Name,
    string Barcode

);

public record ProductResponse(
    Guid Id,
    string Name,
    string Barcode,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
