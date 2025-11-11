using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.ServiceAbstraction;
using ECommerce.Services.Specification;
using ECommerce.Shared.DTOS.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
           var Brands= await _unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDto>>(Brands);

        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
           var Spec=new ProductWithBrandAndTypeSpecification();
            var products= await _unitOfWork.GetRepository<Product,int>().GetAllAsync(Spec);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var types= await _unitOfWork.GetRepository<ProductType,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDto>>(types);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product= await _unitOfWork.GetRepository<Product,int>().GetByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }
    }
}
