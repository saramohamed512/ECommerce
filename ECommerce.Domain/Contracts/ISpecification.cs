using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface ISpecification<TEntity, TKey>where TEntity : BaseEntity<TKey>
    {
        //Include
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get;}
        //Where
        public Expression<Func<TEntity, bool>> Criteria { get; }
    }
}
