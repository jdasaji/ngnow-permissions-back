
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N5now.App.Infrastructure.Elasticsearch.Implements;
using N5now.App.Infrastructure.Elasticsearch.Interfaces;
using N5now.App.Infrastructure.Kafka.Implements;
using N5now.App.Infrastructure.Kafka.Interfaces;
using N5now.App.Permissions.AutoMappers.Configuration;
using N5now.App.Permissions.Common.Exceptions.Middleware;
using N5now.App.Permissions.DataLayer;
using N5now.App.Permissions.Repository.Base;
using N5now.App.Permissions.Repository.Interfaces;
using System;
using System.Reflection;

#nullable enable
namespace N5now.App.Permissions.IOC
{
  public static class IocContainer
  {
    public static IServiceCollection InjectionAppRepository(this IServiceCollection services)
    {
      services.AddScoped<IUnitOfWork<PermissionsContext>, UnitOfWork<PermissionsContext>>();
      services.AddScoped<IPermissionsRepository, PermissionsRepository>();
      return services;
    }

    public static IServiceCollection InjectionAutoMapper(this IServiceCollection services)
    {
      services.AddSingleton<IMapper>(mapperInit.Configure());
      return services;
    }

    private static IServiceCollection InjectionEntityFramework(
      this IServiceCollection services,
      IConfiguration configuration)
    {
      services.AddDbContext<PermissionsContext>((Action<DbContextOptionsBuilder>) (options => options.UseSqlServer(configuration.GetConnectionString("defaultConnection")?.ToString(), (Action<SqlServerDbContextOptionsBuilder>) (b => b.MigrationsAssembly("N5now.App.Permissions.DataLayer").CommandTimeout(new int?(470)))).EnableSensitiveDataLogging()));
      return services;
    }

    public static IServiceCollection InjectionException(this IServiceCollection services)
    {
      services.AddTransient<ExceptionMiddleware>();
      return services;
    }

    private static IServiceCollection InjectionInfrastructureExternal(
      this IServiceCollection services)
    {
      services.AddScoped<IEventProducer, KafkaProducer>();
      services.AddScoped<IElasticsearchService, ElasticsearchService>();
      return services;
    }

    public static IServiceCollection InjectionMediaTr(this IServiceCollection services)
    {
      services.AddMediatR((Action<MediatRServiceConfiguration>) (config => config.RegisterServicesFromAssembly(Assembly.Load("N5now.App.Permissions.Features"))));
      return services;
    }

    public static IServiceCollection InjectionMiddleware(this IServiceCollection services)
    {
      services.AddTransient<ExceptionMiddleware>();
      return services;
    }

    public static IServiceCollection AddInjectionInterface(
      this IServiceCollection services,
      IConfiguration configuration)
    {
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.InjectionMediaTr();
      services.InjectionEntityFramework(configuration);
      services.InjectionAutoMapper();
      services.InjectionAppRepository();
      services.InjectionMiddleware();
      services.InjectionInfrastructureExternal();
      return services;
    }
  }
}
