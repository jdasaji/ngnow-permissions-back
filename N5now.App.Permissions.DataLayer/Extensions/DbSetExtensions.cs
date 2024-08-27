
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Permissions.DataLayer.Extensions
{
  public static class DbSetExtensions
  {
    public static async Task<IEnumerable<T>> FromSqlQueryAsync<T>(
      this DbContext dbContext,
      string sqlQuery,
      object parameters = null,
      IDbTransaction transaction = null,
      int? commandTimeout = null,
      CommandType? commandType = null)
    {
      return await dbContext.Database.GetDbConnection().QueryAsync<T>(sqlQuery, parameters, transaction, commandTimeout, commandType);
    }

    public static async Task<IEnumerable<TReturn>> FromSqlQueryAsync<TFirst, TSecond, TReturn>(
      this DbContext dbContext,
      string sql,
      Func<TFirst, TSecond, TReturn> map,
      object param = null,
      IDbTransaction transaction = null,
      bool buffered = true,
      string splitOn = "Id",
      int? commandTimeout = null,
      CommandType? commandType = null)
    {
      return await dbContext.Database.GetDbConnection().QueryAsync<TFirst, TSecond, TReturn>(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
    }

    public static async Task ExecuteNonQueryAsync(
      this DbContext dbContext,
      string sqlQuery,
      object parameters = null,
      IDbTransaction transaction = null,
      int? commandTimeout = null)
    {
      int num = await dbContext.Database.GetDbConnection().ExecuteAsync(sqlQuery, parameters, transaction, commandTimeout, new CommandType?(CommandType.Text));
    }
  }
}
