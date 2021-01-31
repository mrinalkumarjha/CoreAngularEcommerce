using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    // specification pattern for implementing include in generic way. ls-36
    public interface ISpecification<T>
    {
         Expression<Func<T, bool>> Criteria {get;}
         List<Expression<Func<T, object>>> Includes {get;}

         Expression<Func<T, object>> OrderBy {get;}
          Expression<Func<T, object>> OrderByDescending {get;}

          // for pagination
          int Take {get;}
          int Skip {get;}
          bool IsPagingEnabled {get;}
    }
}