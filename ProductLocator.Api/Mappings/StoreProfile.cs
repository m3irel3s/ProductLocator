using AutoMapper;

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Store, StoreResponse>();
    }
}
