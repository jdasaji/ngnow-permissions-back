
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace N5now.App.Permissions.Common.Exceptions
{
  public class ExceptionController : Exception
  {
    public object Data { get; set; }

    public int CodeError { get; set; }

    public List<string> ErrorMessages { get; set; }

    public ExceptionController(string messages, int codeError = 1, object data = null)
      : base(messages)
    {
      this.ErrorMessages = new List<string>() { messages };
      this.CodeError = codeError;
      this.Data = data;
    }

    public ExceptionController(
      List<string> messages,
      int codeError = 1,
      object data = null,
      bool isAppendLine = false)
      : base(string.Join(",", messages.ToArray()))
    {
      if (!isAppendLine)
      {
        this.ErrorMessages = messages;
        this.CodeError = codeError;
        this.Data = data;
      }
      else
      {
        StringBuilder errors = new StringBuilder();
        messages.ForEach((Action<string>) (m =>
        {
          errors.AppendLine(m);
          errors.Append("<br/>");
        }));
        this.ErrorMessages = new List<string>()
        {
          errors.ToString()
        };
        this.CodeError = codeError;
        this.Data = data;
      }
    }
  }
}
