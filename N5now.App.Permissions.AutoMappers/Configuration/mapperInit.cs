
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable enable
namespace N5now.App.Permissions.AutoMappers.Configuration
{
  public class mapperInit
  {
    public static IMapper Configure()
    {
      Type[] typesProfiles = mapperInit.onGetTypesInNamespace(Assembly.GetExecutingAssembly(), "N5now.App.Permissions.AutoMappers.Profiles");
      return new MapperConfiguration((Action<IMapperConfigurationExpression>) (x =>
      {
        foreach (Type profileType in typesProfiles)
        {
          if (profileType.IsSubclassOf(typeof (Profile)))
            x.AddProfile(profileType);
        }
      })).CreateMapper();
    }

    private static Type[] onGetTypesInNamespace(Assembly assembly, string nameSpace)
    {
      return ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (t => t.Namespace == nameSpace)).ToArray<Type>();
    }
  }
}
