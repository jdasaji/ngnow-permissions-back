
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Query;
using N5now.App.Permissions.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Permissions.Features.commandhandler
{
  public class CommandHandlerBase<TEntity, TId> : ICommandHandlerBase<TEntity, TId> where TEntity : class
  {
    private readonly IRepository<TEntity, TId> _repository;

    public CommandHandlerBase(IRepository<TEntity, TId> repository)
    {
      this._repository = repository;
    }

    protected IRepository<TEntity, TId> Repository => this._repository;

    public virtual async Task<TEntity> GetAsync(
      TId id,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
      return await this._repository.GetAsync(id, include);
    }

    public virtual IQueryable<TEntity> All(bool @readonly = true)
    {
      return this._repository.All(@readonly);
    }

    public virtual IQueryable<TEntity> Find(
      Expression<Func<TEntity, bool>> predicate,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
      bool @readonly = true)
    {
      return this._repository.Find(predicate, include, @readonly);
    }

    public virtual async Task<IEnumerable<TDto>> RunSqlQuery<TDto>(string sqlQuery)
    {
      return await this._repository.RunSqlQuery<TDto>(sqlQuery);
    }

    public async Task<IEnumerable<TDto>> RunStoredProcedure<TDto>(
      string storedProcedure,
      object parameters = null,
      int? commandTimeout = null)
    {
      return await this._repository.RunStoredProcedure<TDto>(storedProcedure, parameters, commandTimeout);
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
      return await this._repository.RunStoredProcedure<TFirst, TSecond, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout);
    }

    public async Task ExecuteNonQueryAsync(
      string sqlQuery,
      object parameters = null,
      IDbTransaction transaction = null,
      int? commandTimeout = null)
    {
      await this._repository.ExecuteNonQueryAsync(sqlQuery, parameters, transaction, commandTimeout);
    }

    public virtual async Task<ValidationResult> AddAsync(
      TEntity entity,
      params IValidator<TEntity>[] validaciones)
    {
      ValidationResult validationResultEntity = await this.ValidateEntityAsync(entity, (IEnumerable<IValidator<TEntity>>) validaciones);
      return await this.AddEntityAsync(entity, validationResultEntity);
    }

    public virtual async Task<ValidationResult> AddAsync(
      TEntity entity,
      IValidator<TEntity> validation)
    {
      ValidationResult validationResultEntity = await this.ValidateEntityAsync(entity, validation);
      return await this.AddEntityAsync(entity, validationResultEntity);
    }

    public virtual async Task<ValidationResult> UpdateAsync(
      TEntity entity,
      params IValidator<TEntity>[] validaciones)
    {
      ValidationResult validationResult = await this.ValidateEntityAsync(entity, (IEnumerable<IValidator<TEntity>>) validaciones);
      if (validationResult.IsValid)
        this._repository.Update(entity);
      return validationResult;
    }

    public virtual async Task<ValidationResult> UpdateAsync(
      TEntity entity,
      IValidator<TEntity> validation)
    {
      ValidationResult validationResult = await this.ValidateEntityAsync(entity, validation);
      if (validationResult.IsValid)
        this._repository.Update(entity);
      return validationResult;
    }

    public virtual async Task<ValidationResult> DeleteAsync(
      TEntity entity,
      params IValidator<TEntity>[] validaciones)
    {
      ValidationResult validationResult = await this.ValidateEntityAsync(entity, (IEnumerable<IValidator<TEntity>>) validaciones);
      if (validationResult.IsValid)
        this._repository.Delete(entity);
      return validationResult;
    }

    public virtual async Task<ValidationResult> DeleteAsync(
      TEntity entity,
      IValidator<TEntity> validation)
    {
      ValidationResult validationResult = await this.ValidateEntityAsync(entity, validation);
      if (validationResult.IsValid)
        this._repository.Delete(entity);
      return validationResult;
    }

    public virtual async Task<ValidationResult> AddRangeAsync(
      IEnumerable<TEntity> entities,
      params IValidator<TEntity>[] validaciones)
    {
      ValidationResult validationResult = new ValidationResult();
      if (!(entities is IList<TEntity> entityList))
        entityList = (IList<TEntity>) entities.ToList<TEntity>();
      IList<TEntity> enumerable = entityList;
      foreach (TEntity entity in (IEnumerable<TEntity>) enumerable)
      {
        ValidationResult validationResult1 = await this.ValidateEntityAsync(entity, (IEnumerable<IValidator<TEntity>>) validaciones);
        if (!validationResult1.IsValid)
        {
          validationResult = new ValidationResult((IEnumerable<ValidationFailure>) validationResult1.Errors);
          return validationResult;
        }
      }
      await this._repository.AddRangeAsync((IEnumerable<TEntity>) enumerable);
      return validationResult;
    }

    public virtual async Task<ValidationResult> AddRangeAsync(
      IEnumerable<TEntity> entities,
      IValidator<TEntity> validation)
    {
      ValidationResult validationResult = new ValidationResult();
      if (!(entities is IList<TEntity> entityList))
        entityList = (IList<TEntity>) entities.ToList<TEntity>();
      IList<TEntity> enumerable = entityList;
      foreach (TEntity entity in (IEnumerable<TEntity>) enumerable)
      {
        ValidationResult validationResult1 = await this.ValidateEntityAsync(entity, validation);
        if (!validationResult1.IsValid)
        {
          validationResult = new ValidationResult((IEnumerable<ValidationFailure>) validationResult1.Errors);
          return validationResult;
        }
      }
      await this._repository.AddRangeAsync((IEnumerable<TEntity>) enumerable);
      return validationResult;
    }

    public async Task<ValidationResult> ValidateEntityAsync(
      TEntity entity,
      IValidator<TEntity> validation)
    {
      return validation != null ? await validation.ValidateAsync(entity) : new ValidationResult();
    }

    public async Task<ValidationResult> ValidateEntityAsync(
      TEntity entity,
      IEnumerable<IValidator<TEntity>> validations)
    {
      if (validations == null)
        return new ValidationResult();
      List<ValidationFailure> errors = new List<ValidationFailure>();
      foreach (IValidator<TEntity> validation in validations)
      {
        ValidationResult validationResult = await validation.ValidateAsync(entity);
        if (!validationResult.IsValid)
          errors.AddRange((IEnumerable<ValidationFailure>) validationResult.Errors);
      }
      return new ValidationResult((IEnumerable<ValidationFailure>) errors);
    }

    protected async Task<ValidationResult> AddEntityAsync(
      TEntity entity,
      ValidationResult validationResultEntity)
    {
      if (validationResultEntity.IsValid)
      {
        TEntity entity1 = await this._repository.AddAsync(entity);
      }
      return validationResultEntity;
    }

    public async Task<ValidationResult> UpdateRangeAsync(
      IEnumerable<TEntity> entities,
      params IValidator<TEntity>[] validaciones)
    {
      ValidationResult validationResult = new ValidationResult();
      if (!(entities is IList<TEntity> entityList))
        entityList = (IList<TEntity>) entities.ToList<TEntity>();
      IList<TEntity> enumerable = entityList;
      foreach (TEntity entity in (IEnumerable<TEntity>) enumerable)
      {
        ValidationResult validationResult1 = await this.ValidateEntityAsync(entity, (IEnumerable<IValidator<TEntity>>) validaciones);
        if (!validationResult1.IsValid)
        {
          validationResult = new ValidationResult((IEnumerable<ValidationFailure>) validationResult1.Errors);
          return validationResult;
        }
      }
      this._repository.UpdateRange((IEnumerable<TEntity>) enumerable);
      return validationResult;
    }

    public async Task<ValidationResult> UpdateRangeAsync(
      IEnumerable<TEntity> entities,
      IValidator<TEntity> validation)
    {
      ValidationResult validationResult = new ValidationResult();
      if (!(entities is IList<TEntity> entityList))
        entityList = (IList<TEntity>) entities.ToList<TEntity>();
      IList<TEntity> enumerable = entityList;
      foreach (TEntity entity in (IEnumerable<TEntity>) enumerable)
      {
        ValidationResult validationResult1 = await this.ValidateEntityAsync(entity, validation);
        if (!validationResult1.IsValid)
        {
          validationResult = new ValidationResult((IEnumerable<ValidationFailure>) validationResult1.Errors);
          return validationResult;
        }
      }
      this._repository.UpdateRange((IEnumerable<TEntity>) enumerable);
      return validationResult;
    }

    public Task<int> MaxAsync(Expression<Func<TEntity, int>> selector)
    {
      return this._repository.MaxAsync(selector);
    }

    public async Task<ValidationResult> DeleteRangeAsync(
      IEnumerable<TEntity> entities,
      params IValidator<TEntity>[] validaciones)
    {
      ValidationResult validationResult = new ValidationResult();
      if (!(entities is IList<TEntity> entityList))
        entityList = (IList<TEntity>) entities.ToList<TEntity>();
      IList<TEntity> enumerable = entityList;
      foreach (TEntity entity in (IEnumerable<TEntity>) enumerable)
      {
        ValidationResult validationResult1 = await this.ValidateEntityAsync(entity, (IEnumerable<IValidator<TEntity>>) validaciones);
        if (!validationResult1.IsValid)
        {
          validationResult = new ValidationResult((IEnumerable<ValidationFailure>) validationResult1.Errors);
          return validationResult;
        }
      }
      this._repository.DeleteRange((IEnumerable<TEntity>) enumerable);
      return validationResult;
    }

    public async Task<ValidationResult> DeleteRangeAsync(
      IEnumerable<TEntity> entities,
      IValidator<TEntity> validation)
    {
      ValidationResult validationResult = new ValidationResult();
      if (!(entities is IList<TEntity> entityList))
        entityList = (IList<TEntity>) entities.ToList<TEntity>();
      IList<TEntity> enumerable = entityList;
      foreach (TEntity entity in (IEnumerable<TEntity>) enumerable)
      {
        ValidationResult validationResult1 = await this.ValidateEntityAsync(entity, validation);
        if (!validationResult1.IsValid)
        {
          validationResult = new ValidationResult((IEnumerable<ValidationFailure>) validationResult1.Errors);
          return validationResult;
        }
      }
      this._repository.DeleteRange((IEnumerable<TEntity>) enumerable);
      return validationResult;
    }
  }
}
