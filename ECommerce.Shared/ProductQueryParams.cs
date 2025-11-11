using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared
{
    public class ProductQueryParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Search { get; set; }
        public ProductSortingOptions? Sort { get; set; } // FIX: Change type from ProductQueryParams? to ProductSortingOptions?
        private int _pageIndex = 1;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = (value <= 1) ? 1 : value; }


        }
        private const int MaxPageSize = 10; 
        private const int DefaultPageSize = 5;
        private int _pageSize = 5;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value <= 0)
                {
                    _pageSize = DefaultPageSize;
                }
                else if (value > MaxPageSize)
                {
                    _pageSize = MaxPageSize;
                }
                else
                {
                    _pageSize = value;

                }
            }
        }

    }
}
