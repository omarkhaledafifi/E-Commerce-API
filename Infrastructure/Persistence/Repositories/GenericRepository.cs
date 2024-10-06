namespace Persistence.Repositories
{
    internal class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {


        private readonly StoreContext _storeContext;

        public GenericRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public async Task AddAsync(TEntity entity) => await _storeContext.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity) => _storeContext.Set<TEntity>().Remove(entity);
        public void Update(TEntity entity) => _storeContext.Set<TEntity>().Update(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false)
         => trackChanges ? await _storeContext.Set<TEntity>().ToListAsync()
            : await _storeContext.Set<TEntity>().AsNoTracking().ToListAsync();
        //if (trackChanges)
        //    return await _storeContext.Set<TEntity>().ToListAsync();
        //return await _storeContext.Set<TEntity>().AsNoTracking().ToListAsync();
        public async Task<TEntity?> GetAsync(TKey id) => await _storeContext.Set<TEntity>().FindAsync(id);

        public async Task<TEntity?> GetAsync(Specifications<TEntity> specifications)
          => await SpecificationEvaluator.GetQuery(_storeContext.Set<TEntity>(), specifications)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(Specifications<TEntity> specifications)
        => await SpecificationEvaluator.GetQuery(_storeContext.Set<TEntity>(), specifications)
            .ToListAsync();

        public async Task<int> CountAsync(Specifications<TEntity> specifications)
        => await SpecificationEvaluator.GetQuery(_storeContext.Set<TEntity>(), specifications)
            .CountAsync();
    }
}
//Context.Set<Product>().Include(brand).Include(Type).ToListAsync()