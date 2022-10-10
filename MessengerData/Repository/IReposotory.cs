using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace MessengerData.Repository
{
    public interface IRepository<T> where T : class
    {
        public ApplicationDbContext GetDbContext();
        public IQueryable<T> Get(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            int pageIndex = 0,
            int pageSize = 20,
            bool AsNoTracking = true
            );

        public T? FirstOrDefault(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool AsNoTracking = true
            );
        public Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool AsNoTracking = true
            );

        public EntityEntry<T> Add(T entity);
        public Task<EntityEntry<T>> AddAsync(T entity);
        public void AddRange(IEnumerable<T> entity);
        public Task AddRangeAsync(IEnumerable<T> entity);
        public EntityEntry<T> Remove(T entity);
        public void RemoveRange(IEnumerable<T> entity);
        public EntityEntry<T> Update(T entity);
        public void UpdateRange(T entity);
        public void Save();
        public Task SaveAsync();
    }
}
