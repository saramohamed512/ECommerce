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
                if (specification.IncludeExpression is not null && specification.IncludeExpression.Any())
                {
                    //// Apply includes from specification
                    //foreach (var includeExpression in specification.IncludeExpression)
                    //{
                    //    query = query.Include(includeExpression);
                    //}
                    query = specification.IncludeExpression.Aggregate(query, (current, include) => current.Include(include));
                }
            }
            return query;
        }
    }
}
