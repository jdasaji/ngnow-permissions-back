
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace N5now.App.Permissions.Features.commandhandler
{
  public static class ValidationFailureExtends
  {
    public static string[] ErrorMessageToArray(this ValidationResult validationResult)
    {
      return validationResult.Errors.Select<ValidationFailure, string>((Func<ValidationFailure, string>) (x => x.ErrorMessage)).ToArray<string>();
    }

    public static List<string> ErrorMessageToList(this ValidationResult validationResult)
    {
      return validationResult.Errors.Select<ValidationFailure, string>((Func<ValidationFailure, string>) (x => x.ErrorMessage)).ToList<string>();
    }
  }
}
