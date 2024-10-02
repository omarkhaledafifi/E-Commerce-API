using System.Linq.Expressions;

namespace Domain.Contracts
{
    public abstract class Specifications<T> where T : class
    {
        public Expression<Func<T, bool>>? Criteria { get; }

        protected Specifications(Expression<Func<T, bool>>? criteria)
        {
            Criteria = criteria;
        }

        public List<Expression<Func<T, object>>> IncludeExpressions { get; } = new();

        protected void AddInclude(Expression<Func<T, object>> expression)
            => IncludeExpressions.Add(expression);
    }
}

// 
// IEnumerable<T>.Where(Func<T,bool>)
// IQueryable<T>.Where(Expression<Func<T,bool>>)
// Include(Expression<Func<T,object>>)