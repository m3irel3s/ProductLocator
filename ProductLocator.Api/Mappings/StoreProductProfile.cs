using AutoMapper;

public class StoreProductProfile : Profile
{
    public StoreProductProfile()
    {
        CreateMap<StoreProduct, StoreProductResponse>()
            .ForCtorParam("StoreName", o => o.MapFrom(x => x.Store.Name))
            .ForCtorParam("ProductName", o => o.MapFrom(x => x.Product.Name))
            .ForCtorParam("ProductBarcode", o => o.MapFrom(x => x.Product.Barcode));
    }
}
