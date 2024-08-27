
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Permissions.Repository.Base
{
  public class UnitOfWork<T> : IUnitOfWork<T>, IDisposable where T : DbContext
  {
    private readonly T _dbContext;
    private bool _disposed;

    public UnitOfWork(T dbContext) => this._dbContext = dbContext;

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public void BeginTransaction() => this._disposed = false;

    public Task SaveChangesAsync()
    {
      return (Task) this._dbContext.SaveChangesAsync(new CancellationToken());
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this._disposed && disposing)
        this._dbContext.Dispose();
      this._disposed = true;
    }
  }
}
