using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence
{
    public class SpecificationEvaluator
    {
        //Method To Create Qery
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery,ISpecification<TEntity, TKey> specification) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;
            if (specification is not null)
            {
                //Where criteria
                if (specification.Criteria is not null)
                {
                    query = query.Where(specification.Criteria);
                }

                //Include criteria
                if (specification.IncludeExpression is not null && specification.IncludeExpression.Any())
                {
                   
                    query = specification.IncludeExpression.Aggregate(query, (current, include) => current.Include(include));
                }
                //OrderBy criteria
                if (specification.OrderBy is not null)
                {
                    query = query.OrderBy(specification.OrderBy);
                }
                //OrderByDescending criteria
                if (specification.OrderByDescending is not null)
                {
                    query = query.OrderByDescending(specification.OrderByDescending);
                }
                //Pagination criteria
                if (specification.IsPaginated)
                {
                    query = query.Skip(specification.Skip).Take(specification.Take);
                }
            }
            return query;
        }
    }
}
