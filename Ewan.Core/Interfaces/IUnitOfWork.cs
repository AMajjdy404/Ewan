namespace Ewan.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync();
    }
}
