using ECommerce.Domain.Entities.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specification
{
    public class ProductWithBrandAndTypeSpecification: BaseSpecification<Product, int>
    {
        public ProductWithBrandAndTypeSpecification(int? brandId, int? typeId): base
            (P=> (!brandId.HasValue || P.BrandId==brandId.Value)&&
            (!typeId.HasValue|| P.TypeId==typeId.Value))
        {
            AddInclude(p => p.ProductBrands);
            AddInclude(p => p.ProductTypes);

        }
        public ProductWithBrandAndTypeSpecification(int id): base(p => p.Id == id)
        {
            
            AddInclude(p => p.ProductBrands);
            AddInclude(p => p.ProductTypes);
        }
    }
}
