

using System.Collections.Generic;

#nullable enable
namespace N5now.App.Permissions.Common.Exceptions.Middleware
{
  public class ErrorResult
  {
    public List<string> Messages { get; set; } = new List<string>();

    public object Data { get; set; }

    public string? Source { get; set; }

    public string? Exception { get; set; }

    public string? ErrorId { get; set; }

    public string? SupportMessage { get; set; }

    public int StatusCode { get; set; }
  }
}
