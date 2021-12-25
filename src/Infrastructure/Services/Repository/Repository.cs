using Microsoft.EntityFrameworkCore;
using SharedKernel.Interfaces;

namespace Infrastructure.Services.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly ISpecificationEvaluator specificationEvaluator;
        public Repository(DbContext context)
            : this(context, SpecificationEvaluator.Default)
        {
            _context = context;
        }
        public Repository(DbContext context, ISpecificationEvaluator specificationEvaluator = null)
        {
            _context = context;
            this.specificationEvaluator = specificationEvaluator;
        }
        public virtual async Task<T> AddAsync(T entity, CancellationToken token = default)
        {
            _context.Set<T>().Add(entity);

            await SaveChangesAsync(token);

            return entity;
        }
        public virtual async Task UpdateAsync(T entity, CancellationToken token = default)
        {
            _context.Entry(entity).State = EntityState.Modified;

            await SaveChangesAsync(token);
        }

        public virtual async Task DeleteAsync(T entity, CancellationToken token = default)
        {
            _context.Set<T>().Remove(entity);

            await SaveChangesAsync(token);
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken token = default)
        {
            _context.Set<T>().RemoveRange(entities);

            await SaveChangesAsync(token);
        }

        public virtual async Task SaveChangesAsync(CancellationToken token = default)
        {
            await _context.SaveChangesAsync(token);
        }
        public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken token = default) where TId : notnull
        {
            return await _context.Set<T>().FindAsync(new object[] { id }, token);
        }

        public virtual async Task<T?> GetBySpecAsync<Spec>(Spec specification, CancellationToken token = default) where Spec : ISpecification<T>
        {
            return await ApplaySpecification(specification).FirstOrDefaultAsync(token);
        }

        public virtual async Task<List<T>> ListAsync(CancellationToken token = default)
        {
            return await _context.Set<T>().ToListAsync(token);
        }

        public virtual async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken token = default)
        {
            var queryResult = await ApplaySpecification(specification).ToListAsync(token);
            return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
        }

        public virtual async Task<int> CountAsync(CancellationToken token = default)
        {
            return await _context.Set<T>().CountAsync(token);
        }

        public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await ApplaySpecification(specification, true).CountAsync(cancellationToken);
        }

        protected virtual IQueryable<T> ApplaySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false)
        {
            return specificationEvaluator.GetQuery(_context.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
        }
    }
}
