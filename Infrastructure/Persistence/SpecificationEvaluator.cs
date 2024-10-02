namespace Persistence
{
    static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(
            IQueryable<T> inputQuery,
            Specifications<T> specifications) where T : class
        {
            var query = inputQuery;
            if (specifications.Criteria is not null)
                query = query.Where(specifications.Criteria);





            query = specifications.IncludeExpressions.Aggregate(
               query,
               (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            return query;

        }
    }
}
