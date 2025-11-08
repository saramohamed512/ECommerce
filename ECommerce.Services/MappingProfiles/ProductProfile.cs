using AutoMapper;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared.DTOS.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            //CreateMap<Source, Destination>();
            CreateMap<ProductBrand, BrandDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrands.Name))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductTypes.Name));

            CreateMap<ProductType, TypeDto>();
        }
    }
}
