using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

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

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap(); // ReverseMap is used in we want to map object vice versa. like address to addressdto and addressdto to address.
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, p => p.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, p => p.MapFrom(s => s.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, p => p.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, p => p.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, p => p.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, p => p.MapFrom<OrderItemUrlResolver>());
                
            

        }
    }
}