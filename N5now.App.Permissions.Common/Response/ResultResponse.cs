
namespace N5now.App.Permissions.Common.Response
{
  public class ResultResponse<T>
  {
    public string CodigoError { get; set; }

    public string Messages { get; set; }

    public T? Data { get; set; }

    public bool IsValid { get; set; }

    public ResultResponse(T? data, string messages = "")
    {
      this.IsValid = true;
      this.Data = data;
      this.Messages = messages;
    }

    public ResultResponse(string codigoError, string messages)
    {
      this.CodigoError = codigoError;
      this.Messages = messages;
      this.IsValid = false;
    }

    public ResultResponse(T error, string? codigoError, string messages = "")
    {
      this.Data = error;
      this.CodigoError = codigoError;
      this.Messages = messages;
      this.IsValid = false;
    }
  }
}
