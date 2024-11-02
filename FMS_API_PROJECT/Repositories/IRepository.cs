using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FMS_API_PROJECT.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByValueAsync(string value, int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveAsync();
        Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string query, params object[] parameters) where T : class;
        Task<int> ExecuteStoredProcedureWithOutputAsync(string query, params SqlParameter[] parameters);

        //Task ExecuteStoredProcedureAsync(string query);
    }
}
