
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Permissions.Repository.Base
{
  public interface IUnitOfWork<T> where T : class
  {
    void BeginTransaction();

    Task SaveChangesAsync();
  }
}
