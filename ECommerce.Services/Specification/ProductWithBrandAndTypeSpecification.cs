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
        public ProductWithBrandAndTypeSpecification(): base()
        {
            AddInclude(p => p.ProductBrands);
            AddInclude(p => p.ProductTypes);

        }
    }
}
