using FMS_API_PROJECT.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FMS_API_PROJECT.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly FMS_DB_Context _fmsDbContext;

        public Repository(FMS_DB_Context fmsDbContext)
        {
            _fmsDbContext = fmsDbContext;
        }

        public async Task AddAsync(T entity)
        {
            _fmsDbContext.Set<T>().Add(entity);
            await SaveAsync(); 
        }

        public async Task DeleteAsync(T entity)
        {
            _fmsDbContext.Set<T>().Remove(entity);
            await SaveAsync(); 
        }
        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string query, params object[] parameters) where T : class
        {
            return await _fmsDbContext.Database.SqlQuery<T>(query, parameters).ToListAsync();
        }

        public async Task<int> ExecuteStoredProcedureWithOutputAsync(string query, params SqlParameter[] parameters)
        {
            await _fmsDbContext.Database.ExecuteSqlCommandAsync(query, parameters);
            // Retrieve the output parameter value
            var outputParam = parameters.FirstOrDefault(p => p.Direction == System.Data.ParameterDirection.Output);
            return outputParam != null ? (int)outputParam.Value : 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _fmsDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _fmsDbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByValueAsync(string value, int id)
        {
            return await _fmsDbContext.Set<T>().FindAsync(value, id);
        }

        public async Task SaveAsync()
        {
            await _fmsDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _fmsDbContext.Entry(entity).State = EntityState.Modified;
            await SaveAsync();
        }

        //Task IRepository<T>.ExecuteStoredProcedureAsync(string query)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
