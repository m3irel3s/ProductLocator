
public class StoreAisleProfile : Profile
{
    public StoreAisleProfile()
    {
        CreateMap<StoreAisle, StoreAisleResponse>()
            .ForCtorParam("StoreProducts", o => o.MapFrom(x => x.StoreProducts));
    }
}
