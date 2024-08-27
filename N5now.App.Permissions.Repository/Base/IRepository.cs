
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Permissions.Repository.Base
{
  public interface IRepository<TEntity, in TId> where TEntity : class
  {
    Task<TEntity> GetAsync(
      TId id,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

    Task<TEntity> AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);

    void DeleteRange(IEnumerable<TEntity> entities);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);

    Task<int> MaxAsync(Expression<Func<TEntity, int>> selector);

    IQueryable<TEntity> All(bool @readonly = true);

    IQueryable<TEntity> Find(
      Expression<Func<TEntity, bool>> predicate,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
      bool @readonly = true);

    Task<IEnumerable<TDto>> RunSqlQuery<TDto>(string sqlQuery, object parameters = null);

    Task<IEnumerable<TDto>> RunStoredProcedure<TDto>(
      string storedProcedure,
      object parameters = null,
      int? commandTimeout = null);

    Task ExecuteNonQueryAsync(
      string sqlQuery,
      object parameters = null,
      IDbTransaction transaction = null,
      int? commandTimeout = null);

    Task<IEnumerable<TReturn>> RunStoredProcedure<TFirst, TSecond, TReturn>(
      string sql,
      Func<TFirst, TSecond, TReturn> map,
      object param = null,
      IDbTransaction transaction = null,
      bool buffered = true,
      string splitOn = "Id",
      int? commandTimeout = null);
  }
}
