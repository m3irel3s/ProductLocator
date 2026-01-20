namespace ProductLocator.Api.Dtos;

public record CreateProductRequest(
    string Name
);

public record ProductResponse(
    Guid Id,
    string Name,
    DateTime CreatedAt
);