using Ewan.Core.Specifications;
using System.Linq.Expressions;

namespace Ewan.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        Task<T?> GetEntityWithSpec(ISpecification<T> spec);

        Task<int> CountAsync(ISpecification<T> spec);

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        void RemoveRange(IEnumerable<T> entities);

        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    }
}
