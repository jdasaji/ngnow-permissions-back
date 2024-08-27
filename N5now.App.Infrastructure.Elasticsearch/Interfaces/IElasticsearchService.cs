
using N5now.App.Permissions.Domain;
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Infrastructure.Elasticsearch.Interfaces
{
  public interface IElasticsearchService
  {
    Task IndexDocumentAsync(PermissionsDomain permission);

    Task<PermissionsDomain> GetDocumentByIdAsync(int id);

    Task UpdateDocumentAsync(int id, PermissionsDomain updatedDocument);
  }
}
