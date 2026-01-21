namespace ProductLocator.Api.Dtos.Product;

public record CreateProductRequest(
    string Name,
    string Barcode
);