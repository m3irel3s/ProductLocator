namespace ProductLocator.Api.Dtos.Store;

public record CreateStoreRequest(
    string Name,
    string Location
);