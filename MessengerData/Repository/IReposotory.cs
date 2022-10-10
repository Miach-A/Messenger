using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace MessengerData.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool AsNoTracking = true
            );

        T? FirstOrDefault(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool AsNoTracking = true
            );
        Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool AsNoTracking = true
            );

        EntityEntry<T> Add(T entity);
        Task<EntityEntry<T>> AddAsync(T entity);
        void AddRange(IEnumerable<T> entity);
        Task AddRangeAsync(IEnumerable<T> entity);
        EntityEntry<T> Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        public EntityEntry<T> Update(T entity);
        public void UpdateRange(T entity);
        void Save();
    }
}
