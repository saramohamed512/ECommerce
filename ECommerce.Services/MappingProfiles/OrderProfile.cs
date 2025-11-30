using AutoMapper;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Shared.DTOS.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDTO,OrderedDictionary>().ReverseMap();
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Product.PictureUrl));

            CreateMap<DeliveryMethod, DeliveryMethodDTO>();
        }
    }
}
