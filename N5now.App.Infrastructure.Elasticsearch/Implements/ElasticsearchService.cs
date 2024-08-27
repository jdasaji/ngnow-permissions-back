
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Options;
using N5now.App.Infrastructure.Elasticsearch.Interfaces;
using N5now.App.Permissions.Domain;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Infrastructure.Elasticsearch.Implements
{
  public class ElasticsearchService : IElasticsearchService
  {
    public ElasticsearchSettings _settings;
    public ElasticsearchClient _elasticClient;

    public ElasticsearchService(IOptions<ElasticsearchSettings> options)
    {
      this._settings = options.Value;
      this._elasticClient = new ElasticsearchClient((IElasticsearchClientSettings) new ElasticsearchClientSettings(new Uri("http://" + this._settings.Hostname + ":" + this._settings.Port)).DefaultIndex("permissions"));
    }

    public async Task<PermissionsDomain> GetDocumentByIdAsync(int id)
    {
      return (await this._elasticClient.SearchAsync<PermissionsDomain>((Action<SearchRequestDescriptor<PermissionsDomain>>) (s => s.Query((Action<QueryDescriptor<PermissionsDomain>>) (q => q.Match((Action<MatchQueryDescriptor<PermissionsDomain>>) (m => m.Field<int>((Expression<Func<PermissionsDomain, int>>) (f => f.Id)).Query((object) id)))))))).Documents.FirstOrDefault<PermissionsDomain>();
    }

    public async Task UpdateDocumentAsync(int id, PermissionsDomain updatedDocument)
    {
      UpdateResponse<PermissionsDomain> updateResponse = await this._elasticClient.UpdateAsync<PermissionsDomain, PermissionsDomain>((IndexName) "permissions", (Id) (long) id, (Action<UpdateRequestDescriptor<PermissionsDomain, PermissionsDomain>>) (u => u.Doc(updatedDocument)));
      if (!updateResponse.IsValidResponse)
        throw new Exception(updateResponse.ElasticsearchServerError.Error.Reason);
      Console.WriteLine("Document indexed successfully");
    }

    public async Task IndexDocumentAsync(PermissionsDomain permission)
    {
      IndexResponse indexResponse = await this._elasticClient.IndexAsync<PermissionsDomain>(permission, (Action<IndexRequestDescriptor<PermissionsDomain>>) (idx => idx.Index((IndexName) "permissions")));
      if (indexResponse.IsValidResponse)
        Console.WriteLine("Document indexed successfully");
      else
        Console.WriteLine("Error indexing document: " + indexResponse.DebugInformation.ToString());
    }
  }
}
