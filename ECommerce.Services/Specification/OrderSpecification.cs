using ECommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specification
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(string Email) : base(o => o.UserEmil == Email)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
        }
        public OrderSpecification(Guid id, string Email) : base(o => o.Id == id && (string.IsNullOrEmpty(Email) || o.UserEmil.ToLower() == Email.ToLower()))
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
           
        }
    }
}
