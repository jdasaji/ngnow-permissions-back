// Decompiled with JetBrains decompiler
// Type: N5now.App.Permissions.Features.commandhandler.ICommandHandlerBase`2
// Assembly: N5now.App.Permissions.Features, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2969A02-2035-45A3-A835-BB24C57C35F1
// Assembly location: C:\Users\jsdan\OneDrive\Fuentes_Trabajo\n5now-dofuscator\compilado\N5now.App.Permissions.Features.dll

using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Permissions.Features.commandhandler
{
  public interface ICommandHandlerBase<TEntity, in TId> where TEntity : class
  {
    Task<TEntity> GetAsync(
      TId id,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

    IQueryable<TEntity> All(bool @readonly = true);

    IQueryable<TEntity> Find(
      Expression<Func<TEntity, bool>> predicate,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
      bool @readonly = true);

    Task<ValidationResult> AddAsync(TEntity entity, params IValidator<TEntity>[] validaciones);

    Task<ValidationResult> AddAsync(TEntity entity, IValidator<TEntity> validation);

    Task<ValidationResult> UpdateAsync(TEntity entity, params IValidator<TEntity>[] validaciones);

    Task<ValidationResult> UpdateAsync(TEntity entity, IValidator<TEntity> validation);

    Task<ValidationResult> UpdateRangeAsync(
      IEnumerable<TEntity> entity,
      params IValidator<TEntity>[] validaciones);

    Task<ValidationResult> UpdateRangeAsync(
      IEnumerable<TEntity> entity,
      IValidator<TEntity> validation);

    Task<ValidationResult> DeleteAsync(TEntity entity, params IValidator<TEntity>[] validaciones);

    Task<ValidationResult> DeleteAsync(TEntity entity, IValidator<TEntity> validation);

    Task<ValidationResult> DeleteRangeAsync(
      IEnumerable<TEntity> entity,
      params IValidator<TEntity>[] validaciones);

    Task<ValidationResult> DeleteRangeAsync(
      IEnumerable<TEntity> entity,
      IValidator<TEntity> validation);

    Task<ValidationResult> AddRangeAsync(
      IEnumerable<TEntity> entities,
      params IValidator<TEntity>[] validaciones);

    Task<ValidationResult> AddRangeAsync(
      IEnumerable<TEntity> entities,
      IValidator<TEntity> validation);

    Task<ValidationResult> ValidateEntityAsync(TEntity entity, IValidator<TEntity> validation);

    Task<ValidationResult> ValidateEntityAsync(
      TEntity entity,
      IEnumerable<IValidator<TEntity>> validations);

    Task<IEnumerable<TDto>> RunSqlQuery<TDto>(string query);

    Task<IEnumerable<TDto>> RunStoredProcedure<TDto>(
      string storedProcedure,
      object parameters = null,
      int? commandTimeout = null);

    Task<IEnumerable<TReturn>> RunStoredProcedure<TFirst, TSecond, TReturn>(
      string sql,
      Func<TFirst, TSecond, TReturn> map,
      object param = null,
      IDbTransaction transaction = null,
      bool buffered = true,
      string splitOn = "Id",
      int? commandTimeout = null);

    Task ExecuteNonQueryAsync(
      string sqlQuery,
      object parameters = null,
      IDbTransaction transaction = null,
      int? commandTimeout = null);

    Task<int> MaxAsync(Expression<Func<TEntity, int>> selector);
  }
}
