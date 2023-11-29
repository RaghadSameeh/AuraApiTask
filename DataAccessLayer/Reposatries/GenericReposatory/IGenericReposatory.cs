using System.Linq.Expressions;

namespace DataAccessLayer.Reposatories
{
    public interface IGenericReposatory<T> where T : class
    {
        Task<List<T>> GetAllAsync (params Expression<Func<T, object>>[] includeProperties);
        T GetById(int id);
        void insert(T Entity);
        Task<string> SoftDelete(int id);
        Task<int> saveAsync();
    }
}
