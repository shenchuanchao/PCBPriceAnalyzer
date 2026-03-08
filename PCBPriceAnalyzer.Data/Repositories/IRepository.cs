using System.Linq.Expressions;

namespace PCBPriceAnalyzer.Data.Repositories
{
    /// <summary>
    /// 泛型仓储接口
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 返回 IQueryable 接口，允许进行 LINQ 查询（如 Include、Where、OrderBy）
        /// </summary>
        IQueryable<T> GetQueryable();
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }

}
