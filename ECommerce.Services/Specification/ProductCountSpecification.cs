using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specification
{
    public class ProductCountSpecification: BaseSpecification<Product, int> 
    {
        public ProductCountSpecification(ProductQueryParams queryParams) : base
            (P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId.Value) &&
            (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId.Value) &&
            (string.IsNullOrEmpty(queryParams.Search) || P.Name.ToLower().Contains(queryParams.Search.ToLower())))
        {

        }
    }
}
