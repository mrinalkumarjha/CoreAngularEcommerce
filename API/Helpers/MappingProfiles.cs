using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    // For Automapper configuration
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // ForMember will map property ProductBrand of ProductToReturnDto to ProductBrand.Name of Product
            // We dont need to use for member if all name is same.
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(pdto => pdto.ProductBrand, p => p.MapFrom(s => s.ProductBrand.Name))
                .ForMember(pdto => pdto.ProductType, p => p.MapFrom(s => s.ProductType.Name))
                .ForMember(pdto => pdto.PictureUrl, p => p.MapFrom<ProductUrlResolver>()); // this will get imageurl from ProductUrlResolver
                
            

        }
    }
}