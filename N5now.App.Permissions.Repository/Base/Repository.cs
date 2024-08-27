
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using N5now.App.Permissions.DataLayer.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Permissions.Repository.Base
{
  public class Repository<TContext, TEntity, TId> : IRepository<TEntity, TId>, IDisposable
    where TContext : DbContext
    where TEntity : class
    where TId : IComparable<TId>
  {
    public Repository(TContext dbContext)
    {
      this.Context = dbContext;
      this.DbSet = this.Context.Set<TEntity>();
    }

    public Repository(TContext dbContext, Microsoft.EntityFrameworkCore.DbSet<TEntity> dbSet)
    {
      this.Context = dbContext;
      this.DbSet = dbSet;
    }

    protected TContext Context { get; }

    protected Microsoft.EntityFrameworkCore.DbSet<TEntity> DbSet { get; set; }

    public virtual async Task<TEntity> GetAsync(
      TId id,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
      IProperty keyProperty = this.Context.Model.FindEntityType(typeof (TEntity)).FindPrimaryKey().Properties[0];
      int keyId = (int) Convert.ChangeType((object) id, typeof (int));
      return await (include != null ? (IQueryable<TEntity>) include((IQueryable<TEntity>) this.DbSet) : (IQueryable<TEntity>) this.DbSet).FirstOrDefaultAsync<TEntity>((Expression<Func<TEntity, bool>>) (e => EF.Property<int>(e, keyProperty.Name) == keyId));
    }

    public async Task<IEnumerable<TDto>> RunSqlQuery<TDto>(string sqlQuery, object parameters = null)
    {
      return await this.Context.FromSqlQueryAsync<TDto>(sqlQuery, parameters);
    }

    public async Task<IEnumerable<TDto>> RunStoredProcedure<TDto>(
      string storedProcedure,
      object parameters = null,
      int? commandTimeout = null)
    {
      return await this.Context.FromSqlQueryAsync<TDto>(storedProcedure, parameters, commandTimeout: commandTimeout, commandType: new CommandType?(CommandType.StoredProcedure));
    }

    public async Task<IEnumerable<TReturn>> RunStoredProcedure<TFirst, TSecond, TReturn>(
      string sql,
      Func<TFirst, TSecond, TReturn> map,
      object param = null,
      IDbTransaction transaction = null,
      bool buffered = true,
      string splitOn = "Id",
      int? commandTimeout = null)
    {
      return await this.Context.FromSqlQueryAsync<TFirst, TSecond, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, new CommandType?(CommandType.StoredProcedure));
    }

    public async Task ExecuteNonQueryAsync(
      string sqlQuery,
      object parameters = null,
      IDbTransaction transaction = null,
      int? commandTimeout = null)
    {
      await this.Context.ExecuteNonQueryAsync(sqlQuery, parameters, transaction, commandTimeout);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
      return (await this.DbSet.AddAsync(entity)).Entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
      await this.DbSet.AddRangeAsync(entities);
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
      this.DbSet.UpdateRange(entities);
    }

    public virtual void Delete(TEntity entity)
    {
      this.DbSet.Attach(entity);
      this.Context.Entry<TEntity>(entity).State = EntityState.Deleted;
    }

    public virtual void DeleteRange(IEnumerable<TEntity> entities)
    {
      this.DbSet.RemoveRange(entities);
    }

    public virtual void Update(TEntity entity)
    {
      if (!this.DbSet.Local.All<TEntity>((Func<TEntity, bool>) (p => (object) p != (object) entity)))
        return;
      this.DbSet.Attach(entity);
      this.Context.Entry<TEntity>(entity).State = EntityState.Modified;
    }

    public virtual IQueryable<TEntity> All(bool @readonly = true)
    {
      return !@readonly ? (IQueryable<TEntity>) this.DbSet : this.DbSet.AsNoTracking<TEntity>();
    }

    public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
    {
      return this.DbSet.Where<TEntity>(expression).CountAsync<TEntity>();
    }

    public virtual async Task<int> MaxAsync(Expression<Func<TEntity, int>> selector)
    {
      return await this.DbSet.MaxAsync<TEntity, int>(selector);
    }

    public virtual IQueryable<TEntity> Find(
      Expression<Func<TEntity, bool>> predicate,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
      bool @readonly = true)
    {
      IQueryable<TEntity> source = (IQueryable<TEntity>) this.DbSet;
      if (@readonly)
        source = source.AsNoTracking<TEntity>();
      if (include != null)
        source = (IQueryable<TEntity>) include(source);
      return source.Where<TEntity>(predicate);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.Context?.Dispose();
    }
  }
}
