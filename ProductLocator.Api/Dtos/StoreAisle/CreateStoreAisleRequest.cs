namespace ProductLocator.Api.Dtos.StoreAisle;

public record CreateStoreAisleRequest(
    string Name,
    int MaxShelf
);