
using Microsoft.AspNetCore.Http;
using N5now.App.Permissions.Common.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Permissions.Common.Exceptions.Middleware
{
  public class ExceptionMiddleware : IMiddleware
  {
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      try
      {
        await next(context);
      }
      catch (Exception ex)
      {
        string str = Guid.NewGuid().ToString();
        ErrorResult error = new ErrorResult()
        {
          Source = ex.TargetSite?.DeclaringType?.FullName,
          Exception = ex.Message.Trim(),
          ErrorId = str,
          SupportMessage = "Provide the Error Id: " + str + " to the support team for further analysis."
        };
        error.Messages.Add(ex.Message);
        if (!(ex is ExceptionController) && ex.InnerException != null)
        {
          while (ex.InnerException != null)
            ex = ex.InnerException;
        }
        if (ex is ExceptionController exceptionController)
        {
          error.StatusCode = exceptionController.CodeError;
          error.Messages = exceptionController.ErrorMessages;
          error.Data = exceptionController.Data;
        }
        else
        {
          error.Messages = ((IEnumerable<string>) ex.Message.Split("")).ToList<string>();
          error.StatusCode = 500;
        }
        HttpResponse response = context.Response;
        if (!response.HasStarted)
        {
          response.ContentType = "application/json";
          response.StatusCode = 200;
          string text = JsonConvert.SerializeObject((object) new ResultResponse<ErrorResult>(error, error.StatusCode.ToString(), (string) null), new JsonSerializerSettings()
          {
            ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver()
          });
          await response.WriteAsync(text, Encoding.UTF8);
        }
      }
    }
  }
}
